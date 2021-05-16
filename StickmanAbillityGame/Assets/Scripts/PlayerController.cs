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
    public PhotonView photonView;
    private float SaveJumpForce;
    public Transform playerPos1;
    public Transform playerPos2;
    private bool isOnWallLeft;
    private bool isOnWallRight;
    public float WalljumpForce;
    public float maxHealth = 100;
    private float currentHealth;
    private HelthBar healthbar;
    private HelthBar Oponenthealthbar;
    public PlayerController(float walljumpForce)
    {
        WalljumpForce = walljumpForce;
    }
    [PunRPC]
    public void OponentHealth(float health)
    {
        Oponenthealthbar.SetHealth(health);
    }
    public void Damage(float damageAmount)
    {
        currentHealth -= damageAmount;
        healthbar.SetHealth(currentHealth);
        photonView.RPC("OponentHealth", PhotonTargets.Others, currentHealth);
        
    }
    private void Start()
    {
        healthbar = GameObject.FindGameObjectWithTag("OwnHealthBar").GetComponent<HelthBar>();
        Oponenthealthbar = GameObject.FindGameObjectWithTag("OponentsHealthbar").GetComponent<HelthBar>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        Rigidbody2D[] Gravity01 = GetComponentsInChildren<Rigidbody2D>();
        SaveJumpForce = jumpForce;
        if (!photonView.isMine)
        {
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.isKinematic = true;
                //RBCHILDREN.gravityScale = 0;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(5);
        }
        if (photonView.isMine)
        {
            KeyInput();
        }
    }

    void KeyInput()

    {
        Rigidbody2D[] Gravity01 = GetComponentsInChildren<Rigidbody2D>();
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
            jumpForce = 15000;
        }
        else
        {
            jumpForce = SaveJumpForce;
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

        if (isOnGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        isOnWallLeft = Physics2D.OverlapCircle(playerPos2.position, positionRadius, ground);
        isOnWallRight = Physics2D.OverlapCircle(playerPos1.position, positionRadius, ground);
        isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);

        if (isOnGround == false && isOnWallRight == true && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.left * WalljumpForce);
            rb.AddForce(Vector2.up * WalljumpForce);
            direction = false;
        }
        if (isOnGround == false && isOnWallLeft == true && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.right * WalljumpForce);
            rb.AddForce(Vector2.up * WalljumpForce);
            direction = true;
        }


    }


}


