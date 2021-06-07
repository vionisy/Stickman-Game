using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxEnergy = 100;
    private float currentEnergy;
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
    private HelthBar energybar;
    private HelthBar Oponenthealthbar;
    public bool Dead = false;
    private bool gravity = false;
    private bool regenerating = true;
    private int size = 0;
    public float growspeed = 0.001f;
    private Camera cam;
    public LineRenderer lr;
    public Transform Hand1;
    public GameObject RightHand;
    public Transform ShootingPoint;
    public SpringJoint2D springjoint;
    public Transform ShootingPoint2;
    public GameObject GravityBall;
    public float firerate = 0.2f;
    private bool readytofire = true;
    public PlayerController(float walljumpForce)
    {
        WalljumpForce = walljumpForce;
    }
    [PunRPC]
    public void OponentHealth(float thehealths)
    {
        if (Oponenthealthbar)
            Oponenthealthbar.SetHealth(thehealths); ;
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
        currentEnergy = maxEnergy;
        readytofire = true;
        springjoint.enabled = false;
        lr.enabled = false;
        FollowMouse[] followMouse = GetComponentsInChildren<FollowMouse>();
        foreach (FollowMouse FollowTheMouse in followMouse)
        {
            FollowTheMouse.enabled = true;
        }
        springjoint = GetComponentInChildren<SpringJoint2D>();
        cam = FindObjectOfType<Camera>();
        healthbar = GameObject.FindGameObjectWithTag("OwnHealthBar").GetComponent<HelthBar>();
        energybar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<HelthBar>();
        GameObject[] oponenthealthbars = GameObject.FindGameObjectsWithTag("OponentsHealthbar");
        energybar.SetMaxHealth(maxEnergy);
        foreach (GameObject thehealth in oponenthealthbars)
        {
            if (!thehealth.GetComponent<PhotonView>().isMine)
            {
                Oponenthealthbar = thehealth.GetComponent<HelthBar>();
                thehealth.SetActive(true);
            }
            else
            {
                thehealth.SetActive(false);
            }
        }
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        if (Oponenthealthbar)
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
        if (Oponenthealthbar)
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
    [PunRPC]
    public void Line2(Vector3 pos)
    {
        lr.SetPosition(0, pos);
    }
    public void Line(Vector3 pos)
    {
        photonView.RPC("Line2", PhotonTargets.All, pos);
    }
    public void loseEnergy(float amount)
    {
        if (currentEnergy >= 0)
            currentEnergy -= amount;
    }
    public void Grapple(Vector3 pos, Rigidbody2D RB)
    {
        if (MenuController.power == 1 &&  photonView.isMine)
        {
            springjoint.connectedBody = RB;
            springjoint.connectedAnchor = pos;
            springjoint.enabled = true;
            if (MenuController.power == 1 && photonView.isMine)
            {
                GameObject.FindGameObjectWithTag("LowerArm").GetComponent<FollowMouse>().enabled = false;
                GameObject.FindGameObjectWithTag("UpperArm").GetComponent<FollowMouse>().enabled = false;
            }
        }
    }
    [PunRPC]
    public void startGrapling()
    {
        Debug.Log("Start");
        lr.enabled = true;
    }
    [PunRPC]
    public void stopGrapling()
    {
        Debug.Log("Stop");
        lr.enabled = false;
    }
    private IEnumerator shoot()
    {

        yield return new WaitForSeconds(0.35f);
        PhotonNetwork.Instantiate(RightHand.name, ShootingPoint.position, ShootingPoint.rotation, 0);
        photonView.RPC("startGrapling", PhotonTargets.All);
        //Instantiate(RightHand, ShootingPoint.position, ShootingPoint.rotation);
    }
    private void shootGravityBall()
    {
        PhotonNetwork.Instantiate(GravityBall.name, ShootingPoint2.position, ShootingPoint.rotation, 0);
    }
    
    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(firerate);
        readytofire = true;
    }
    private bool stop = true;
    private void Update()
    {
        
        energybar.SetHealth(currentEnergy);
        if (lr.enabled == true && GameObject.FindGameObjectWithTag("RightFist"))
            lr.SetPosition(0, GameObject.FindGameObjectWithTag("RightFist").transform.position);
        lr.SetPosition(1, Hand1.transform.position);
        if (MenuController.power == 1 && photonView.isMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine("shoot");
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                photonView.RPC("stopGrapling", PhotonTargets.All);
                springjoint.enabled = false;
                GameObject.FindGameObjectWithTag("LowerArm").GetComponent<FollowMouse>().enabled = true;
                GameObject.FindGameObjectWithTag("UpperArm").GetComponent<FollowMouse>().enabled = true;
            }
            Vector2 transform2D = new Vector2(Hand1.transform.position.x, Hand1.transform.position.y);
            //if (Vector2.Distance(springjoint.connectedAnchor, transform2D) <= 5 && !Input.GetKey(KeyCode.Mouse0))
            //springjoint.enabled = false;
        }
        else
        {
            //lr.enabled = false;
            //springjoint.enabled = false;
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
    private void FixedUpdate()
    {
        if (photonView.isMine && Dead == false)
        {
            KeyInput2();
        }
    }
    void KeyInput2()
    {
        if (currentEnergy <= maxEnergy)
            currentEnergy += 0.07f;
        if (regenerating == true && currentHealth <= maxHealth)
        {
            currentHealth += 0.02f;
            healthbar.SetHealth(currentHealth);
            photonView.RPC("UpdateHealthBar", PhotonTargets.Others, currentHealth);
        }
        if (MenuController.power == 3 && transform.localScale.x <= 1.5f && photonView.isMine && size == 1)
        {
            transform.localScale += new Vector3(growspeed, growspeed, growspeed);
        }
        if (MenuController.power == 3 && transform.localScale.x > 1 && photonView.isMine && size == 0)
        {
            transform.localScale -= new Vector3(growspeed, growspeed, growspeed);
        }
        if (MenuController.power == 3 && transform.localScale.x < 1 && photonView.isMine && size == 0)
        {
            transform.localScale += new Vector3(growspeed, growspeed, growspeed);
        }
        if (MenuController.power == 3 && transform.localScale.x > 0.6f && photonView.isMine && size == -1)
        {
            transform.localScale -= new Vector3(growspeed, growspeed, growspeed);
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
        if (Input.GetKeyDown(KeyCode.Q) && MenuController.power == 3 && size != 0 && size != -1)
        {
            damage[] dammage = GetComponentsInChildren<damage>();
            foreach (damage DAMAGE in dammage)
            {
                DAMAGE.multiplyer = 1;
            }
            size = 0;
            stop = false;
            playerSpeed -= 500;
            jumpForce -= 1000;
            positionRadius -= 0.4f;
            FollowMouse[] followMouse = GetComponentsInChildren<FollowMouse>();
            foreach (FollowMouse FollowTheMouse in followMouse)
            {
                FollowTheMouse.Maxspeed -= 30;
            }
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.mass -= 0.3f;
            }
            stop = true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && MenuController.power == 3&& size == -1)
        {
            damage[] dammage = GetComponentsInChildren<damage>();
            foreach (damage DAMAGE in dammage)
            {
                DAMAGE.multiplyer = 1;
            }
            size = 0;
            stop = false;
            stop = true;
        }
        if (Input.GetKeyDown(KeyCode.E) && MenuController.power == 3 && size != 1 && currentEnergy >= 100)
        {
            damage[] dammage = GetComponentsInChildren<damage>();
            foreach (damage DAMAGE in dammage)
            {
                DAMAGE.multiplyer = 1;
            }
            size = 1;
            stop = false;
            playerSpeed += 500;
            jumpForce += 1000;
            positionRadius += 0.4f;
            FollowMouse[] followMouse = GetComponentsInChildren<FollowMouse>();
            foreach (FollowMouse FollowTheMouse in followMouse)
            {
                FollowTheMouse.Maxspeed += 30;
            }
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.mass += 0.3f;
            }
            stop = true;
            loseEnergy(100);
        }
        if (Input.GetKeyDown(KeyCode.R) && MenuController.power == 3 && size != -1 && currentEnergy >= 100)
        {
            damage[] dammage = GetComponentsInChildren<damage>();
            foreach (damage DAMAGE in dammage)
            {
                DAMAGE.multiplyer = 3;
            }
            size = -1;
            stop = false;
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            stop = true;
            loseEnergy(100);
        }
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
        if (Input.GetKey(KeyCode.E) && MenuController.power == 4 && readytofire == true && currentEnergy >= 50)
        {
            loseEnergy(50);
            shootGravityBall();
            StartCoroutine("FireRate");
            readytofire = false;
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

        if (MenuController.power == 2)
        {
            SpriteRenderer[] Transparency = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer theTransparency in Transparency)
            {
                theTransparency.color = new Color(0f, 0f, 0f, 1f);
            }

            photonView.RPC("Invisibillity", PhotonTargets.Others);
        }

    }
    
    [PunRPC]
    public void Invisibillity()
    {
        SpriteRenderer[] Transparency = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer theTransparency in Transparency)
        {
            theTransparency.color = new Color(1f, 1f, 1f, 0f);
        }
    }


}


