using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Pool;

/**
 * RoomManager.
 * Manage all entities in the room.
 */

public abstract class RoomManager : MonoBehaviour
{

    protected Player player;
    protected enum ROOMTYPE
    {
        PUZZLE_ROOM,
        FIGHTING_ROOM,
        HYBRID_ROOM,
        TREASURE_ROOM
    }
    /**
     * Concrete classes
     * Every room starts with their own conditions.
     * Each room will have their list of 
     */
    public HashSet<string> conditions { get; protected set; }
    public List<EntityBehaviour> items { get; protected set; }
    public List<EnemyBehaviour> enemies { get; protected set; }
    public List<PressureSwitchBehaviour> pressureitems;
    public LayerMask CollisionObjects;
    public List<NPCBehaviour> npcs { get; protected set; }
    protected ROOMTYPE roomtype;

    /**
     * Prefabs.
     */
    [Header("Prefabs")]
    //[SerializeField] GameObject[] enemyPrefabs;
    //[SerializeField] EntityBehaviour[] entityPrefabs;
    //   [SerializeField] NPCBehaviour NPCPrefab;
    [SerializeField] protected DoorBehaviour entryDoor;
    [SerializeField] protected DoorBehaviour[] doors;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Vector2 roomSize;
    [SerializeField] private Color _colour;

    protected AstarPath astarPath;
    protected DoorManager doorManager;


    /**
     * Data.
     */
    public EntityData[] _EntityDatas;

    /**
     * Room.
     */
    protected PolygonCollider2D roomArea;
    protected bool activated;
    private Vector2 areaminBound;
    private Vector2 areamaxBound;
    protected DialogueManager dialMgr;
    private TypingTestTL typingTestTL;
    protected Collider2D _collider;
    private HashSet<Vector3> safeRoute;


    //private AstarPath AstarGraph;


    /*
     * UI
     */
    protected PopUpSettings popUpSettings;
    private PoolManager poolManager;
    private Seeker seeker;

    /**
     * Retrieving of Data.
     */
    protected virtual void Awake()
    {
        activated = false;
        roomArea = GetComponent<PolygonCollider2D>();
        this.areaminBound = roomArea.bounds.min;
        this.areamaxBound = roomArea.bounds.max;
        conditions = new HashSet<string>();
        items = new List<EntityBehaviour>();
        enemies = new List<EnemyBehaviour>();
        npcs = new List<NPCBehaviour>();
        player = GameObject.FindObjectOfType<Player>(true);
        dialMgr = GameObject.FindObjectOfType<DialogueManager>(true);
        typingTestTL = GameObject.FindObjectOfType<TypingTestTL>(true);
        popUpSettings = FindObjectOfType<PopUpSettings>(true);
        poolManager = FindObjectOfType<PoolManager>(true);
        //doorManager = FindObjectOfType<DoorManager>(true);
        astarPath = FindObjectOfType<AstarPath>(true);
        

    }

    //protected void OnEnable()
    //{
    //    InitializeAStar();
    //}

    private void Start()
    { 
        //SafePath();
        
    }

    //protected void OnDisable()
    //{
    //    DeActivateAStar();
    //}


    protected virtual void Update()
    {
        if (activated == false)
        {
            _collider = Physics2D.OverlapBox(transform.position, roomSize, 0, LayerMask.GetMask("Player"));
            if (_collider != null)
            {
                
                activated = true;
                dialMgr.SetCurrentRoom(this);
                player.SetCurrentRoom(this);
                
                SpawnObjects(_EntityDatas);
                //InitializeAStar();
                //AddConditionalNPCS();
            }
        }
        
        
       

    }
    private void DeActivateAStar()
    {

        AstarPath astar = gameObject.GetComponent<AstarPath>();
        if (astar != null)
        {
            Destroy(astar);
        }

    }


    private void InitializeAStar()
    {
        if (roomtype != ROOMTYPE.FIGHTING_ROOM && roomtype != ROOMTYPE.HYBRID_ROOM)
        {
            return;
        }
        gameObject.AddComponent<AstarPath>();
        AstarData astarData = AstarPath.active.data;
        GridGraph gg = astarData.AddGraph(typeof(GridGraph)) as GridGraph;
        gg.center = transform.position;
        gg.is2D = true;
        gg.SetDimensions((int)(areamaxBound.x - areaminBound.x), (int)(areamaxBound.y - areaminBound.y), 1f);
        gg.collision.use2D = true;
        gg.collision.mask = CollisionObjects;
        AstarPath.active.Scan(gg);

    }


