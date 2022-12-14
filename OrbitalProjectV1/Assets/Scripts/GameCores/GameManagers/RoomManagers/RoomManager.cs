using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using UnityEngine.Rendering.Universal;
using EntityCores;
using EntityCores.Enemy;
using EntityCores.Behavioural;
using EntityDataMgt;
using GameManagement.Helpers;
using GameManagement.UIComps;

/// <summary>
/// This is the template RoomManager which manages all entity events happening in the room.
/// </summary>
namespace GameManagement
{

    public abstract class RoomManager : MonoBehaviour, IDataPersistence
    {
        #region Variables
        #region RoomData_VAR
        /**
         * Data Generation
         */
        [SerializeField] private string id;
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected Vector2 roomSize;
        [ContextMenu("Generate id")]
        private void GenerateGuid()
        {
            id = System.Guid.NewGuid().ToString();
        }

        public int RoomIndex;
        protected Player player;
        public enum ROOMTYPE
        {
            PUZZLE_ROOM,
            FIGHTING_ROOM,
            HYBRID_ROOM,
            TREASURE_ROOM,
            SAVE_ROOM,
            PUZZLE2_ROOM,
            ROOMBEFOREBOSS,
            BOSSROOM,
            COUNT,

        }
        public ROOMTYPE roomtype;
        public AstarPath astarPath { get; private set; }

        /**
         * Data.
         */

        public bool terrainGenerated;
        private TerrainGenerator terrainGenerator;
        protected List<Vector3Int> pointsInRoomBound;
        #endregion

        #region EntityBehaviours_VAR
        /**
         * Entity Behaviours
         */
        public EntityData[] _EntityDatas;
        public HashSet<string> conditions { get; protected set; }
        public int conditionSize;
        public List<EntityBehaviour> items { get; protected set; }
        public List<EnemyBehaviour> enemies { get; protected set; }
        public List<EntityData> spawnlater { get; protected set; }
        public List<PressureSwitchBehaviour> pressureitems;
        public LayerMask CollisionObjects;
        public List<NPCBehaviour> npcs { get; protected set; }
        #endregion

        #region UI_VAR
        /*
         * UI
         */
        protected PopUpSettings popUpSettings;
        private PoolManager poolManager;
        public Light2D[] lights;
        protected UITextDescription textDescription;
        protected RoomDesign roomDesigner;
        private UIObjectivePointer pointer;
        [SerializeField] private Color _colour;
        #endregion

        #region Doors_and_Portals_VAR
        /**
         * Prefabs.
         */
        [Header("Doors and Portal")]
        [SerializeField] protected DoorBehaviour pressureSwitchDoor;
        [SerializeField] protected DoorBehaviour entryDoor;
        [SerializeField] protected DoorBehaviour exitDoor;
        [SerializeField] protected DoorBehaviour[] doors;
        [SerializeField] protected ItemWithTextData portal;

        #endregion

        #region Audio_VAR
        private GlobalAudioManager globalAudioManager;
        public AudioClip BossRoomAudio;
        public AudioClip BeforeBossRoomAudio;
        #endregion

        #region RoomComponents_VAR
        /**
         * Room.
         */
        protected Collider2D roomArea;
        protected bool activated;
        protected bool pressureRoomComplete;
        protected int startNum;
        private Vector2 areaminBound;
        private Vector2 areamaxBound;
        protected DialogueManager dialMgr;
        private TypingTestTL typingTestTL;
        protected Collider2D _collider;
        private HashSet<Vector3> safeRoute;
        #endregion
        #endregion

        #region MonoBehaviour
        /**
         * Retrieving of Data.
         */
        protected virtual void Awake()
        {
            activated = false;
            conditions = new HashSet<string>();
            items = new List<EntityBehaviour>();
            enemies = new List<EnemyBehaviour>();
            npcs = new List<NPCBehaviour>();
            pressureitems = new List<PressureSwitchBehaviour>();
            popUpSettings = FindObjectOfType<PopUpSettings>(true);
            astarPath = FindObjectOfType<AstarPath>(true);
            spawnlater = new List<EntityData>();
            GenerateGuid();
            _colour = Color.red;


        }

        protected virtual void Start()
        {
            roomArea = GetComponent<Collider2D>();
            player = Player.instance;
            dialMgr = DialogueManager.instance;
            typingTestTL = TypingTestTL.instance;
            poolManager = PoolManager.instance;
            textDescription = UITextDescription.instance;
            pointer = UIObjectivePointer.instance;
            globalAudioManager = GlobalAudioManager.instance;
            roomDesigner = RoomDesign.instance;
            terrainGenerator = TerrainGenerator.instance;
            this.areaminBound = roomArea.bounds.min;
            this.areamaxBound = roomArea.bounds.max;
            terrainGenerated = false;
            terrainGenerator = TerrainGenerator.instance;
            LightUpLights();


        }

        private void LightUpLights()
        {
            if (lights == null)
            {
                return;
            }
            foreach (Light2D light in lights)
            {

                light.GetComponent<Animator>().SetBool(light.name, true);
            }
        }

