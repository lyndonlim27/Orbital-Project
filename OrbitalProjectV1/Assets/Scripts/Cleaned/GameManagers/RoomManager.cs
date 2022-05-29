using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * RoomManager.
 * Manage all entities in the room.
 */

public abstract class RoomManager : MonoBehaviour
{

    protected Player player;
    /**
     * Concrete classes
     * Every room starts with their own conditions.
     * Each room will have their list of 
     */
    public List<string> conditions { get; protected set; }
    public List<EntityBehaviour> items { get; protected set; }
    public List<EnemyBehaviour> enemies { get; protected set; }
    public List<NPCBehaviour> npcs { get; protected set; }

    /**
     * Prefabs.
     */
    [Header("Prefabs")]
    [SerializeField] EnemyBehaviour[] enemyPrefabs;
    [SerializeField] EntityBehaviour[] entityPrefabs;
 //   [SerializeField] NPCBehaviour NPCPrefab;
    [SerializeField] protected GameObject[] doors;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected Vector2 roomSize;

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
    

    /*
     * UI
     */
    private PopUpSettings popUpSettings;

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

    }

    protected virtual void Update()
    {
        _collider = Physics2D.OverlapBox(transform.position, roomSize, 0, LayerMask.GetMask("Player"));
        if (_collider != null)
        {

            if (activated == false)
            {
                activated = true;
                dialMgr.SetCurrentRoom(this);
                SpawnObjects(_EntityData);
                //AddConditionalNPCS();
            }
            else
            {

            }
        }
        else
        {
            if (activated)
            {
                if (conditions.Count == 0)
                {
                    //this.enabled = false;
                }
            }
        }
    }


    public abstract void FulfillCondition(string key);

    public abstract void UnfulfillCondition(string key);

    protected virtual void RoomChecker()
    {

        if (conditions.Count == 0 && CheckEnemiesDead() && _collider != null)
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Animator>().SetBool("Open", true);
                door.GetComponent<Collider2D>().enabled = false;
            }
        }
        else
        {
            foreach (GameObject door in doors)
            {
                door.GetComponent<Animator>().SetBool("Open", false);
                door.GetComponent<Collider2D>().enabled = true;
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
        } while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
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
        } while (!Physics2D.OverlapCircle(randomPoint, 1, layerMask));
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
            InstantiateEntity(_item);
            
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
      
        switch (data._type)
        {
            default:
            case EntityData.TYPE.OBJECT:
                entityPrefabs[0].SetEntityStats(data);
                entityPrefabs[0].GetComponent<SpriteRenderer>().sprite = data.sprite;
                Instantiate(entityPrefabs[0], pos, Quaternion.identity).SetCurrentRoom(this);
                break;
            case EntityData.TYPE.ITEM:
                entityPrefabs[1].SetEntityStats(data);
                entityPrefabs[1].GetComponent<SpriteRenderer>().sprite = data.sprite;
                Instantiate(entityPrefabs[1], pos, Quaternion.identity).SetCurrentRoom(this);
                break;
            case EntityData.TYPE.PRESSURE_SWITCH:
                entityPrefabs[2].SetEntityStats(data);
                entityPrefabs[2].GetComponent<SpriteRenderer>().sprite = data.sprite;
                Instantiate(entityPrefabs[2], pos, Quaternion.identity).SetCurrentRoom(this);
                break;
            case EntityData.TYPE.SWITCH:
                entityPrefabs[3].SetEntityStats(data);
                entityPrefabs[3].GetComponent<SpriteRenderer>().sprite = data.sprite;
                Instantiate(entityPrefabs[3], pos, Quaternion.identity).SetCurrentRoom(this);
                break;
            case EntityData.TYPE.NPC:
                entityPrefabs[4].SetEntityStats(data);
                entityPrefabs[4].GetComponent<SpriteRenderer>().sprite = data.sprite;
                Instantiate(entityPrefabs[4], pos, Quaternion.identity).SetCurrentRoom(this);
                break;
            case EntityData.TYPE.ENEMY:
                enemyPrefabs[0].SetEntityStats(data);
                enemyPrefabs[0].GetComponent<SpriteRenderer>().sprite = data.sprite;
                Instantiate(enemyPrefabs[0], data.pos, Quaternion.identity).SetCurrentRoom(this);
                break;
            case EntityData.TYPE.BOSS:
                enemyPrefabs[1].SetEntityStats(data);
                enemyPrefabs[1].GetComponent<SpriteRenderer>().sprite = data.sprite;
                EliteMonsterA emA = (EliteMonsterA) Instantiate(enemyPrefabs[1], data.pos, Quaternion.identity);
                emA.SetCurrentRoom(this);
                enemies.Add(emA);
                break;


        }
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
    protected void CheckRunningEvents()
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

    


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (conditions.Count == 0)
            {
                this.enabled = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, roomSize);


    }

}
