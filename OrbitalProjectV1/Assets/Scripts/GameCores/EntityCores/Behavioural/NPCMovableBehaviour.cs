using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EntityCores
{
    public class NPCMovableBehaviour : NPCBehaviour
    {
        private Player _target;

        private Collider2D _col;
        // Start is called before the first frame update

        protected override void Awake()
        {
            base.Awake();
            _col = GetComponent<Collider2D>();
        }
        protected override void Start()
        {
            _target = GameObject.FindObjectOfType<Player>();
            _col.enabled = true;
        }

        // Update is called once per frame
        void FixedUpdate()
        {

            if (proceedable)
            {
                Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
                point2Target.Normalize();
                point2Target = -point2Target;
                animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
                animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
                animator.SetFloat("Speed", point2Target.magnitude);
                float steps = 3 * Time.deltaTime;
                Vector2 offset = new Vector2(_target.transform.position.x, _target.transform.position.y + 1.5f);
                transform.position = Vector3.MoveTowards(transform.position, offset, steps);
            }

        }


        internal override void Fulfill()
        {
            base.Fulfill();
            _col.enabled = false;
        }
       
    }
}