        protected void OnDisable()
        {
            //UnlockDoorsOnThisLevel();
            DeActivateAStar();
        }

        protected virtual void Update()
        {
            DecorateRoom();
            if (activated == false)
            {
                _collider = Physics2D.OverlapBox(transform.position, roomSize, 0, LayerMask.GetMask("Player"));
                if (_collider != null)
                {
                    activated = true;
                    if (pointsInRoomBound == null)
                    {
                        GetSpawnablePointsInRoom(); //for level1 and level2;
                    }
                    InitializeAStar();
                    SettingDialogueMgr();
                    SettingUpAudio();
                    SpawnObjects(_EntityDatas);
                    StartEnemyDialogueIfAny();

                }

            }

            conditionSize = conditions.Count;
        }

        public void UpdateSpawnablePoints()
        {
            List<Vector3Int> newpoints = new List<Vector3Int>(pointsInRoomBound);
            foreach (Vector3Int possiblepoint in pointsInRoomBound)
            {
                if (Physics2D.OverlapCircle((Vector2Int)possiblepoint, 2, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior")))
                {
                    newpoints.Remove(possiblepoint);

                }
                else if (terrainGenerator != null)
                {

                    if (terrainGenerator.unWalkableTiles.Contains(possiblepoint))
                    {
                        newpoints.Remove(possiblepoint);
                    }

                }
            }
            //pointsInRoomBound = newpoints;
        }

        private void StartEnemyDialogueIfAny()
        {
            foreach (EnemyBehaviour enemy in enemies)
            {
                enemy.StartDialogue();


            }
        }



        #endregion

        #region Internal Methods
        #region RoomSettings
        private void DecorateRoom()
        {
            if (terrainGenerated)
            {
                return;
            }
            else
            {
                if (roomDesigner != null)
                {
                    roomDesigner.GenerateRoomDesign(roomtype, this);
                    terrainGenerated = true;
                }
            }

        }

        private void RemovalOfTerrainWalls()
        {
            foreach (DoorBehaviour door in doors)
            {
                if (door != null)
                {
                    door.RemoveTerrainWalls();
                }
            }
        }

        private void SettingUpAudio()
        {
            if (globalAudioManager == null)
            {
                return;
            }

            switch (roomtype)
            {
                default:
                    globalAudioManager.PlayTrack(this.RoomIndex);
                    break;
                case ROOMTYPE.ROOMBEFOREBOSS:
                    if (BeforeBossRoomAudio == null)
                    {
                        Debug.LogError("BeforeBossRoom lacking audio");
                    }

                    globalAudioManager.PlaySpecificTrack(BeforeBossRoomAudio, 0.5f);
                    break;
                case ROOMTYPE.BOSSROOM:
                    if (BossRoomAudio == null)
                    {
                        Debug.LogError("BossRoom lacking audio");
                    }
                    globalAudioManager.PlaySpecificTrack(BossRoomAudio, 0.5f);
                    break;
            }

        }

        /**
         * Setting Up dialogue Manager's room.
         */
        private void SettingDialogueMgr()
        {
            if (dialMgr != null)
            {
                dialMgr.SetCurrentRoom(this);

            }

        }

        /**
         * Setting Up ExitDoor.
         */
        public void SettingExitDoor(DoorBehaviour door)
        {
            if (doors == null)
            {
                doors = new DoorBehaviour[25];
            }
            for (int i = 0; i < 25; i++)
            {
                if (doors[i] == null)
                {
                    doors[i] = door;
                    return;
                }
            }
        }

        #endregion

        #region AStarGraph

        /**
         * Initialize AStarPath.
         */
        private void InitializeAStar()
        {
            if (roomtype != ROOMTYPE.FIGHTING_ROOM && roomtype != ROOMTYPE.HYBRID_ROOM && roomtype != ROOMTYPE.BOSSROOM)
            {
                return;
            }
            astarPath = gameObject.AddComponent<AstarPath>();
            AstarData astarData = AstarPath.active.data;
            GridGraph gg = astarData.AddGraph(typeof(GridGraph)) as GridGraph;
            gg.center = transform.position;
            gg.is2D = true;
            gg.SetDimensions((int)(areamaxBound.x - areaminBound.x), (int)(areamaxBound.y - areaminBound.y), 1f);
            gg.collision.use2D = true;
            gg.collision.mask = CollisionObjects;
            AstarPath.active.Scan(gg);
            AstarPath.active.logPathResults = Pathfinding.PathLog.None;

        }

        /**
         * DeActivate Astar.
         */
        private void DeActivateAStar()
        {

            AstarPath astar = gameObject.GetComponent<AstarPath>();
            if (astar != null)
            {
                Destroy(astar);
            }

        }

        #endregion

        #region Spawning Methods
        /**
         * SpawnObject inside the room.
         */
        public void SpawnObject(EntityData entitydata)
        {
            if (entitydata != null)
            {
                if (entitydata.condition == 1)
                {
                    conditions.Add(entitydata._name + entitydata.GetInstanceID());
                }
                if (entitydata.spawnAtStart)
                {

                    InstantiateEntity(entitydata);
                }

            }
        }

        /**
         * Spawn Portal at the end of the round.
         */
        public IEnumerator SpawnEndCredit(GameObject go)
        {
            while (pointsInRoomBound == null)
            {
                yield return null;
            }

            Transform tr = Instantiate(go, GetRandomObjectPoint(), Quaternion.identity, transform).transform;
            MiniMapImage.instance.minimapIcons.Add(tr.Find("ShipIcon"));
        }


        /**
         * SpawnObjects inside the room.
         */
        public void SpawnObjects(EntityData[] entityDatas)
        {
            if (entityDatas == null)
            {

                return;
            }
            for (int i = 0; i < entityDatas.Length; i++)
            {

                EntityData _item = entityDatas[i];
                if (_item == null)
                {
                    return;
                }
                if (_item.condition == 1)
                {

                    conditions.Add(_item._name + _item.GetInstanceID());
                }

                if (!_item.spawnAtStart)
                {
                    spawnlater.Add(_item);
                    continue;
                }
                else
                {

                    if (_item.multispawns)
                    {
                        if (_item.random)
                        {
                            InstantiateMultiPositionRandom(_item);
                        }
                        else
                        {
                            InstantiateMultiPosition(_item);
                        }

                    }
                    else
                    {

                        InstantiateEntity(_item);
                    }

                }


                /*
                initprop.SetEntityStats(_item);
                initprop.SetCurrentRoom(this); */
            }
        }

        /**
         * Spawn instant without staggering.
         */
        private void SpawnInstant(List<Vector2> points, EntityData item)
        {
            foreach (Vector2 point in points)
            {
                EntityData copy = Instantiate(item);
                EntityBehaviour entity = poolManager.GetObject(item._type);
                InitializeEntity(copy, point, entity);
            }
        }

        /**
         * Spawn objects staggered.
         */
        private IEnumerator SpawnStaggered(List<Vector2> points, EntityData item)
        {
            foreach (Vector2 point in points)
            {
                EntityData copy = Instantiate(item);
                EntityBehaviour entity = poolManager.GetObject(item._type);
                entity.spriteRenderer.color = copy.defaultcolor;
                InitializeEntity(copy, point, entity);
                yield return new WaitForSeconds(item.staggerTime);

            }
        }

        #endregion

        #region Entities Settings

        #region Initializers
        /**
         * Instantiating single entity.
         */
        public void InstantiateEntity(EntityData data)
        {
            //Vector2 pos = data.random ? GetRandomObjectPointGivenSize(data.sprite.bounds.size.magnitude) : data.pos;
            Vector2 pos = data.random ? GetRandomObjectPoint() : data.pos;
            EntityBehaviour entity = poolManager.GetObject(data._type);
            InitializeEntity(data, pos, entity);

        }

        /**
        * Instantiate MultiplePositionals.
        */
        protected void InstantiateMultiPosition(EntityData item)
        {
            List<Vector2> points;
            Patterns pattern = Patterns.of(item.startPos, item.endPos);
            switch (item.pattern)
            {
                default:
                case EntityData.PATTERN.BOX_LINE:
                    points = pattern.Box();
                    break;
                case EntityData.PATTERN.CROSS:
                    points = pattern.Cross();
                    break;
                case EntityData.PATTERN.SIMPLE_DIAG:
                    points = pattern.Diagonal();
                    break;
                case EntityData.PATTERN.PATTERN1:
                    points = pattern.Pattern1();
                    break;
                case EntityData.PATTERN.PATTERN2:
                    points = pattern.Pattern2();
                    break;
                case EntityData.PATTERN.PATTERN3:
                    points = pattern.Pattern3();
                    break;
            }

            //points.Sort();
            if (item.staggered)
            {
                StartCoroutine(SpawnStaggered(points, item));
            }
            else
            {
                SpawnInstant(points, item);
            }


        }

        /**
       * Instantiate MultiplePositionalsRandom.
       */
        protected void InstantiateMultiPositionRandom(EntityData item)
        {
            List<Vector2> points = new List<Vector2>();
            int rand = Random.Range(25, 35);
            for (int i = 0; i < rand; i++)
            {
                points.Add((Vector2)GetRandomObjectPoint());
            }
            //points.Sort();
            //Patterns pattern = Patterns.of(points[0],points[rand-1]);
            //switch (item.pattern)
            //{
            //    default:
            //    case EntityData.PATTERN.BOX_LINE:
            //        points = pattern.Box();
            //        break;
            //    case EntityData.PATTERN.CROSS:
            //        points = pattern.Cross();
            //        break;
            //    case EntityData.PATTERN.SIMPLE_DIAG:
            //        points = pattern.Diagonal();
            //        break;
            //    case EntityData.PATTERN.PATTERN1:
            //        points = pattern.Pattern1();
            //        break;
            //    case EntityData.PATTERN.PATTERN2:
            //        points = pattern.Pattern2();
            //        break;
            //    case EntityData.PATTERN.PATTERN3:
            //        points = pattern.Pattern3();
            //        break;
            //}

            //points.Sort();
            if (item.staggered)
            {
                StartCoroutine(SpawnStaggered(points, item));
            }
            else
            {
                SpawnInstant(points, item);
            }


        }

        /**
         * Initializing Entities.
         */

        private void InitializeEntity(EntityData data, Vector2 pos, EntityBehaviour entity)
        {
            SettingDefaults(data, pos, entity);
            switch (data._type)
            {
                default:
                    entity.transform.SetParent(transform);
                    items.Add(entity);
                    break;
                case EntityData.TYPE.PRESSURE_SWITCH:
                    entity.transform.SetParent(transform);
                    pressureitems.Add((PressureSwitchBehaviour)entity);
                    break;
                case EntityData.TYPE.NPC:
                case EntityData.TYPE.NPCMOVABLE:
                    entity.transform.SetParent(transform);
                    SettingUpNPCObject((NPCData)data);
                    npcs.Add((NPCBehaviour)entity);
                    break;
                case EntityData.TYPE.ENEMY:
                case EntityData.TYPE.BOSS:
                    SettingUpEnemy((EnemyData)data, pos, (EnemyBehaviour)entity);
                    break;

            }
            entity.gameObject.SetActive(true);
        }
        #endregion

        #region Entities Data Setting
        // <summary>
        // Setting Datas for entities.
        // </summary>

        /**
         * Setting general data for entities.
         */
        private void SettingDefaults(EntityData data, Vector2 pos, EntityBehaviour entity)
        {
            entity.SetEntityStats(data);
            //entity.GetComponent<SpriteRenderer>().sprite = data.sprite;
            entity.transform.position = pos;
            entity.transform.localScale = new Vector2(data.scale, data.scale);
            entity.SetCurrentRoom(this);


        }

        /**
         * Setting NPC datas
         */

        private void SettingUpNPCObject(NPCData data)
        {
            UnfulfillCondition(data._npcAction.ToString());
            switch (data._npcAction)
            {
                case NPCData.NPCActions.DEFAULT:
                    FulfillCondition(data._npcAction.ToString());
                    break;
                case NPCData.NPCActions.TYPINGTEST:
                    TypingTestTL _tl = FindObjectOfType<TypingTestTL>(true);
                    _tl.SetCurrentRoom(this);
                    break;
            }



        }

        /**
         * Setting Enemies datas
         */

        private void SettingUpEnemy(EnemyData data, Vector2 pos, EnemyBehaviour emf)
        {
            GameObject go = new GameObject(data._name);
            go.layer = LayerMask.NameToLayer("enemy");
            go.transform.position = pos;
            go.AddComponent<CapsuleCollider2D>();
            Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0f;
            rb.drag = 1.5f;
            rb.mass = 100f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.freezeRotation = true;
            emf.transform.SetParent(go.transform);
            emf.transform.localScale = new Vector2(data.scale, data.scale);
            emf.transform.localPosition = Vector3.zero;
            //emf.animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", data.animatorname)) as RuntimeAnimatorController;
            go.transform.SetParent(transform);
            enemies.Add(emf);
        }
        #endregion

        #endregion

        #region Pause/Resume
        /**
         * Resume game.
         */
        public void ResumeGame()
        {


            EntityBehaviour[] entities = GetComponentsInChildren<EntityBehaviour>();
            foreach (EntityBehaviour entity in entities)
            {
                Freezable freezable = entity.GetComponent<Freezable>();
                if (freezable != null && !entity.Debuffed)
                {
                    freezable.UnFreeze();
                }
                //_enemy.enabled = false;
                //_enemy.animator.enabled = false;

            }

            if (!player.insidePuzzle)
            {
                player.UnFreeze();
                player.enabled = true;
            }

        }

        /**
         * Pause game.
         */
        public void PauseGame()
        {
            EntityBehaviour[] entities = GetComponentsInChildren<EntityBehaviour>();
            foreach (EntityBehaviour entity in entities)
            {
                Freezable freezable = entity.GetComponent<Freezable>();
                if (freezable != null)
                {
                    freezable.Freeze();
                }

            }

            if (!player.insidePuzzle)
            {
                player.Freeze();
                player.enabled = false;
            }

        }
        #endregion

        #region Collider-Related methods
        /**
         * Initialize rooom when is player is first detected.
         * @param collision 
         * Check whether is player.
         */
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (!this.enabled)
            {

                return;
            }

            else
            {



                if (collision.CompareTag("Player") || collision.CompareTag("Stealth"))

                {

                    if (player.GetCurrentRoom() != this)
                    {
                        if (textDescription != null && textDescription.isActiveAndEnabled)
                        {
                            textDescription.StartDescription(this.name);

                        }
                        pointer.StopNavi();
                        player.SetCurrentRoom(this);
                    }
                    if (!CanProceed())
                    {

                        if (pressureRoomComplete)
                        {

                            pressureSwitchDoor.unlocked = false;
                        }

                        LockDoorsOnThisLevel();
                    }
                }
            }

        }

        /**
         * Disable room collider.
         */
        public void DisableCollider()
        {
            this.roomArea.enabled = false;
        }

        #endregion

        #region Doors Handling
        /**
         * Lock all doors controlled by current room.
         */
        protected void LockDoorsOnThisLevel()
        {
            if (activated)
            {
                foreach (DoorBehaviour door in doors)
                {
                    if (door != null)
                    {
                        door.unlocked = false;
                    }

                }

            }

        }

        /**
         * Unlock all doors controlled by current room.
         */
        protected void UnlockDoorsOnThisLevel()
        {

            foreach (DoorBehaviour door in doors)
            {
                if (door != null)
                {
                    door.unlocked = true;
                }
            }

        }

        /**
         * Get all doors controlled by current room.
         */

        public DoorBehaviour[] GetDoors()
        {
            return this.doors;
        }

        /** TempDoor Opener.
         * Open doors temporary for pressureplates.
         */
        private IEnumerator OpenDoorsTemp(float duration)
        {
            pressureSwitchDoor.unlocked = true;
            yield return new WaitForSeconds(duration);
            pressureSwitchDoor.unlocked = conditions.Count == 0;
        }
        #endregion

        #region Room Conditions Checkers
        // <summary>
        // Room Conditions Checker. 
        // </summary>

        /**
         * NPC Prerequisite Checker.
         */
        protected void CheckNPCPrereq()
        {
            foreach (NPCBehaviour npc in npcs)
            {
                NPCData _data = npc.GetData() as NPCData;
                if (_data.prereq != null)
                {
                    if (!conditions.Contains(_data.prereq._name + _data.prereq.GetInstanceID()))
                    {
                        npc.Proceed();
                    }
                }
            }
        }

        /**
         * PressurePlate Checker.
         */
        protected void PressurePlateCheck()
        {
            if (activated)
            {

                if (pressureitems.Count != 0 && pressureitems.TrueForAll(item => item.IsOn()))
                {

                    float duration = pressureitems[0].data.duration;
                    StartCoroutine(OpenDoorsTemp(duration));
                }

                if (conditions.Count == 0 && !pressureRoomComplete)
                {

                    PressureRoomAfterAction();
                    pressureSwitchDoor.unlocked = true;

                }
            }
        }

        /**
         * WaveRoom Checker.
         */
        protected virtual void FightRoomCheck(int waveNum)
        {
            if (activated)
            {
                if (CheckEnemiesDead() && startNum <= waveNum)
                {
                    int rand = Random.Range(3, 5);
                    startNum++;
                    if (textDescription.isActiveAndEnabled)
                    {
                        textDescription.StartDescription("Wave " + startNum);
                    }

                    for (int i = 0; i < rand; i++)
                    {
                        EntityData entitydata = _EntityDatas[Random.Range(0, _EntityDatas.Length)];
                        SpawnObject(entitydata);
                    }

                }

            }
        }

        protected virtual bool CanProceed()
        {
            if (activated)
            {
                return conditions.Count == 0 && CheckEnemiesDead();
            }
            else
            {
                return false;
            }

        }

        protected virtual bool ConditionsCleared()
        {
            return conditions.Count == 0;
        }

        /**
         * Check that all enemies are dead.
         * @return true when enemies are dead.
         */
        protected bool CheckEnemiesDead()
        {
            return enemies.TrueForAll(enemy => enemy.isDead);
        }


        /**
        * Pause/Unpause game when dialogue is/is not running.
        */
        protected virtual void CheckRunningEvents()
        {
            if (activated)
            {
                if (dialMgr.playing || typingTestTL.isActiveAndEnabled || popUpSettings.gameObject.activeInHierarchy)
                {

                    PauseGame();
                }
                else
                {
                    ResumeGame();

                }

            }

        }

        /**
         * Check If Room Proceedable.
         */
        protected virtual void RoomChecker()
        {

            if (CanProceed())
            {
                for (int i = 0; i < doors.Length; i++)
                {
                    if (doors[i] != null)
                    {
                        doors[i].unlocked = true;
                    }



                }
                DisableTrapBehaviour();
                pointToObjective();
                ActivateNextRoom();
                //SpawnPortal();

                if (textDescription.isActiveAndEnabled)
                {
                    textDescription.StartDescription("You hear a loud creak..");
                }
                this.enabled = false;
            }

        }

        private void ActivateNextRoom()
        {
            if (_GameManager.instance != null)
            {
                _GameManager.instance.EnableRoom(RoomIndex + 1);
            }
        }
        #endregion

        #region De-Activation Behaviour
        /**
         * PressureRoom afterAction.
         */
        private void PressureRoomAfterAction()
        {
            if (!pressureRoomComplete)
            {
                spawnlater.ForEach(spawn =>
                {
                    EntityData ed = Instantiate(spawn);
                    ed.spawnAtStart = true;
                    SpawnObject(ed);
                });
                pressureRoomComplete = true;

            }
        }

        /**
        * Destroy all boss spawns.
        */
        public void DestroyAllFodders()
        {
            FodderStationary[] fodders = GetComponentsInChildren<FodderStationary>(true);
            fodders.ToList().ForEach(fodder => poolManager.ReleaseObject(fodder));

        }

        /**
         * Destroy all boss props.
         */
        public void DestroyBossProps()
        {
            ItemWithTextBehaviour[] bossprops = GetComponentsInChildren<ItemWithTextBehaviour>(true);
            bossprops.ToList().ForEach(bossprop =>
            {
                if (bossprop.GetData()._type == EntityData.TYPE.BOSSPROPS)
                {
                    ItemWithTextData _data = bossprop.GetData() as ItemWithTextData;
                    conditions.Remove(_data._name + _data.GetInstanceID());
                    poolManager.ReleaseObject(bossprop);
                }
            }
            );
        }

        /**
         * Disable All Trap Behaviours.
         */
        private void DisableTrapBehaviour()
        {
            TrapBehaviour[] trapBehaviours = GetComponentsInChildren<TrapBehaviour>();
            TrapMovementBehaviour[] trapMovementBehaviours = GetComponentsInChildren<TrapMovementBehaviour>();
            if (trapBehaviours != null)
            {
                foreach (TrapBehaviour trap in trapBehaviours)
                {
                    trap.enabled = false;
                }
            }

            if (trapMovementBehaviours != null)
            {
                foreach (TrapMovementBehaviour trapmvmt in trapMovementBehaviours)
                {
                    trapmvmt.enabled = false;
                }
            }

        }
        #endregion

        #region UI-Related

        /** Navigator Reset.
         *  Restart the navigation.
         */
        private void RestartNavigation(GameData data)
        {
            pointer.StopNavi();
            Dictionary<string, int> dict = data.rooms;
            int lastroomid = 999;
            string lastroomname = "";
            foreach (KeyValuePair<string, int> pair in dict)
            {

                if (pair.Value != 1)
                {
                    string currroomname = pair.Key;
                    int index = currroomname.LastIndexOf(":") + 1;
                    int currroomid;
                    if (int.TryParse(currroomname.Substring(index), out currroomid))
                    {
                        if (currroomid < lastroomid)
                        {
                            lastroomid = currroomid;
                            lastroomname = currroomname.Substring(0, index - 1);

                        }
                    }
                }
            }
            RoomManager lastroom = GameObject.Find(lastroomname).GetComponent<RoomManager>();
            pointer.StartNavi(lastroom.entryDoor.transform.position);

        }

        /**
         * Roomsize Drawing.
         */
        private void OnDrawGizmos()
        {
            Gizmos.color = _colour;
            Gizmos.DrawWireCube(transform.position, roomSize);


        }

        /**
         * Initialize pointer.
         */
        private void pointToObjective()
        {
            if (exitDoor != null)
            {
                pointer.StartNavi(exitDoor.transform.position);
            }

        }
        #endregion

        #endregion

        #region Client-Access Methods

        #region Setters
        public void SetUpEntityDatas(EntityData[] entityDatas)
        {
            _EntityDatas = entityDatas;
        }

        public void SetUpEntityData(EntityData entityData)
        {
            if (_EntityDatas == null)
            {
                _EntityDatas = new EntityData[20];
            }


            // basically shudve used list from the start... but ok i dont want to reinitialize the tut_level1 so i will just do this.
            for (int i = 0; i < _EntityDatas.Length; i++)
            {
                if (_EntityDatas[i] == null)
                {
                    _EntityDatas[i] = entityData;
                    break;
                }
            }

        }

        public void SetUpRoomSize(Vector2Int size)
        {
            roomSize = size;
        }

        public void SetUpPortal(ItemWithTextData portalData)
        {
            portal = portalData;
        }

        #endregion

        #region Getters
        /**
         * Get current room bounds.
         */
        public Bounds GetSpawnAreaBound()
        {
            return roomArea.bounds;
        }

        public EntityData GetBossData()
        {
            if (roomtype == ROOMTYPE.BOSSROOM)
            {
                return _EntityDatas.First(entity => entity._type == EntityData.TYPE.ENEMY);
            }
            else
            {
                throw new MissingComponentException();
            }
        }

        public Vector2Int GetRoomSize()
        {
            return (Vector2Int)new Vector2Int((int)roomSize.x, (int)roomSize.y);
        }


        #endregion

        #region Room-Fulfilling
        /**
         * Fulfill condiitons for current room.
         */
        public virtual void FulfillCondition(string key)
        {
            if (conditions.Contains(key))
            {
                conditions.Remove(key);
            }
        }

        /**
         * Unfulfill condiitons for current room.
         */
        public virtual void UnfulfillCondition(string key)
        {
            if (!conditions.Contains(key))
            {
                conditions.Add(key);
            }

        }
        #endregion

        #region RandomPoint Helpers
        /** 
         * Get a random position inside 2 given vectors.
         * @param minBound, maxBound
         * Boundaries of the room. 
         * @return get randompoint within the radius.
         */
        public Vector2 GetRandomPoint(Vector2 minArea, Vector2 maxArea)
        {

            Vector2 randomPoint;
            do
            {
                randomPoint = new Vector2(
                Random.Range(minArea.x, maxArea.x),
                Random.Range(minArea.y, maxArea.y));
            } while (!roomArea.OverlapPoint(randomPoint) && Physics2D.OverlapCircle(randomPoint, 1, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior", "PassableDeco"))); //&& !Physics2D.OverlapCircle(randomPoint, 2, LayerMask.GetMask("Obstacles"))
                                                                                                                                                                                        //} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
            return randomPoint;
        }

        public Vector2 GetRandomPointMovement()
        {
            Vector3Int randomPoint = pointsInRoomBound[Random.Range(0, pointsInRoomBound.Count)];
            return (Vector2Int)randomPoint;
        }

        /** 
         * Get a random position inside the room.
         * @param minBound, maxBound
         * Boundaries of the room. 
         * @return get randompoint within the radius.
         */
        public Vector2 GetRandomObjectPoint()
        {
            if (pointsInRoomBound.Count == 0)
            {
                Debug.Log("Not enough points");
                return Vector2.zero;
            }
            Vector3Int randomPoint;
            do
            {
                randomPoint = pointsInRoomBound[Random.Range(0, pointsInRoomBound.Count)];
                pointsInRoomBound.Remove(randomPoint);
            } while (!roomArea.OverlapPoint((Vector2Int)randomPoint) || Physics2D.OverlapCircle((Vector2Int)randomPoint, 2, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior", "PassableDeco")));

            return (Vector2Int)randomPoint;
            //Vector2 randomPoint;
            //int iterations = 1000;
            //Debug.Log(terrainGenerator);
            ////not sure if using randompoint will be computationally cheaper than getting storing all positions of room and filtering out walkable areas. 
            //do
            //{
            //    randomPoint = new Vector2(
            //    Random.Range(areaminBound.x, areamaxBound.x),
            //    Random.Range(areaminBound.y, areamaxBound.y));
            //    Debug.Log(terrainGenerator.unWalkableTiles.Contains(new Vector2(randomPoint.x,randomPoint.y)));
            //} while (!roomArea.OverlapPoint(randomPoint)
            //|| Physics2D.OverlapCircle(randomPoint, 2, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior"))
            //|| terrainGenerator == null ? false : terrainGenerator.unWalkableTiles.Contains(new Vector3Int((int)randomPoint.x,(int)randomPoint.y))); //&& !Physics2D.OverlapCircle(randomPoint, 2, LayerMask.GetMask("Obstacles"))
            ////} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
            //return randomPoint;
        }

        public void GetSpawnablePointsInRoom()
        {
            pointsInRoomBound = new List<Vector3Int>();
            for (int i = (int)areaminBound.x; i < (int)areamaxBound.x; i++)
            {
                for (int j = (int)areaminBound.y; j < (int)areamaxBound.y; j++)
                {
                    var possiblepoint = new Vector3Int(i, j);


                    if (!Physics2D.OverlapCircle((Vector2Int)possiblepoint, 2, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior", "PassableDeco")))
                    {
                        if (terrainGenerator == null)
                        {
                            pointsInRoomBound.Add(possiblepoint);

                        }
                        else
                        {
                            if (!terrainGenerator.unWalkableTiles.Contains(possiblepoint))
                            {
                                pointsInRoomBound.Add(possiblepoint);
                            }

                        }
                    }

                }

            }
        }


        /**
        * Get A random point where no overlap between obstacles given a size.
        */
        public Vector2 GetRandomObjectPointGivenSize(float size)
        {
            Vector2 randomPoint;
            int iterations = 1000;
            do
            {
                randomPoint = new Vector2(
                Random.Range(areaminBound.x + 4f, areamaxBound.x - 4f),
                Random.Range(areaminBound.y + 4f, areamaxBound.y - 4f));
                iterations--;
            } while (!roomArea.OverlapPoint(randomPoint) && Physics2D.OverlapCircle(randomPoint, size, LayerMask.GetMask("Obstacles", "PassableDeco")) && iterations > 0); //&& !Physics2D.OverlapCircle(randomPoint, 2, LayerMask.GetMask("Obstacles"))
                                                                                                                                                                           //} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
            return randomPoint;

            //not sure if using randompoint will be computationally cheaper than getting storing all positions of room and filtering out walkable areas. 
            //Vector3Int randomPoint;

            //do
            //{
            //    randomPoint = pointsInRoomBound[Random.Range(0, pointsInRoomBound.Count)];
            //} while (Physics2D.OverlapCircle((Vector2Int) randomPoint, (int) size, LayerMask.GetMask("Obstacles", "HouseExterior", "HouseInterior"))); //&& !Physics2D.OverlapCircle(randomPoint, 2, LayerMask.GetMask("Obstacles"))
            ////} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
            //if (randomPoint == Vector3Int.zero)
            //{
            //    throw new UnityException("No randompoints found");
            //} else
            //{
            //    pointsInRoomBound.Remove(randomPoint);
            //    return (Vector2Int) randomPoint;
            //}
        }

        /**
         * Get A random point where no overlap between obstacles.
         */
        public Vector2Int GetRandomTilePointGivenPoints(Vector2Int minArea, Vector2Int maxArea, bool insideArea, LayerMask layerMask)
        {

            Vector2Int randomPoint;
            do
            {
                randomPoint = new Vector2Int(
                Random.Range(minArea.x, maxArea.x),
                Random.Range(minArea.y, maxArea.y));
            } while (insideArea ? !Physics2D.OverlapPoint(randomPoint, layerMask) : Physics2D.OverlapPoint(randomPoint, layerMask));
            //} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
            return randomPoint;
            //do
            //{
            //    randomPoint =  pointsInRoomBound[Random.Range(0, pointsInRoomBound.Count)];
            //} while (insideArea ? !Physics2D.OverlapPoint((Vector2Int) randomPoint, layerMask) : Physics2D.OverlapPoint((Vector2Int) randomPoint, layerMask)); //&& !Physics2D.OverlapCircle(randomPoint, 2, LayerMask.GetMask("Obstacles"))
            ////} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
            //if (randomPoint == Vector3Int.zero)
            //{
            //    throw new UnityException("No randompoints found");
            //}
            //else
            //{
            //    pointsInRoomBound.Remove(randomPoint);
            //    return (Vector2Int)randomPoint;
            //}
        }


        #endregion

        #region Game Save/Loads
        /** Healpoints reactivation.
         *  ReActivate All HealingPoints upon load.
         */
        private void ActivatedHealPoints(GameData data)
        {
            HealingBehaviour healingBehaviour = GetComponentInChildren<HealingBehaviour>();
            if (healingBehaviour != null && data.rooms.GetValueOrDefault(this.name + ":" + RoomIndex, -1) == 1)
            {
                healingBehaviour.enabled = true;
            }
        }

        /** SaveData.
         * Saving RoomData.
         */
        public void SaveData(ref GameData data)
        {
            if (this.enabled)
            {
                if (!CanProceed())
                {
                    data.rooms[this.name + ":" + RoomIndex] = 0;
                    return;
                }
            }

            data.rooms[this.name + ":" + RoomIndex] = 1;

        }

        /** LoadData.
         * Load RoomData from cloud.
         */
        public void LoadData(GameData data)
        {
            ActivatedHealPoints(data);
            if (this.roomtype != ROOMTYPE.SAVE_ROOM)
            {
                if (data.rooms.GetValueOrDefault(this.name + ":" + RoomIndex, -1) == 1)
                {
                    this.enabled = false;
                }
            }

            if (pointer.isActiveAndEnabled)
            {
                RestartNavigation(data);
            }


        }
        #endregion

        #endregion
    }
}

#region Unused Methods

/**
    //* SafePath for room.
    //*/
//private void SafePath()
//{
//    safeRoute = new HashSet<Vector3>();

//    var startPosition = entryDoor.transform.position;
//    foreach (DoorBehaviour door in doors)
//    {
//        Vector2 _doorpos = door.transform.position;
//        //if (_doorpos.x > roomArea.bounds.max.x)
//        //{
//        //    _doorpos.x = roomArea.bounds.max.x;
//        //}
//        //else if (_doorpos.x < roomArea.bounds.min.x)
//        //{
//        //    _doorpos.x = roomArea.bounds.min.x;
//        //}

//        //if (_doorpos.y > roomArea.bounds.max.y)
//        //{
//        //    _doorpos.y = roomArea.bounds.max.y;
//        //}
//        //else if (_doorpos.y < roomArea.bounds.min.y)
//        //{
//        //    _doorpos.y = roomArea.bounds.min.y;
//        //
//        //Debug.Log(ABPath.Construct(GetRandomPoint(), GetRandomPoint()).vectorPath.Count);
//        safeRoute.UnionWith(ABPath.Construct(startPosition, door.transform.position).vectorPath);
//    }

//}

/**
     * Storing conditional NPCs.
     */
/*
protected void AddConditionalNPCS()
{
    foreach (NPCData _npcd in _npcData)
    {
        int conditional = _npcd.condition;
        if (conditional == 1)
        {
            conditions.Add(_npcd._name);
            Debug.Log(_npcd._name);
        }

        NPCBehaviour initNPC = Instantiate(NPCPrefab, _npcd.pos, Quaternion.identity);
        initNPC.SetEntityStats(_npcd);
        initNPC.SetCurrentRoom(this);
    }

}*/

/**
 * Check that every conditions are fulfilled and enemies are dead.
 * @return true when conditions are fulfilled.
 */

//private void OnTriggerExit2D(Collider2D collision)
//{
//    if (collision.CompareTag("Player") || collision.CompareTag("Stealth"))
//    {
//        player.SetCurrentRoom(null);
//    }
//}

//private void OnTriggerExit2D(Collider2D collision)
//{
//    if (collision.CompareTag("Player"))
//    {
//        playerInRoom = false;
//        if (CanProceed())
//        {
//            Debug.Log("DAFuq?");
//            foreach(DoorBehaviour door in doors)
//            {
//                door.LockDoor();
//            }
//            this.enabled = false;
//        }
//    }
//}
#endregion
