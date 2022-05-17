using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingSpell : MonoBehaviour
{
    private Enemy enemy;
    private Collider2D col;
    private Animator animator;
    private int castCounter;
    private Vector2 leftAttackOffset;
    [Header("Spells")]
    [SerializeField] private List<GameObject> spellPrefabs;
    // Start is called before the first frame update
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        animator = enemy.GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        castCounter = 0;
        leftAttackOffset = transform.localPosition;
    }

    private void Update()
    {
        if (enemy.GetComponent<SpriteRenderer>().flipX)
        {
            transform.localPosition = new Vector3(leftAttackOffset.x * -1, leftAttackOffset.y);
        }
        else
        {
            transform.localPosition = leftAttackOffset;
        }
        castCounter++;
    }  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("Player") && castCounter == 1000)
        {
            Debug.Log(animator);
            animator.SetBool("isWalking", false);
            animator.SetTrigger("CastTrigger");
            Player player = collision.GetComponent<Player>();
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            GameObject.Instantiate(spellPrefabs[0], player.transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
            
            
        } 
    }

    void onTriggerExit2D(Collider2D collider)
    {
        animator.SetBool("isWalking", true);
    }

}
