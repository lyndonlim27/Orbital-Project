using System.Collections;
using UnityEngine;
using EntityDataMgt;
using GameManagement;

namespace EntityCores
{
    /**
     * This is entity core behaviour.
     */
    public abstract class EntityBehaviour : MonoBehaviour
    {
        #region Variables
        #region Serialized Variables
        [SerializeField] protected RoomManager currentRoom;
        #endregion

        #region Client-Accessible Variables
        public SpriteRenderer spriteRenderer { get; protected set; }

        public bool isDead { get; protected set; }

        public bool inAnimation;

        public bool Debuffed;
        #endregion

        #region Internal Variables
        protected PoolManager poolManager;

        protected int health;
        #endregion
        #endregion

        /**
         * Monobehaviours
         */
        #region Monobehaviour
        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
            Debuffed = false;
        }

        //A necessary method for object-pooling. 
        protected virtual void OnEnable()
        {
            if (GetData() != null)
            {
                spriteRenderer.sprite = GetData().sprite;
            }

        }
        //To get all singleton components.
        protected virtual void Start()
        {
            poolManager = PoolManager.instance;
        }
        #endregion

        /**
         * All internal methods
         */
        #region Internal Methods
        //Resets all previously faded texture colors for object pooling.
        protected virtual void ResettingColor()
        {
            Color c = spriteRenderer.color;
            c.a = 1;
            spriteRenderer.color = c;

        }
        #endregion

        /**
         * All client-accesible methods.
         */
        #region Client-Access Methods

        //To retrieve data for entity, there are many of types of datas which can be returned, hence this method is abstract.
        public abstract EntityData GetData();

        //Defeated behaviours, varies between entities.
        public abstract void Defeated();

        //To set data for entity by RoomManagers, which are also acting as spawners.
        public abstract void SetEntityStats(EntityData stats);

        //General damage taking behaviour for all entities.
        public virtual void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0 && !isDead)
            {
                isDead = true;
                Defeated();
            }
        }

        //General room-setting method for all entities, for accountability of entities.
        public virtual void SetCurrentRoom(RoomManager roomManager)
        {
            currentRoom = roomManager;
        }

        //Fading out animation.
        public virtual IEnumerator FadeOut()
        {
            for (float f = 1f; f >= -0.05f; f -= 0.05f)
            {
                Color c = spriteRenderer.color;
                c.a = f;
                spriteRenderer.color = c;
                yield return new WaitForSeconds(0.05f);
            }
            poolManager.ReleaseObject(this);
        }



        #endregion


        #region Migrated methods
        ////A general footstep audio for entities
        //protected virtual IEnumerator FootStepAudio()
        //{
        //    if (!GetData().floating && GetData().moveable && !inAudio)
        //    {
        //        inAudio = true;
        //        audioSource.clip = footStep;
        //        audioSource.Play();
        //        yield return new WaitForSeconds(footStep.length);
        //        inAudio = false;
        //    }
        //}

        //protected IEnumerator LoadSingleAudio(AudioClip audioClip)
        //{
        //    audioSource.Stop();
        //    inAudio = true;
        //    float ogpitch = audioSource.pitch;
        //    audioSource.pitch = 1f;
        //    audioSource.clip = audioClip;
        //    audioSource.Play();
        //    yield return new WaitForSeconds(audioSource.clip.length);
        //    inAudio = false;
        //    audioSource.pitch = ogpitch;

        //}
        #endregion
    }
}
