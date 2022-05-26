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
    [SerializeField] EntityBehaviour[] itemPrefab;
    [SerializeField] NPCBehaviour NPCPrefab;

    /**
     * Data.
     */
    public EntityData[] _itemData;
    public NPCData[] _npcData;
    public EnemyData[] _enemyData;

    /**
     * Room.
     */
    private Collider2D roomArea;
    protected bool activated;
    private Vector2 areaminBound;
    private Vector2 areamaxBound;
    private DialogueManager dialMgr;
    private TypingTestTL typingTestTL;

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
        roomArea = GetComponent<Collider2D>();
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

    protected abstract void RoomChecker();

    public abstract void FulfillCondition(string key);

    protected abstract void UnfulfillCondition(string key);

    /** 
     * Get a random position inside the room.
     * @param minBound, maxBound
     * Boundaries of the room. 
     * @return get randompoint within the radius.
     */
    private Vector2 GetRandomPoint(Vector2 minBound, Vector2 maxBound)
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
            Random.Range(minBound.x, maxBound.x),
            Random.Range(minBound.y, maxBound.y));

        } while (!roomArea.OverlapPoint(randomPoint));
        return randomPoint;
    }


    /**
     * SpawnObjects inside the room.
     */
    protected void SpawnObjects()
    {
        if (_itemData == null)
        {
            return;
        }
        for (int i = 0; i < _itemData.Length; i++)
        {
            EntityData _item = _itemData[i];
            Vector2 randomPoint = GetRandomPoint(areaminBound, areamaxBound);
            string placementType = _item.placementType;
            EntityBehaviour initprop;
            switch (placementType)
            {
                //manual placement for all other objects that are not unconditional;
                //anything that is not unconditional -> switch, conditional, etc.
                default:
                case "MANUAL":
                    initprop = Instantiate(itemPrefab[1], _item.pos, Quaternion.identity);
                    if (_item.condition == 1)
                    {
                        conditions.Add(_item._name);
                    }
                    break;
                case "RANDOM":
                    initprop = Instantiate(itemPrefab[0], randomPoint, Quaternion.identity);
                    break;

            }
            initprop.SetEntityStats(_item);



        }
    }

    /**
     * Storing conditional NPCs.
     */
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
        }

    }

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
        if (dialMgr.playing || typingTestTL.isActiveAndEnabled)
        {
            //// for now we just use enemy tags, i think next time if got other stuffs then see how
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
        if (!popUpSettings.gameObject.activeInHierarchy)
        {
            foreach (EnemyBehaviour _enemy in enemies)
            {
                _enemy.enabled = true;
                _enemy.animator.enabled = true;
            }
            player.enabled = true;
        }
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

    }

    /**
     * Initialize rooom when is player is first detected.
     * @param collision 
     * Check whether is player.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (activated == false)
            {
                activated = true;
                SpawnObjects();
                AddConditionalNPCS();

            }
            
        }
        
    }

}
