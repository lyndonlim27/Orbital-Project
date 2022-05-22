using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, ActiveProps
{
    Vector2 target;
    ItemStats _item;
    void ActiveProps.Actiate()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            this.transform.position = (Vector2)transform.position + target.normalized * _item.speed * Time.fixedDeltaTime;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _item.type == "trap")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(_item.damage);
        }
    }


}
