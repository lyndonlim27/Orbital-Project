using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovableBehaviour : NPCBehaviour
{
    private Animator _animator;
    private Player _target;
    private DialogueManager dialogueManager;
    private RoomManager roomManager;
    private DetectionScript _detectionScript;
    private Collider2D _col;
    // Start is called before the first frame update

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _detectionScript = GetComponentInChildren<DetectionScript>();
        _col = GetComponent<Collider2D>();
        dialogueManager = GameObject.FindObjectOfType<DialogueManager>(true);
    }
    void Start()
    {
        _target = GameObject.FindObjectOfType<Player>();
        roomManager = transform.root.GetComponent<RoomManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fulfilled)
        {
            _col.enabled = true;
        }
        if (!fulfilled & _detectionScript.playerDetected) {
            Vector2 point2Target = (Vector2)transform.position - (Vector2)_target.transform.position;
            point2Target.Normalize();
            point2Target = -point2Target;
            _animator.SetFloat("Horizontal", Mathf.RoundToInt(point2Target.x));
            _animator.SetFloat("Vertical", Mathf.RoundToInt(point2Target.y));
            _animator.SetFloat("Speed", point2Target.magnitude);
            float steps = 3 * Time.deltaTime;
            Vector2 offset = new Vector2(_target.transform.position.x, _target.transform.position.y + 1);
            transform.position = Vector3.MoveTowards(transform.position, offset, steps);
        }

    }
}