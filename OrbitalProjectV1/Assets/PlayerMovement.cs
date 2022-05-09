using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool canMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            //if movement input is not 0, try to move
            if (movementInput != Vector2.zero)
            {
                bool succ = tryMove(movementInput);

                if (!succ && movementInput.x > 0)
                {
                    succ = tryMove(new Vector2(movementInput.x, 0));

                    if (!succ && movementInput.y > 0)
                    {
                        succ = tryMove(new Vector2(0, movementInput.y));
                    }
                }
                animator.SetBool("isMoving", succ);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            // Set direction of sprite to movement dir;
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
                //swordAttack.attackdirection = SwordAttack.AttackDirection.left;
                //swordAttack.AttackLeft();
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
                //swordAttack.AttackRight();
                //swordAttack.attackdirection = SwordAttack.AttackDirection.right;
            }
        }
    }
    private bool tryMove(Vector2 direction)
    {
        // check for potential collisions;
        int count = rb.Cast(
            movementInput,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }

    }

    public void SwordAttack()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    void OnMove(InputValue movementVal)
    {
        movementInput = movementVal.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
}
