using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool Gravitation = false;
    public Animator anim;
    public Rigidbody2D rb;
    public float jumpForce;
    public float playerSpeed;
    public Vector2 JumpHeight;
    private bool isOnGround;
    public float positionRadius;
    public LayerMask ground;
    public Transform playerPos;
    public float GravitationScale = -1.5f;
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
    private bool Dead = false;
    private bool gravity = false;
    private bool regenerating = true;
    public PlayerController(float walljumpForce)
    {
        WalljumpForce = walljumpForce;
    }
    [PunRPC]
    public void OponentHealth(float thehealths)
    {
        Oponenthealthbar.SetHealth(thehealths);;
    }
    [PunRPC]
    public void Damage2(float TheDamageAmont)
    {
        currentHealth -= TheDamageAmont;
        healthbar.SetHealth(currentHealth);
        StartCoroutine("WaitForRegenerating");
        photonView.RPC("OponentHealth", PhotonTargets.Others, currentHealth);
    }
    public void Damage(float damageamount)
    {
        if (!photonView.isMine)
            photonView.RPC("Damage2", PhotonTargets.Others, damageamount);
    }
    private void Start()
    {
        if (MenuController.power == 3 && photonView.isMine)
        {
            playerSpeed += 500;
            jumpForce += 1000;
            positionRadius = 0.6f;
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.mass += 0.3f;
            }
        }
        healthbar = GameObject.FindGameObjectWithTag("OwnHealthBar").GetComponent<HelthBar>();
        Oponenthealthbar = GameObject.FindGameObjectWithTag("OponentsHealthbar").GetComponent<HelthBar>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        Oponenthealthbar.SetMaxHealth(maxHealth);
        Rigidbody2D[] Gravity01 = GetComponentsInChildren<Rigidbody2D>();
        SaveJumpForce = jumpForce;
        if (!photonView.isMine)
        {
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.isKinematic = true;
                RBCHILDREN.gravityScale = 0;
            }
        }
    }
    [PunRPC]
    public void UpdateHealthBar(float theHealth)
    {
        Oponenthealthbar.SetHealth(theHealth);
    }
    [PunRPC]
    public void dead()
    {
        Destroy(gameObject);
    }
    private IEnumerator WaitForRegenerating()
    {
        regenerating = false;
        float HealthBeforeWaiting = currentHealth;
        yield return new WaitForSeconds(5);
        if (currentHealth == HealthBeforeWaiting)
        {
            regenerating = true;
        }
    }
    private void Update()
    {
        if (MenuController.power == 3 && transform.localScale.x <= 1.6f && photonView.isMine)
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        }
        
        if (regenerating == true && currentHealth <= maxHealth)
        {
            currentHealth += 0.02f;
            healthbar.SetHealth(currentHealth);
            photonView.RPC("UpdateHealthBar", PhotonTargets.Others, currentHealth);

        }
        if (currentHealth <= 0 && Dead == false)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().StartCoroutine("Respawn");
            Dead = true;
            StartCoroutine("deadbody");
        }
        if (photonView.isMine && Dead == false)
        {
            KeyInput();
        }
    }
    private IEnumerator deadbody()
    {
        Balance[] balances = GetComponentsInChildren<Balance>();
        foreach (Balance theBalances in balances)
        {
            theBalances.enabled = false;
        }
        BalanceArms[] balancesarms = GetComponentsInChildren<BalanceArms>();
        foreach (BalanceArms theBalanceArms in balancesarms)
        {
            theBalanceArms.enabled = false;
        }
        FollowMouse[] followMouse = GetComponentsInChildren<FollowMouse>();
        foreach (FollowMouse FollowTheMouse in followMouse)
        {
            FollowTheMouse.enabled = false;
        }

        yield return new WaitForSeconds(4);
        photonView.RPC("dead", PhotonTargets.All);

        
    }
    [PunRPC]
    public void GravitationChange(bool theGravitation)
    {
        Gravitation = theGravitation;
    }
    void KeyInput()

    {
        Rigidbody2D[] Gravity01 = GetComponentsInChildren<Rigidbody2D>();
        Balance[] Balances = GetComponentsInChildren<Balance>();
        if (Input.GetKey(KeyCode.UpArrow) && MenuController.power == 4)
        {
            PlayerController.Gravitation = true;
            photonView.RPC("GravitationChange", PhotonTargets.Others, Gravitation);
        }
        if (Input.GetKey(KeyCode.DownArrow) && MenuController.power == 4)
        {
            PlayerController.Gravitation = false;
            photonView.RPC("GravitationChange", PhotonTargets.Others, Gravitation);
        }
        if (PlayerController.Gravitation == true)
        {
            foreach (Rigidbody2D gravitation in Gravity01)
            {
                gravity = true;
                gravitation.gravityScale = GravitationScale;
            }
        }
        else
        {
            foreach (Rigidbody2D gravitation in Gravity01)
            {
                gravity = false;
                gravitation.gravityScale = 1.5f;
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
                    direction = true;
                    anim.Play("Walk");
                    if (gravity == false)
                    {
                        rb.AddForce(Vector2.right * playerSpeed * Time.deltaTime);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * playerSpeed * Time.deltaTime);
                    }
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
                    direction = false;
                    if (gravity == true)
                    {
                        rb.AddForce(Vector2.right * playerSpeed * Time.deltaTime);
                    }
                    else
                    {
                        rb.AddForce(Vector2.left * playerSpeed * Time.deltaTime);
                    }
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
            if (gravity == false)
                rb.AddForce(Vector2.up * jumpForce);
            else
                rb.AddForce(Vector2.down * jumpForce);
        }

        isOnWallLeft = Physics2D.OverlapCircle(playerPos2.position, positionRadius, ground);
        isOnWallRight = Physics2D.OverlapCircle(playerPos1.position, positionRadius, ground);
        isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);

        if (isOnGround == false && isOnWallRight == true && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.left * WalljumpForce);
            if (gravity == false)
                rb.AddForce(Vector2.up * WalljumpForce);
            else
                rb.AddForce(Vector2.down * WalljumpForce);
            direction = false;
        }
        if (isOnGround == false && isOnWallLeft == true && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.right * WalljumpForce);
            if (gravity == false)
                rb.AddForce(Vector2.up * WalljumpForce);
            else
                rb.AddForce(Vector2.down * WalljumpForce);
            direction = true;
        }


    }


}


