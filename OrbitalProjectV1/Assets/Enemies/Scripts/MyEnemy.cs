using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemy : MonoBehaviour
{
    public enum ANIMATION_CODE
    {
        ATTACK_END,
        CAST_END,
        ATTACK_TRIGGER
    }

    public Vector2 roamPos;
    public Vector2 roamMinArea, roamMaxArea;
    public GameObject detectionRange;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject customWeapon;
    public EnemySpell spellprefab;
    public float moveSpd = 2.0f;

    StateMachine stateMachine;


    // Start is called before the first frame update
    void Start()
    {
        detectionRange.AddComponent<DetectionScript>();
        customWeapon.AddComponent<WeaponScript>();

        stateMachine = new StateMachine();
        stateMachine.AddState(StateMachine.STATE.MOVE, new MoveState(this.gameObject, this.stateMachine));
        stateMachine.AddState(StateMachine.STATE.ATTACK, new AttackState(this.gameObject, this.stateMachine));
        stateMachine.Init(StateMachine.STATE.MOVE, MoveState.MOVETYPE.POSITION);
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.flipX)
            customWeapon.GetComponent<WeaponScript>().FaceRight();
        else
            customWeapon.GetComponent<WeaponScript>().FaceLeft();

        stateMachine.Update();

    }


    /// <summary>
    /// Called in Animation "Attack"
    /// </summary>
    public void Attack()
    {
        // check if player still in weapon range
        WeaponScript weap = customWeapon.GetComponent<WeaponScript>();
        if (weap.playerDetected == null)
            return;

        Player player = weap.playerDetected.GetComponent<Player>();
        float force = weap.force;
        int damage = weap.damage;
        if (player != null)
        {
            Vector2 direction = ((Vector2)player.transform.position - (Vector2)transform.position).normalized;
            player.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            player.TakeDamage(damage);
            //StartCoroutine(KnockBackCo(player.rb));
        }
    }
    /// <summary>
    /// Called in Animation "Attack"
    /// </summary>
    public void StopAttack()
    {
        //do nothing
        //customWeapon.StopAttack();
    }
    /// <summary>
    /// Called in Animation "Cast"
    /// </summary>


    public void AnimationTrigger(ANIMATION_CODE code)
    {
        switch (code)
        {
            case ANIMATION_CODE.ATTACK_END:
                stateMachine.ChangeState(StateMachine.STATE.MOVE, MoveState.MOVETYPE.POSITION);
                break;
            case ANIMATION_CODE.CAST_END:
                stateMachine.ChangeState(StateMachine.STATE.MOVE, MoveState.MOVETYPE.POSITION);
                break;
            case ANIMATION_CODE.ATTACK_TRIGGER:
                Attack();
                //weapon.attack() better
                break;
        }
    }
}

public class WeaponScript : MonoBehaviour
{
    public int damage = 3;
    public float force = 3.0f;
    public GameObject playerDetected = null;
    public void FaceLeft()
    {
        this.gameObject.transform.localPosition = new Vector3(-0.4f, 0, 0);
    }
    public void FaceRight()
    {
        this.gameObject.transform.localPosition = new Vector3(0.4f, 0, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerDetected = other.gameObject;
            //deal damage to enemy;
            //i need player script.

        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerDetected = null;
        }
    }
};
