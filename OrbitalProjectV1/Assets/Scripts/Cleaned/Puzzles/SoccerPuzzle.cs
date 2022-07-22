using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPuzzle : MonoBehaviour, Puzzle
{
    bool activated;
    RoomManager currentRoom;
    Bounds boundaries;
    DoorBehaviour[] doors;
    List<Vector2> doorpositions;
    [SerializeField] SwitchData targetData;
    [SerializeField] PressureSwitchBehaviour target;
    ItemWithTextData balldata;
    ItemWithTextBehaviour ball;
    Animator targetAnimator;
    int dir;
    bool stop;
    Transform targetTransform;

    private void Awake()
    {
        currentRoom = GetComponent<RoomManager>();
        boundaries = currentRoom.GetSpawnAreaBound();
        doors = currentRoom.GetDoors();
        doorpositions = new List<Vector2>();
        foreach (DoorBehaviour door in doors)
        {
            if(door != null)
            {
                doorpositions.Add(door.transform.position);
            }
            
        }
        targetData = Resources.Load("Data/PressurePlates/TargetData") as SwitchData;
        balldata = Resources.Load("Data/ItemWithText/BallData") as ItemWithTextData;
        dir = Random.Range(0, 1);
        activated = false;
        stop = false;
    }

    private void Update()
    {
        if (IsComplete())
        {
            stop = true;
            targetAnimator.SetTrigger("Death");
            this.enabled = false;

        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            if (!stop)
            {
                MoveTarget();
            }   
            
        }

    }


    public void ActivatePuzzle(int seqs)
    {
        activated = true;
        SpawnGoal();
        SpawnBall();
        target = GetComponentInChildren<PressureSwitchBehaviour>();
        target.layerMask = LayerMask.GetMask("Ball");
        targetTransform = target.transform;
        target.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        targetAnimator = target.GetComponent<Animator>();
        targetAnimator.runtimeAnimatorController = Resources.Load("Animations/AnimatorControllers/AC_Dummy") as RuntimeAnimatorController;

    }

    private void SpawnBall()
    {
        balldata.random = false;
        balldata.pos = currentRoom.transform.position;
        currentRoom.SpawnObject(balldata);
        ball = GetComponentInChildren<ItemWithTextBehaviour>();
    
    }

    private void SpawnGoal()
    {
        List<Vector2> possibles = PossiblePositions();
        Vector2 selectedpos = possibles[Random.Range(0, possibles.Count)];
        targetData.pos = selectedpos;
        targetData.random = false;
        currentRoom.SpawnObject(targetData);
    }

    private List<Vector2> PossiblePositions()
    {
        List<Vector2> possible = new List<Vector2>();
        for (int x = (int)boundaries.min.x; x < (int) boundaries.max.x; x++)
        {
            var pos1 = new Vector2(x, boundaries.min.y);
            var pos2 = new Vector2(x, boundaries.max.y);
            if (!doorpositions.Contains(pos1))
            {
                possible.Add(pos1);
            }

            if (!doorpositions.Contains(pos2))
            {
                possible.Add(pos2);
            }
        }

        for (int y = (int)boundaries.min.y; y < (int)boundaries.max.y; y++)
        {
            var pos1 = new Vector2(boundaries.min.x, y);
            var pos2 = new Vector2(boundaries.max.x, y);
            if (!doorpositions.Contains(pos1))
            {
                possible.Add(pos1);
            }

            if (!doorpositions.Contains(pos2))
            {
                possible.Add(pos2);
            }
        }
        return possible;
    }

    private void MoveTarget()
    {
        /// go up and down
        if (target.transform.position.x == boundaries.min.x || target.transform.position.x == boundaries.max.x)
        {
            
            MoveUpandDown();
        }
        /// go left and right
        else if (target.transform.position.y == boundaries.min.y || target.transform.position.y == boundaries.max.y)
        {
         
            MoveLeftAndRight();
        }
    }

    private void MoveUpandDown()
    {
        //up
        if (dir == 0)
        {
            if (targetTransform.position.y < boundaries.max.y)
            {
                targetTransform.position += Time.fixedDeltaTime * Vector3.up * 2f;
            }

            if (targetTransform.position.y >= boundaries.max.y)
            {
                dir = 1;
            }
        }
        //down
        else
        {
            if (targetTransform.position.y > boundaries.min.y)
            {
                targetTransform.position += Time.fixedDeltaTime * Vector3.down * 2f;
            }

            if (targetTransform.position.y <= boundaries.min.y)
            {
                dir = 0;
            }

        }
    }


    private void MoveLeftAndRight()
    {
        //up
        if (dir == 0)
        {
            if (targetTransform.position.x < boundaries.max.x)
            {
                targetTransform.position += Time.deltaTime * Vector3.right * 2f;
            }

            if (targetTransform.position.x >= boundaries.max.x)
            {
                dir = 1;
            }
        }
        //down
        else
        {
            if (targetTransform.position.x > boundaries.min.x)
            {
                targetTransform.position += Time.deltaTime * Vector3.left * 2f;
            }

            if (targetTransform.position.x <= boundaries.min.x)
            {
                dir = 0;
            }

        }
    }

    public void Fulfill()
    {
        
    }

    public bool IsActivated()
    {
        return activated;
    }

    public bool IsComplete()
    {
        if (target != null)
        {
            return target.IsOn();
        } else
        {
            return false;
        }
        
    }

    public void Next()
    {
        throw new System.NotImplementedException();
    }

}
