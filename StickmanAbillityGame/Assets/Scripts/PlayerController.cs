using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region variables
    #region private
    private bool FlightOn = false;
    private float num;
    private StressReceiver camerashake;
    private bool stomp = false;
    private bool DoubleJump;
    private bool DBCoolDown = false;
    private bool fireOn = false;
    private float SaveJumpForce;
    private bool isFrozen = false;
    private bool Onlyonce = true;
    private bool Onlyonce2 = true;
    private bool JohnCena = false;
    private FixedJoystick joystick;
    private FixedJoystick joystick2;
    private bool direction;
    private float currentEnergy;
    private bool isOnGround;
    private float currentHealth;
    private HelthBar healthbar;
    private HelthBar energybar;
    private HelthBar Oponenthealthbar;
    private HelthBar JumpboostBar;
    private HelthBar SpeedBar;
    private HelthBar HealthboostBar;
    private HelthBar StrenghtBar;
    private bool isOnWallLeft;
    private bool isInWater;
    private bool isOnWallRight;
    private bool gravity = false;
    private bool regenerating = true;
    private int size = 0;
    private Camera cam;
    private bool readytofire = true;
    private bool sizeBackToNormal = false;
    private bool leftarmsize = false;
    private int jumpBoost = 0;
    private int speedBoost = 0;
    private int strengthBoost = 0;
    private int healthBoost = 0;
    private bool HeadOnFire = false;
    private bool stop = true;
    private float dashCount;
    private int dashDirection;
    #endregion
    #region public
    public float dashSpeed;
    public float startDashCount;


    public ParticleSystem Flightparticles1;
    public ParticleSystem Flightparticles2;
    public float SwimRotation;
    public bool waitforRotation = false;
    public ParticleSystem Bubles;
    public GameObject IceFoot1;
    public GameObject IceFoot2;
    public GameObject FireDamage;
    public ParticleSystem FireParticles;
    public ParticleSystem OnFireParticles;
    public Rigidbody2D Headrb;
    public Rigidbody2D LeftLowLeg;
    public Rigidbody2D RightLowLeg;
    public GameObject leftarm;
    public float maxEnergy = 100;
    public static bool Gravitation = false;
    public Animator anim;
    public float swimmanim;
    public Rigidbody2D rb;
    public float jumpForce;
    public float playerSpeed;
    public Vector2 JumpHeight;
    public float positionRadius;
    public LayerMask ground;
    public LayerMask water;
    public Transform playerPos;
    public float GravitationScale = -1.5f;
    public PhotonView photonView;
    public Transform playerPos1;
    public Transform playerPos2;
    public float WalljumpForce;
    public float maxHealth = 100;
    public bool Dead = false;
    public float growspeed = 0.001f;
    public LineRenderer lr;
    public Transform Hand1;
    public GameObject RightHand;
    public Transform ShootingPoint;
    public SpringJoint2D springjoint;
    public Transform ShootingPoint2;
    public GameObject GravityBall;
    public GameObject IceBall;
    public float firerate = 0.2f;
    public LayerMask LayerToFreeze;
    public float IcefieldofImpact;
    public PhysicsMaterial2D HighFriction;
    public PhysicsMaterial2D IceFriction;
    public ParticleSystem psIce;
    public ParticleSystem psFire;
    #endregion
    #endregion

    #region functions
    #region IEnumerators
    private IEnumerator CameraShakeStomp()
    {
        camerashake.InduceStress(1.1f);
        yield return new WaitForSeconds(0.14f);
        camerashake.InduceStress(0);
    }
    IEnumerator ChangeSwimRotation(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            SwimRotation = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        SwimRotation = v_end;
        waitforRotation = false;
    }
    private IEnumerator changetheGravity()
    {
        if (MenuController.power == 4)
        {
            PlayerController.Gravitation = true;
            yield return new WaitForSeconds(15);
            PlayerController.Gravitation = false;
        }
    }
    private IEnumerator DoubleJumpCooldown()
    {
        yield return new WaitForSeconds(5);
        DBCoolDown = false;
    }
    private IEnumerator visible()
    {
        photonView.RPC("sidebar", PhotonTargets.Others);
        yield return new WaitForSeconds(1.5f);
        photonView.RPC("Invisibillity", PhotonTargets.Others);
    }
    private IEnumerator FrozenFeet()
    {
        photonView.RPC("IceFeetStart", PhotonTargets.AllBuffered);
        RightLowLeg.sharedMaterial = IceFriction;
        LeftLowLeg.sharedMaterial = IceFriction;
        yield return new WaitForSeconds(20);
        RightLowLeg.sharedMaterial = HighFriction;
        LeftLowLeg.sharedMaterial = HighFriction;
        photonView.RPC("IceFeetStop", PhotonTargets.AllBuffered);

    }
    private IEnumerator FireHead()
    {
        HeadOnFire = true;
        photonView.RPC("FireOn", PhotonTargets.All);
        yield return new WaitForSeconds(9);
        photonView.RPC("FireOff", PhotonTargets.All);
        HeadOnFire = false;
    }
    private IEnumerator YourFrozen()
    {
        isFrozen = true;
        yield return new WaitForSeconds(5);
        isFrozen = false;
    }
    private IEnumerator WaitForRegenerating()
    {
        regenerating = false;
        float HealthBeforeWaiting = currentHealth;
        yield return new WaitForSeconds(10);
        if (currentHealth == HealthBeforeWaiting)
        {
            regenerating = true;
        }
    }
    private IEnumerator leftarmgrow()
    {
        leftarmsize = true;
        yield return new WaitForSeconds(15);
        leftarmsize = false;
    }
    private IEnumerator shoot()
    {
        yield return new WaitForSeconds(0.5f);
        PhotonNetwork.Instantiate(RightHand.name, ShootingPoint.position, ShootingPoint.rotation, 0);
        photonView.RPC("startGrapling", PhotonTargets.All);
        //Instantiate(RightHand, ShootingPoint.position, ShootingPoint.rotation);
    }
    #endregion
    #region normal voids
    
    public void jumpBoostLevelUp()
    {
        jumpBoost += 1;
        jumpForce += 50;
        SaveJumpForce += 500;
        JumpboostBar.SetHealth(jumpBoost);
    }

    public void speedBoostLevelUp()
    {
        speedBoost += 1;
        playerSpeed += 250;
        SpeedBar.SetHealth(speedBoost);
    }
    public void strengthBoostLevelUp()
    {
        strengthBoost += 1;
        damage[] strength = GetComponentsInChildren<damage>();
        foreach (damage strength2 in strength)
        {
            strength2.multiplyer += 0.1f;
        }
        StrenghtBar.SetHealth(strengthBoost);
    }
    public void healthBoostLevelUp()
    {
        healthBoost += 1;
        maxHealth += 25f;
        HealthboostBar.SetHealth(healthBoost);
    }
    private IEnumerator deadbody()
    {
        if (MenuController.power == 2 && photonView.isMine)
            photonView.RPC("sidebar", PhotonTargets.Others);
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
        if (MenuController.selectedgamemode == 2)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<BattleRoyaleManager>().dead_screen();
        }
        photonView.RPC("dead", PhotonTargets.All);
    }
    private IEnumerator sizeBack()
    {
        yield return new WaitForSeconds(20);
        sizeBackToNormal = true;
    }
    public void Damage(float damageamount)
    {
        if (!photonView.isMine)
            photonView.RPC("Damage2", PhotonTargets.Others, damageamount);
    }
    public void delete()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    private void FireAttack()
    {
        photonView.RPC("psFire2", PhotonTargets.All);
        Collider2D[] objects = Physics2D.OverlapCircleAll(rb.transform.position, IcefieldofImpact, LayerToFreeze);
        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - rb.transform.position;
            if (obj.GetComponentInParent<PlayerController>())
            {
                obj.GetComponentInParent<PlayerController>().Fireattack();
            }
            if (obj.GetComponentInParent<TheBraaiiinnn>())
            {
                obj.GetComponentInParent<TheBraaiiinnn>().Fireattack();
            }
        }
    }
    private void iceField()
    {
        photonView.RPC("psIce2", PhotonTargets.All);
        Collider2D[] objects = Physics2D.OverlapCircleAll(rb.transform.position, IcefieldofImpact, LayerToFreeze);
        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - rb.transform.position;
            if (obj.GetComponentInParent<PlayerController>())
            {
                obj.GetComponentInParent<PlayerController>().FreezeFeet1();
                obj.GetComponentInParent<PlayerController>().Damage(3);
            }
            if (obj.GetComponentInParent<TheBraaiiinnn>())
            {
                obj.GetComponentInParent<TheBraaiiinnn>().FreezeFeet1();
                obj.GetComponentInParent<TheBraaiiinnn>().Damage(3);
            }
        }
    }
    public void FreezeFeet1()
    {
        if (!photonView.isMine)
            photonView.RPC("FreezeFeet2", PhotonTargets.Others);
    }
    public void Fireattack3()
    {
        if (!photonView.isMine)
            photonView.RPC("ThisGuyIsOnFire", PhotonTargets.Others);
        else
            ThisGuyIsOnFire();
    }
    public void Fireattack()
    {
        if (!photonView.isMine)
            photonView.RPC("ThisGuyIsOnFire", PhotonTargets.Others);
    }
    public PlayerController(float walljumpForce)
    {
        WalljumpForce = walljumpForce;
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
        if (MenuController.power == 1 && photonView.isMine)
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
    private void shootGravityBall()
    {
        //PhotonNetwork.Instantiate(GravityBall.name, ShootingPoint2.position, ShootingPoint.rotation, 0);
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(firerate);
        readytofire = true;
    }

    public void Freeze1()
    {
        if (!photonView.isMine)
        {
            photonView.RPC("Freeze2", PhotonTargets.Others);
        }
    }
    private IEnumerator stamping()
    {

        direction = true;
        anim.Play("Stomp");
        stomp = true;
        yield return new WaitForSeconds(1);
        stomp = false;
        photonView.RPC("UnCameraShake", PhotonTargets.All);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject theplayers in players)
        {
            if (!theplayers.GetComponent<PlayerController>().photonView.isMine)
                theplayers.GetComponent<PlayerController>().Damage(80);
        }
        GameObject[] Ki = GameObject.FindGameObjectsWithTag("Ai");
        foreach (GameObject theplayers in Ki)
        {
            Debug.Log("stomp");
            if (theplayers.GetComponent<TheBraaiiinnn>())
                theplayers.GetComponent<TheBraaiiinnn>().Damage(80);
        }
    }
    #endregion
    #region PunRPC voids
    [PunRPC]
    public void Jumping()
    {
        if (GameManager.playernumber == 1)
        {
            if (gravity == false)
                rb.AddForce(Vector2.up * jumpForce);
            else
                rb.AddForce(Vector2.down * jumpForce);
        }
    }
    [PunRPC]
    public void firestart()
    {
        FireParticles.Play();
    }
    [PunRPC]
    public void firestop()
    {
        FireParticles.Stop();
    }
    [PunRPC]
    public void Invisibillity()
    {
        if (!photonView.isMine)
        {
            SpriteRenderer[] Transparency = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer theTransparency in Transparency)
            {
                theTransparency.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }
    [PunRPC]
    public void sidebar()
    {
        if (!photonView.isMine)
        {
            SpriteRenderer[] Transparency = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer theTransparency in Transparency)
            {
                theTransparency.color = new Color(0.2f, 0.2f, 0.2f, 1f);
            }
        }
    }
    [PunRPC]
    public void GravitationChange(bool theGravitation)
    {
        Gravitation = theGravitation;
    }
    [PunRPC]
    public void hidehealthbar()
    {
        GetComponentInChildren<HelthBar>().gameObject.SetActive(false);
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
        if (MenuController.power == 2 && photonView.isMine)
            StartCoroutine("visible");
        currentHealth -= TheDamageAmont;
        healthbar.SetHealth(currentHealth);
        StartCoroutine("WaitForRegenerating");
        photonView.RPC("OponentHealth", PhotonTargets.Others, currentHealth);
    }
    [PunRPC]
    public void FreezeFeet2()
    {

        StartCoroutine("FrozenFeet");
    }
    [PunRPC]
    public void IceFeetStart()
    {
        IceFoot1.SetActive(true);
        IceFoot2.SetActive(true);
    }
    [PunRPC]
    public void IceFeetStop()
    {
        IceFoot1.SetActive(false);
        IceFoot2.SetActive(false);
    }
    [PunRPC]
    public void FireOn()
    {
        OnFireParticles.Play();
    }
    [PunRPC]
    public void FireOff()
    {
        OnFireParticles.Stop();
    }
    [PunRPC]
    public void ThisGuyIsOnFire()
    {
        StartCoroutine("FireHead");
    }
    [PunRPC]
    public void psIce2()
    {
        psIce.Play();
    }
    [PunRPC]
    public void psFire2()
    {
        psFire.Play();
    }
    [PunRPC]
    public void Frooozen1()
    {
        SpriteRenderer[] Transparency2 = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer theTransparency in Transparency2)
        {
            theTransparency.color = new Color(0.7f, 1f, 1f, 0.9f);
        }
    }
    [PunRPC]
    public void Frooozen2()
    {
        SpriteRenderer[] Transparency = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer theTransparency in Transparency)
        {
            theTransparency.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    [PunRPC]
    public void UnCameraShake()
    {
        StartCoroutine("CameraShakeStomp");
    }
    [PunRPC]
    public void Freeze2()
    {
        StartCoroutine("YourFrozen");
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
    [PunRPC]
    public void Line2(Vector3 pos)
    {
        lr.SetPosition(0, pos);
    }
    [PunRPC]
    public void startGrapling()
    {
        lr.enabled = true;
    }
    [PunRPC]
    public void stopGrapling()
    {
        lr.enabled = false;
    }
    #endregion
    #endregion

    #region startandUpdae
    private void Start()
    {
        dashCount = startDashCount;

        camerashake = FindObjectOfType<Camera>().GetComponent<StressReceiver>();
        if (MenuController.power == 3 && photonView.isMine)
        {

            maxHealth += 50;
            SaveJumpForce += 1000;
            jumpForce += 1000;
            FollowMouse[] thefollowMouse = GetComponentsInChildren<FollowMouse>();
            foreach (FollowMouse FollowTheMouse in thefollowMouse)
            {
                FollowTheMouse.Maxspeed += 10;
            }
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.mass += 0.1f;
            }
        }
        OnFireParticles.Stop();
        FireParticles.Stop();
        psIce.Stop();
        if (MenuController.power == 2 && photonView.isMine)
        {
            SpriteRenderer[] Transparency = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer theTransparency in Transparency)
            {
                theTransparency.color = new Color(0.15f, 0.15f, 0.15f, 1f);
            }
            photonView.RPC("Invisibillity", PhotonTargets.OthersBuffered);
            photonView.RPC("hidehealthbar", PhotonTargets.OthersBuffered);
        }
        if (MenuController.power == 3 && photonView.isMine)
            maxEnergy -= 50;
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
        if (MenuController.selectedgamemode == 2)
        {
            SpeedBar = GameObject.FindGameObjectWithTag("SpeedBar").GetComponent<HelthBar>();
            StrenghtBar = GameObject.FindGameObjectWithTag("StrenghtBar").GetComponent<HelthBar>();
            HealthboostBar = GameObject.FindGameObjectWithTag("HealthboostBar").GetComponent<HelthBar>();
            JumpboostBar = GameObject.FindGameObjectWithTag("JumpboostBar").GetComponent<HelthBar>();
            HealthboostBar.SetMaxHealth(20);
            StrenghtBar.SetMaxHealth(20);
            JumpboostBar.SetMaxHealth(20);
            SpeedBar.SetMaxHealth(20);
            HealthboostBar.SetHealth(0);
            StrenghtBar.SetHealth(0);
            JumpboostBar.SetHealth(0);
            SpeedBar.SetHealth(0);
        }
        healthbar = GameObject.FindGameObjectWithTag("OwnHealthBar").GetComponent<HelthBar>();
        energybar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<HelthBar>();
        GameObject[] oponenthealthbars = GameObject.FindGameObjectsWithTag("OponentsHealthbar");
        if (photonView.isMine)
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
        if (GameManager.playernumber != 1)
        {
            Rigidbody2D[] rbChildren = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D RBCHILDREN in rbChildren)
            {
                RBCHILDREN.isKinematic = true;
                RBCHILDREN.gravityScale = 0;
            }
        }
    }
    private void Update()
    {
        if (photonView.isMine)
        {
            if (isOnGround == true && Input.GetKeyDown(KeyCode.Space) || (joystick != null && joystick.Vertical >= 0.3 && isOnGround == true) && Onlyonce == true)
            {
                Onlyonce = false;
                photonView.RPC("Jumping", PhotonTargets.All);
            }
            else if (joystick != null && joystick.Vertical <= 0.3)
            {
                Onlyonce = true;
            }
        }
        if (isFrozen == false && Dead == false)
        {
            FixedJoint2D[] freeze = GetComponentsInChildren<FixedJoint2D>();
            foreach (FixedJoint2D frozen in freeze)
            {
                if (frozen.gameObject.name != "Chest" && frozen.gameObject.name != "Neck")
                {
                    frozen.enabled = false;
                }
            }
            Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rbs in rigidbodies)
            {
                rbs.freezeRotation = false;
            }
        }
        else if (isFrozen == true && Dead == false)
        {
            Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D rbs in rigidbodies)
            {
                rbs.freezeRotation = true;
            }
        }
        FixedJoystick[] fixedJoysticks = FindObjectsOfType<FixedJoystick>();
        if (GameManager.HandyControllsOn == true)
            foreach (FixedJoystick joysticks in fixedJoysticks)
            {
                if (joysticks.tag == "Joystick")
                {
                    joystick = joysticks;
                }
            }
        else
            joystick = null;
        FixedJoystick[] fixedJoysticks2 = FindObjectsOfType<FixedJoystick>();
        if (GameManager.HandyControllsOn == true)
            foreach (FixedJoystick joysticks2 in fixedJoysticks2)
            {
                if (joysticks2.tag == "Joystick2")
                {
                    joystick2 = joysticks2;
                }
            }
        else
            joystick2 = null;
        if (Input.GetKeyDown(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.B))
        {
            //Acivate John Cena
            JohnCena = true;
        }
        if (photonView.isMine)
            energybar.SetHealth(currentEnergy);
        if (lr.enabled == true && GameObject.FindGameObjectWithTag("RightFist"))
            lr.SetPosition(0, GameObject.FindGameObjectWithTag("RightFist").transform.position);
        lr.SetPosition(1, Hand1.transform.position);
        if (MenuController.power == 1 && photonView.isMine)
        {
            if ((GameManager.HandyControllsOn == false && Input.GetKeyDown(KeyCode.Mouse0)) || (joystick2 != null && joystick2.Direction != new Vector2(0, 0) && Onlyonce2 == true))
            {
                StartCoroutine("shoot");
                Onlyonce2 = false;
            }

            if ((GameManager.HandyControllsOn == false && Input.GetKeyUp(KeyCode.Mouse0)) || (joystick2 != null && joystick2.Direction == new Vector2(0, 0)))
            {
                Onlyonce2 = true;
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
            Dead = true;
            StartCoroutine("deadbody");
            if (MenuController.selectedgamemode != 2 && MenuController.selectedgamemode != 1 && photonView.isMine)
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().StartCoroutine("Respawn");
        }
        if (GameManager.playernumber == 1 && Dead == false && isFrozen == false)
        {
            KeyInput();
        }


    }
    private void FixedUpdate()
    {
        #region <Dash Abillity>
        //dash

        if (MenuController.power == 2)
        {
            if (dashDirection == 0)
            {
                if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.A))
                {
                    dashDirection = 1;
                }

                if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.D))
                {
                    dashDirection = 2;
                }

            }
            else
            {
                if (dashCount <= 0)
                {
                    dashDirection = 0;
                    dashCount = startDashCount;
                    rb.velocity = Vector2.zero;
                }


                dashCount -= Time.deltaTime;
                {
                    
                    if (dashDirection == 1)
                    {
                        rb.velocity = Vector2.left * dashSpeed;
                    }
                    if (dashDirection == 2)
                    {
                        rb.velocity = Vector2.right * dashSpeed;
                    }
                }
            }
        }
          
        #endregion
        if (MenuController.power == 3 && photonView.isMine && transform.localScale.x <= 1.4f)
        {
            transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);


        }
        if (GameManager.playernumber == 1 && Dead == false && isFrozen == false)
        {
            KeyInput2();
        }
    }
    //Use KeyInput2 as FixedUpdate only running when the player isn't dead and the owner of the photonview
    void KeyInput2()
    {
        photonView.RPC("UpdateHealthBar", PhotonTargets.Others, currentHealth);
        if (FlightOn == true)
        {
            currentEnergy -= 0.3f;
            if (currentEnergy <= 1)
            {
                Flightparticles1.Stop();
                Flightparticles2.Stop();
                FlightOn = false;
                Rigidbody2D[] rig2ds = GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D rig in rig2ds)
                {
                    rig.mass = 1;
                }
            }
            if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") > 0) || (joystick != null && joystick.Horizontal >= 0.1))
            {
                rb.AddForce(rb.transform.right * 350 * Time.deltaTime);
            }
            if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") < 0) || (joystick != null && joystick.Horizontal <= -0.1))
            {
                rb.AddForce(rb.transform.right * -350 * Time.deltaTime);
            }
            if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") < 0) || (joystick != null && joystick.Vertical <= -0.1))
            {
                rb.AddForce(rb.transform.up * -500 * Time.deltaTime);
            }
            if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") > 0) || (joystick != null && joystick.Vertical >= -0.1))
            {
                rb.AddForce(rb.transform.up * 2000 * Time.deltaTime);
            }
            if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") > 0) || (joystick != null && joystick.Horizontal >= 0.1))
            {
            }
            else if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") < 0) || (joystick != null && joystick.Horizontal <= -0.1))
            {
            }
            else if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") < 0) || (joystick != null && joystick.Vertical <= -0.1))
            {
            }
            else if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") > 0) || (joystick != null && joystick.Vertical >= -0.1))
            {
            }
            else
            {
                Rigidbody2D[] rig2ds = GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D rig in rig2ds)
                {
                    rig.velocity -= rig.velocity / 50;
                }
            }
        }
        if (HeadOnFire == true)
            Damage2(0.1f);
        if (fireOn == true)
            loseEnergy(0.6f);
        Vector3 Leftarmsscale = leftarm.transform.localScale;
        if (leftarmsize == false && Leftarmsscale.y >= 0.4871715)
        {
            leftarm.transform.localScale -= new Vector3(0, 0.01f, 0);
            leftarm.GetComponent<damage>().multiplyer = 1;
        }
        if (leftarmsize == true && Leftarmsscale.y <= 1)
        {
            leftarm.GetComponent<damage>().multiplyer = 1.5f;
            leftarm.transform.localScale += new Vector3(0, 0.01f, 0);
        }
        if (currentEnergy <= maxEnergy)
            currentEnergy += 0.07f;
        if (regenerating == true && currentHealth <= maxHealth)
        {
            currentHealth += 0.1f;
            healthbar.SetHealth(currentHealth);
        }
        if (JohnCena == true)
            Debug.Log("Hast Du schon Gehofft?");

    }
    //Use KeyInput as Update only running when the player isn't dead and the owner of the photonview
    void KeyInput()
    {
        if (isInWater == true && Bubles.isPlaying == false)
            Bubles.Play();
        else if (isInWater == false && Bubles.isPlaying == true)
            Bubles.Stop();
        if (MenuController.power == 1 && (Input.GetKey(KeyCode.E) || (GameManager.E_pressed == true && photonView.isMine)) && currentEnergy >= maxEnergy)
        {
            GameManager.E_pressed = false;
            StartCoroutine("leftarmgrow");
            currentEnergy = 0;
        }

        Rigidbody2D[] Gravity01 = GetComponentsInChildren<Rigidbody2D>();
        Balance[] Balances = GetComponentsInChildren<Balance>();
        if ((Input.GetKey(KeyCode.E) || (GameManager.E_pressed == true && photonView.isMine)) && MenuController.power == 4 && readytofire == true && currentEnergy >= 50)
        {
            GameManager.E_pressed = false;
            loseEnergy(50);
            if (photonView.isMine)
                PhotonNetwork.Instantiate(GravityBall.name, ShootingPoint2.position, ShootingPoint.rotation, 0);
            StartCoroutine("FireRate");
            readytofire = false;
        }
        if ((Input.GetKey(KeyCode.E) || (GameManager.E_pressed == true && photonView.isMine)) && MenuController.power == 5 && readytofire == true && currentEnergy >= 50)
        {
            GameManager.E_pressed = false;
            if (photonView.isMine)
            {
                loseEnergy(50);
                PhotonNetwork.Instantiate(IceBall.name, ShootingPoint2.position, ShootingPoint.rotation, 0);
            }
            StartCoroutine("FireRate");
            readytofire = false;
        }
        if ((Input.GetKeyDown(KeyCode.E) || (GameManager.E_pressed == true && photonView.isMine)) && MenuController.power == 3 && readytofire == true && currentEnergy >= 50 && isOnGround == true)
        {
            StartCoroutine("stamping");
            GameManager.E_pressed = false;
            loseEnergy(50);
        }
        if ((Input.GetKeyDown(KeyCode.Q) || (GameManager.Q_pressed == true && photonView.isMine)) && MenuController.power == 5 && readytofire == true && currentEnergy >= 50)
        {
            GameManager.Q_pressed = false;
            loseEnergy(50);
            iceField();
        }
        if ((Input.GetKeyDown(KeyCode.Q) || (GameManager.Q_pressed == true && photonView.isMine)) && MenuController.power == 6 && readytofire == true && currentEnergy >= 50)
        {
            GameManager.Q_pressed = false;
            loseEnergy(50);
            FireAttack();
        }
        if ((Input.GetKeyDown(KeyCode.E) || (GameManager.E_pressed == true && photonView.isMine)) && MenuController.power == 6 && currentEnergy >= 1)
        {
            if (fireOn == false)
            {
                fireOn = true;
                photonView.RPC("firestart", PhotonTargets.All);
                FireDamage.SetActive(true);
            }
            else if (fireOn == true)
            {
                photonView.RPC("firestop", PhotonTargets.All);
                fireOn = false;
                FireDamage.SetActive(false);
            }
            GameManager.E_pressed = false;
        }
        if ((Input.GetKeyDown(KeyCode.E) || (GameManager.E_pressed == true && photonView.isMine)) && MenuController.power == 7 && currentEnergy >= 1)
        {
            if (FlightOn == false)
            {
                Flightparticles1.Play();
                Flightparticles2.Play();
                FlightOn = true;
                Rigidbody2D[] rig2ds = GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D rig in rig2ds)
                {
                    rig.mass = 0.05f;
                }
            }
            else if (FlightOn == true)
            {
                Flightparticles1.Stop();
                Flightparticles2.Stop();
                FlightOn = false;
                Rigidbody2D[] rig2ds = GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D rig in rig2ds)
                {
                    rig.mass = 1;
                }
            }
            GameManager.E_pressed = false;
        }

        if (currentEnergy <= 1)
        {
            fireOn = false;
            photonView.RPC("firestop", PhotonTargets.All);
            FireDamage.SetActive(false);

        }
        if ((Input.GetKeyDown(KeyCode.Q) || (GameManager.Q_pressed == true && photonView.isMine)) && MenuController.power == 4 && readytofire == true && currentEnergy >= 100)
        {
            GameManager.Q_pressed = false;
            loseEnergy(100);
            StartCoroutine("changetheGravity");
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
                if (MenuController.power == 4)
                    gravitation.gravityScale = 1.2f;
                else
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
        if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") > 0) || (joystick != null && joystick.Horizontal >= 0.1) && stomp == false && isInWater == false)
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
                if (direction == true && stomp == false)
                    anim.Play("Idle");
                if (direction == false && stomp == false)
                    anim.Play("Idle2");
            }

        }
        else if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") < 0) || (joystick != null && joystick.Horizontal <= -0.1) && isInWater == false)
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
                if (direction == true && stomp == false)
                    anim.Play("Idle");
                if (direction == false && stomp == false)
                    anim.Play("Idle2");
            }
        }
        else
        {
            if (direction == true && stomp == false && isInWater == false)
                anim.Play("Idle");
            if (direction == false && stomp == false && isInWater == false)
                anim.Play("Idle2");
        }
        if (SwimRotation > 360)
            SwimRotation = 1;
        else if (SwimRotation < 0)
            SwimRotation = 359;
        if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") > 0) || (joystick != null && joystick.Horizontal >= 0.1) && stomp == false)
        {
            if (isInWater == true)
            {
                if (SwimRotation != 270)
                {
                    if (SwimRotation >= 90 && SwimRotation < 270)
                        SwimRotation += 0.5f;
                    else
                        SwimRotation += -0.5f;
                }
                rb.AddForce(rb.transform.up * 10000 * Time.deltaTime);
            }

        }
        else if ((Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Horizontal") < 0) || (joystick != null && joystick.Horizontal <= -0.1) && stomp == false)
        {

            if (isInWater == true)
            {

                if (SwimRotation != 90)
                {
                    if (SwimRotation >= 270 && SwimRotation < 90)
                        SwimRotation += 0.5f;
                    else
                        SwimRotation += -0.5f;
                }
                rb.AddForce(rb.transform.up * 10000 * Time.deltaTime);
            }
        }
        else if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") < 0) || (joystick != null && joystick.Vertical <= -0.1) && stomp == false)
        {
            if (isInWater == true)
            {
                if (SwimRotation != 180)
                {
                    if (SwimRotation < 180)
                        SwimRotation += 0.5f;
                    else
                        SwimRotation += -0.5f;
                }
                rb.AddForce(rb.transform.up * 10000 * Time.deltaTime);
            }
        }
        else if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") > 0) || (joystick != null && joystick.Vertical >= -0.1) && stomp == false)
        {

            if (isInWater == true)
            {

                if (SwimRotation != 0)
                {
                    if (SwimRotation >= 180)
                        SwimRotation += 0.5f;
                    else
                        SwimRotation += -0.5f;
                }
                rb.AddForce(rb.transform.up * 10000 * Time.deltaTime);
            }
        }
            if (isInWater == true)
            {
                
                anim.enabled = false;
                foreach (Balance swimming in GetComponentsInChildren<Balance>())
                {
                    swimming.targetRotation = SwimRotation;
                }
                foreach (BalanceArms swimming in GetComponentsInChildren<BalanceArms>())
                {
                    swimming.force = 20;
                    swimming.targetRotation = SwimRotation + swimmanim;
                }
            }
            else
                anim.enabled = true;
            if ((isOnGround == false && Input.GetKeyDown(KeyCode.Space) || (joystick != null && joystick.Vertical >= 0.3 && isOnGround == false) && Onlyonce == true) && DoubleJump == true && MenuController.power == 2)
            {
                DBCoolDown = true;
                StartCoroutine("DoubleJumpCooldown");
                DoubleJump = false;
                Onlyonce = false;
                if (gravity == false)
                    rb.AddForce(Vector2.up * jumpForce);
                else
                    rb.AddForce(Vector2.down * jumpForce);
            }
            if (isOnGround == true && DBCoolDown == false)
                DoubleJump = true;
            isOnWallLeft = Physics2D.OverlapCircle(playerPos2.position, positionRadius, ground);
            isOnWallRight = Physics2D.OverlapCircle(playerPos1.position, positionRadius, ground);
            isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);
            isInWater = Physics2D.OverlapCircle(playerPos.position, positionRadius, water);
            if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.H))
        if (isInWater == true)
        {

            anim.enabled = false;
            foreach (Balance swimming in GetComponentsInChildren<Balance>())
            {
                swimming.targetRotation = SwimRotation;
            }
            foreach (BalanceArms swimming in GetComponentsInChildren<BalanceArms>())
            {
                swimming.force = 20;
                swimming.targetRotation = SwimRotation + swimmanim;
            }
        }
        else
            anim.enabled = true;
        if (isOnGround == true && Input.GetKeyDown(KeyCode.Space) || (joystick != null && joystick.Vertical >= 0.3 && isOnGround == true) && Onlyonce == true)
        {
            Onlyonce = false;
            if (gravity == false)
                rb.AddForce(Vector2.up * jumpForce);
            else
                rb.AddForce(Vector2.down * jumpForce);
        }
        else if (joystick != null && joystick.Vertical <= 0.3)
        {
            Onlyonce = true;
        }
        if ((isOnGround == false && Input.GetKeyDown(KeyCode.Space) || (joystick != null && joystick.Vertical >= 0.3 && isOnGround == false) && Onlyonce == true) && DoubleJump == true && MenuController.power == 2)
        {
            DBCoolDown = true;
            StartCoroutine("DoubleJumpCooldown");
            DoubleJump = false;
            Onlyonce = false;
            if (gravity == false)
                rb.AddForce(Vector2.up * jumpForce);
            else
                rb.AddForce(Vector2.down * jumpForce);
        }
        if (isOnGround == true && DBCoolDown == false)
            DoubleJump = true;
        isOnWallLeft = Physics2D.OverlapCircle(playerPos2.position, positionRadius, ground);
        isOnWallRight = Physics2D.OverlapCircle(playerPos1.position, positionRadius, ground);
        isOnGround = Physics2D.OverlapCircle(playerPos.position, positionRadius, ground);
        isInWater = Physics2D.OverlapCircle(playerPos.position, positionRadius, water);
        if (Input.GetKey(KeyCode.J) && Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.N) && Input.GetKey(KeyCode.H))
        {
            maxHealth = 3000;
            currentHealth = maxHealth;
            damage[] dammage = GetComponentsInChildren<damage>();
            foreach (damage DAMAGE in dammage)
            {
                DAMAGE.multiplyer = 5;
            }
            playerSpeed = 5000;
            jumpForce = 7000;
        }
        if (isOnGround == false && isOnWallRight == true && Input.GetKeyDown(KeyCode.Space) || (joystick != null && joystick.Vertical >= 0.3) && Onlyonce == true && isOnGround == false && isOnWallLeft == true)
        {
            Onlyonce = false;
            rb.AddForce(Vector2.left * WalljumpForce);
            if (gravity == false)
                rb.AddForce(Vector2.up * WalljumpForce);
            else
                rb.AddForce(Vector2.down * WalljumpForce);
            direction = false;
        }
        if (isOnGround == false && isOnWallLeft == true && Input.GetKeyDown(KeyCode.Space) || (joystick != null && joystick.Vertical >= 0.3) && Onlyonce == true && isOnGround == false && isOnWallLeft == true)
        {
            Onlyonce = false;
            rb.AddForce(Vector2.right * WalljumpForce);
            if (gravity == false)
                rb.AddForce(Vector2.up * WalljumpForce);
            else
                rb.AddForce(Vector2.down * WalljumpForce);
            direction = true;
        }
    }
    #endregion
}
