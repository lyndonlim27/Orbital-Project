using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _2StateTrap: MonoBehaviour
{
    private Collider2D col;
    private Animator animator;
    public ItemStats _item;
    //optional
    private StateMachine stateMachine;
    private Transform obj;
    private ActiveProps activeProps;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        activeProps = this.GetComponentInChildren<ActiveProps>();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 pos;
        switch (_item.dir)
        {
            default:
            case "left":
                pos = new Vector2(col.bounds.min.x, 0);
                break;
            case "right":
                pos = new Vector2(col.bounds.max.x, 0);
                break;
            case "down":
                pos = new Vector2(0,col.bounds.min.y);
                break;
            case "up":
                pos = new Vector2(0,col.bounds.max.y);
               break;
        }
     
        
    }

}
