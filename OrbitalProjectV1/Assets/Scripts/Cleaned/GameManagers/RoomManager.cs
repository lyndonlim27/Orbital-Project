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
    public List<string> conditions { get; protected set; }
    public List<EntityBehaviour> items { get; protected set; }
    public List<EnemyBehaviour> enemies { get; protected set; }
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
    [SerializeField] protected DoorBehaviour[] doors;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Vector2 roomSize;
    [SerializeField] private Color _colour;
    protected DoorManager doorManager;


    /**
     * Data.
     */
    public EntityData[] _EntityData;
   // public NPCData[] _npcData;
    public EnemyData[] _enemyData;

    /**
     * Room.
     */
    private PolygonCollider2D roomArea;
    protected bool activated;
    private Vector2 areaminBound;
    private Vector2 areamaxBound;
    protected DialogueManager dialMgr;
    private TypingTestTL typingTestTL;
    private Collider2D _collider;
    
    //private AstarPath AstarGraph;


    /*
     * UI
     */
    protected PopUpSettings popUpSettings;
    private PoolManager poolManager;

    /**
     * Retrieving of Data.
     */
    protected virtual void Awake()
    {
        activated = false;
        roomArea = GetComponent<PolygonCollider2D>();
        this.areaminBound = roomArea.bounds.min;
        this.areamaxBound = roomArea.bounds.max;
        conditions = new List<string>();
        items = new List<EntityBehaviour>();
        enemies = new List<EnemyBehaviour>();
        npcs = new List<NPCBehaviour>();
        player = GameObject.FindObjectOfType<Player>(true);
        dialMgr = GameObject.FindObjectOfType<DialogueManager>(true);
        typingTestTL = GameObject.FindObjectOfType<TypingTestTL>(true);
        popUpSettings = FindObjectOfType<PopUpSettings>(true);
        poolManager = FindObjectOfType<PoolManager>(true);
        doorManager = FindObjectOfType<DoorManager>(true);

    }

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
                SpawnObjects(_EntityData);
                InitializeAStar();
                //AddConditionalNPCS();
            }
        }

        Debug.Log(player.GetCurrentRoom());
    }
    private void DeActivateAStar()
    {

        AstarPath astar = gameObject.GetComponent<AstarPath>();
        if (astar != null)
        {
            astar.enabled = false;
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
        gg.SetDimensions((int)(areamaxBound.x - areaminBound.x), (int) (areamaxBound.y - areaminBound.y), 1f);
        gg.collision.use2D = true;
        gg.collision.mask = CollisionObjects;
        AstarPath.active.Scan(gg);
        
    }


    public virtual void FulfillCondition(string key)
    {
        conditions.Remove(key);
    }

    public virtual void UnfulfillCondition(string key)
    {
        conditions.Add(key);
    }

    protected virtual void RoomChecker()
    {
        if (CanProceed())
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doorManager.clearDoor(this,i);
            }
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
        } while (!roomArea.OverlapPoint(randomPoint));
        //while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
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
        } while (!roomArea.OverlapPoint(randomPoint));
        //} while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
        return randomPoint;
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
            //EntityBehaviour initprop;
            if (_item.condition == 1)
            {
                conditions.Add(_item._name);
            }

            if (!_item.spawnAtStart)
            {
                continue;
            } else
            {
                InstantiateEntity(_item);
            }
            
            
            /*
            initprop.SetEntityStats(_item);
            initprop.SetCurrentRoom(this); */
        }
    }

    /**
     * Check what type the entity it is and instantiate
     */

    public void InstantiateEntity(EntityData data)
    {


        Vector2 pos = data.random ? GetRandomPoint() : data.pos;
        EntityBehaviour entity = poolManager.GetObject(data._type);
        InitializeEntity(data, pos, entity);
        
    }

    private void SettingUpEnemy(EnemyData data, Vector2 pos, EnemyBehaviour emf)
    {
        GameObject go = new GameObject(data._name);
        go.layer = LayerMask.NameToLayer("enemy");
        go.transform.position = pos;
        emf.transform.SetParent(go.transform);
        emf.transform.localScale = new Vector2(data.scale, data.scale);
        emf.transform.localPosition = Vector3.zero;
        emf.animator.runtimeAnimatorController = Resources.Load(string.Format("Animations/AnimatorControllers/{0}", data.animatorname)) as RuntimeAnimatorController;
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
                break;
            case EntityData.TYPE.NPC:
                npcs.Add((NPCBehaviour)entity);
                entity.transform.SetParent(transform);
                break;
            case EntityData.TYPE.ENEMY:
            case EntityData.TYPE.BOSS:
                SettingUpEnemy((EnemyData) data, pos, (EnemyBehaviour)entity);
                break;

        }
    }

    private void SettingDefaults(EntityData data, Vector2 pos, EntityBehaviour entity)
    {
        entity.SetEntityStats(data);
        entity.GetComponent<SpriteRenderer>().sprite = data.sprite;
        entity.transform.position = pos;
        entity.gameObject.SetActive(true);
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
    protected bool CanProceed()
    {

        return conditions.Count == 0 && CheckEnemiesDead();
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
        Debug.Log(collision.tag);
        if (collision.CompareTag("Player"))
        {
            doorManager.LockDoorsOnAllLevels();
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
