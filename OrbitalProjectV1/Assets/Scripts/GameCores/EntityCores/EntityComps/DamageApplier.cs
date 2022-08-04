using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EntityDataMgt;

namespace EntityCores.Behavioural
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageApplier : MonoBehaviour
    {
        protected DetectionScript detectionScript;
        protected AudioSource audioSource;
        protected AudioClip meleeAudio;
        protected EntityBehaviour parent;
        protected EnemyData enemyData;
        protected TrapData trapData;
        protected float attackSpeed;
        protected int damage;
        protected bool damaging;
        protected Collider2D col;
        [SerializeField] protected bool inAudio;


        private void Awake()
        {
            parent = transform.parent.GetComponent<EntityBehaviour>();
            inAudio = false;
            audioSource = GetComponent<AudioSource>();
            col = GetComponent<Collider2D>();
        }

        //private void OnEnable()
        //{
        //    CheckingParent();
        //}
        //private void CheckingParent()
        //{
        //    enemyData = (EnemyData) parent.GetData();
        //    trapData = (TrapData)parent.GetData();
        //    SettingUpDamageVals();
        //}

        public void SettingUpDamage(int damageValue)
        {
            this.damage = damageValue;

        }
        public void SettingUpAudio(AudioClip audioClip)
        {
            meleeAudio = audioClip;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //var destruct = collision.GetComponent<DestructableTilemap>();
            //if (destruct != null)
            //{
            //    Debug.Log("Thhis is destructable tilemap" + destruct);
            //    var collisionPoint = collision.ClosestPoint(transform.position);
            //    destruct.DestroyTile(Vector3Int.RoundToInt(collisionPoint));
            //}

            Player target = collision.gameObject.GetComponent<Player>();
            if (target != null && parent.inAnimation && !parent.isDead)
            {

                Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
                if (!target.IsDead())
                {
                    target.GetComponent<Rigidbody2D>().AddForce(direction * attackSpeed, ForceMode2D.Impulse);
                    target.TakeDamage(damage);
                }
                if (meleeAudio != null)
                {
                    if (!inAudio && parent.inAnimation)
                    {
                        StartCoroutine(LoadSingleAudio(meleeAudio));
                    }


                }

            }

        }

        //private void OnTriggerStay2D(Collider2D collision)
        //{
        //    Player target = collision.gameObject.GetComponent<Player>();
        //    if (target != null && parent.inAnimation && !parent.isDead && !damaging) 
        //    {
        //        Vector2 direction = ((Vector2)target.transform.position - (Vector2)transform.position).normalized;
        //        StartCoroutine(DoDamage(target));
        //        //target.GetComponent<Rigidbody2D>().AddForce(direction * enemyData.attackSpeed, ForceMode2D.Impulse);

        //    }

        //}

        private void OnTriggerExit2D(Collider2D collision)
        {

        }

        private IEnumerator DoDamage(Player player)
        {
            damaging = true;
            while (player != null && parent.inAnimation && !parent.isDead)
            {
                player.TakeDamage(damage);
                yield return new WaitForSeconds(1f);
            }
            damaging = false;
        }

        protected IEnumerator LoadSingleAudio(AudioClip audioClip)
        {

            inAudio = true;
            float ogpitch = audioSource.pitch;
            audioSource.pitch = 1f;
            audioSource.clip = audioClip;
            audioSource.Play();
            yield return new WaitForSeconds(1.5f);
            inAudio = false;
            audioSource.pitch = ogpitch;

        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 1);
        }
    }
}