    public virtual void FulfillCondition(string key)
    {
        if (conditions.Contains(key))
        {
            conditions.Remove(key);
        }
    }

    public virtual void UnfulfillCondition(string key)
    {
        if (!conditions.Contains(key))
        {
            conditions.Add(key);
        }
        
    }

    protected virtual void RoomChecker()
    {

        if (CanProceed())
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].unlocked = true;
                this.enabled = false;
            }
        } else
        {
            LockDoorsOnThisLevel();
        }
        
    }


    /** 
     * Get a random position inside the room.
     * @param minBound, maxBound
     * Boundaries of the room. 
     * @return get randompoint within the radius.
     */
    public Vector2 GetRandomPoint()
    {
        Vector2 randomPoint;
        do
        {
            //AABB axis - 4 possible bounds, top left, top right, bl, br
            //tl = min.x, max.y;
            //bl = min.x, min.y;
            //tr = max.x, max.y;
            //br = max.x, min.y;
            //anywhere within these 4 bounds are possible pts;
            randomPoint = new Vector2(
            Random.Range(areaminBound.x, areamaxBound.x),
            Random.Range(areaminBound.y, areamaxBound.y));

        } while (!roomArea.OverlapPoint(randomPoint) && !Physics2D.OverlapCircle(randomPoint, 1, layerMask));
        //&& safeRoute.Contains(randomPoint));
        return randomPoint;
    }

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
            //AABB axis - 4 possible bounds, top left, top right, bl, br
            //tl = min.x, max.y;
            //bl = min.x, min.y;
            //tr = max.x, max.y;
            //br = max.x, min.y;
            //anywhere within these 4 bounds are possible pts;
            randomPoint = new Vector2(
            Random.Range(minArea.x, maxArea.x),
            Random.Range(minArea.y, maxArea.y));
        } while (!roomArea.OverlapPoint(randomPoint) && !Physics2D.OverlapCircle(randomPoint, 2, layerMask));
        //} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
        return randomPoint;
    }

    /**
     * SpawnObject inside the room.
     */
    public void SpawnObject(EntityData entitydata)
    {
        if (entitydata != null)
        {
            if (entitydata.condition == 1)
            {
                conditions.Add(entitydata._name);
            }
            if (entitydata.spawnAtStart)
            { 
            
                InstantiateEntity(entitydata);
            }

        }
    }


    /**
     * SpawnObjects inside the room.
     */
    public void SpawnObjects(EntityData[] entityDatas)
    {
        if (entityDatas == null)
        {
            Debug.Log(this);
            return;
        }
        for (int i = 0; i < entityDatas.Length; i++)
        {

            EntityData _item = entityDatas[i];
            //EntityBehaviour initprop;
            Debug.Log("This is :" + _item);
            if (_item.condition == 1)
            {
                conditions.Add(_item._name);
            }

            if (!_item.spawnAtStart)
            {
                Debug.Log("??????");
                continue;
            } else
            {
                Debug.Log("WTF? this is item:" + _item);
                if (_item.multispawns)
                {
                    InstantiateMultiPosition(_item);
                } else
                {
                    Debug.Log("this is item: " +_item);
                    InstantiateEntity(_item);
                }
                
            }


            /*
            initprop.SetEntityStats(_item);
            initprop.SetCurrentRoom(this); */
        }
    }

    protected void InstantiateMultiPosition(EntityData item)
    {
        Debug.Log("Entered Multi");
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

        foreach (Vector2 point in points)
        { 
            Debug.Log("This is entity data" + item);
            EntityData copy = Instantiate(item);
            EntityBehaviour entity = poolManager.GetObject(item._type);
            InitializeEntity(copy, point, entity);
        }       
    }

    public Bounds GetRoomAreaBounds()
    {
        return roomArea.bounds;
    }

    private void SafePath()
    {
        safeRoute = new HashSet<Vector3>();
        Debug.Log(entryDoor.transform.position);
        var startPosition = entryDoor.transform.position;
        foreach (DoorBehaviour door in doors)
        {
            Vector2 _doorpos = door.transform.position;
            //if (_doorpos.x > roomArea.bounds.max.x)
            //{
            //    _doorpos.x = roomArea.bounds.max.x;
            //}
            //else if (_doorpos.x < roomArea.bounds.min.x)
            //{
            //    _doorpos.x = roomArea.bounds.min.x;
            //}

            //if (_doorpos.y > roomArea.bounds.max.y)
            //{
            //    _doorpos.y = roomArea.bounds.max.y;
            //}
            //else if (_doorpos.y < roomArea.bounds.min.y)
            //{
            //    _doorpos.y = roomArea.bounds.min.y;
            //
            //Debug.Log(ABPath.Construct(GetRandomPoint(), GetRandomPoint()).vectorPath.Count);
            safeRoute.UnionWith(ABPath.Construct(startPosition, door.transform.position).vectorPath);
        }

    }

    

    public void InstantiateEntity(EntityData data)
    {


        Debug.Log("This is entity data" + data);
        Vector2 pos = data.random ? GetRandomPoint() : data.pos;
        EntityBehaviour entity = poolManager.GetObject(data._type);
        InitializeEntity(data, pos, entity);

    }

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
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
        emf.transform.SetParent(go.transform);
        emf.transform.localScale = new Vector2(data.scale, data.scale);
        emf.transform.localPosition = Vector3.zero;
        //emf.animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", data.animatorname)) as RuntimeAnimatorController;
        go.transform.SetParent(transform);
        enemies.Add(emf);
    }

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
                entity.transform.SetParent(transform);
                npcs.Add((NPCBehaviour) entity);
                break;
            case EntityData.TYPE.ENEMY:
            case EntityData.TYPE.BOSS:
                SettingUpEnemy((EnemyData)data, pos, (EnemyBehaviour)entity);
                break;

        }
        entity.gameObject.SetActive(true);
    }

    private void SettingDefaults(EntityData data, Vector2 pos, EntityBehaviour entity)
    {
        entity.SetEntityStats(data);
        entity.GetComponent<SpriteRenderer>().sprite = data.sprite;
        entity.transform.position = pos;
        entity.transform.localScale = new Vector2(data.scale, data.scale);
        entity.SetCurrentRoom(this);
        
        
    }

    public DoorBehaviour[] GetDoors()
    {
        return this.doors;
    }


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
    protected virtual bool CanProceed()
    {
        if (activated)
        {
            return conditions.Count == 0 && CheckEnemiesDead();
        } else
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

        if (dialMgr.playing || typingTestTL.isActiveAndEnabled || popUpSettings.gameObject.activeInHierarchy)
        {

            PauseGame();
        }
        else
        {
            ResumeGame();

        }
    }



    /**
     * Resume game.
     */
    public void ResumeGame()
    {

        foreach (EnemyBehaviour _enemy in enemies)
        {
            _enemy.enabled = true;
            _enemy.animator.enabled = true;
        }
        player.enabled = true;
    }

    /**
     * Pause game.
     */
    public void PauseGame()
    {
        foreach (EnemyBehaviour _enemy in enemies)
        {
            _enemy.enabled = false;
            _enemy.animator.enabled = false;
        }

        player.enabled = false;
    }

    /**
     * Initialize rooom when is player is first detected.
     * @param collision 
     * Check whether is player.
     */
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (activated == false)
            {
                activated = true;
                dialMgr.SetCurrentRoom(this);
                SpawnObjects();
                AddConditionalNPCS();

            }
            
        }
        
    }*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Still called");
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerStealth"))
        {
            player.SetCurrentRoom(this);
            // maybe we need to use this in the future, dk
            if (!CanProceed())
            {
                Debug.Log("Entered here");
                LockDoorsOnThisLevel();
            }
        }
        
    }

    protected void LockDoorsOnThisLevel()
    {
        foreach (DoorBehaviour door in doors)
        {
            door.unlocked = false;
        }
    }


    public void DisableCollider()
    {
        this.roomArea.enabled = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("PlayerStealth"))
        {
            player.SetCurrentRoom(null);
        }
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = _colour;
        Gizmos.DrawWireCube(transform.position, roomSize);
        

    }

}
