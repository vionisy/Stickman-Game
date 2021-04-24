using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator anim;
    public Rigidbody2D rb;
    public float jumpForce;
    public float playerSpeed;
    public Vector2 JumpHeight;
    private bool isOnGround;
    public float positionRadius;
    public LayerMask ground;
    public Transform playerPos;
    public KeyCode Key;
    public float GravitationScale;
    private bool direction;
    [SerializeField] private List<Rigidbody2D> Gravity01;

    void Update()

    {
        if (Input.GetKey(Key))
        {
            foreach (Rigidbody2D gravitation in Gravity01)
            {
                gravitation.gravityScale = GravitationScale;
            }
        }
        else
        {
            foreach (Rigidbody2D gravitation in Gravity01)
            {
                gravitation.gravityScale = 1f;
            }
        }
        if (Time.timeScale != 1)
        {
            jumpForce = 30000;
        }
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (isOnGround == true)
                {
                    anim.Play("Walk");
                    rb.AddForce(Vector2.right * playerSpeed * Time.deltaTime);
                    direction = true;
                }
                else
                {
                    if (direction == true)
                        anim.Play("Idle");
                    if (direction == false)
                        anim.Play("Idle2");
                }

            }
            else
            {
                if (isOnGround == true)
                {
                    anim.Play("WalkBack");
                    rb.AddForce(Vector2.left * playerSpeed * Time.deltaTime);
                    direction = false;
                }
                else
                {
                    if (direction == true)
                        anim.Play("Idle");
                    if (direction == false)
                        anim.Play("Idle2");
                }


            }
        }
        else
        {
            if (direction == true)
                anim.Play("Idle");
            if (direction == false)
                anim.Play("Idle2");
        }

        isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);
            if (isOnGround == true && Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector2.up * jumpForce);
            }

        }


    }


