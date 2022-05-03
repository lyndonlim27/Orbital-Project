using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    //allows for changes from unity directly
    public float Health {
        set {
            health = value;
            if (health <= 0) {
                Defeated();
            }
        }
        get {
            return health;
        }
    }
    public float health = 1; //default

    private void Start() {
        animator = GetComponent<Animator>();
        
    }

    public void Defeated(){
        animator.SetBool("isAlive",false);
    }

    public void RemoveEnemy() {
        Destroy(gameObject);
    }

}
