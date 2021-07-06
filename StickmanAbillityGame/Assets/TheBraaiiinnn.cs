using System.Collections;
using UnityEngine;

public class TheBraaiiinnn : MonoBehaviour
{
    public ParticleSystem Bubles;
    private bool jump = false;
    private bool armsActive;
    private float armRotation;
    private bool GoRight = false;
    private bool GoLeft = false;
    private StressReceiver camerashake;
    private bool stomp = false;
    private bool DoubleJump;
    public GameObject IceFoot1;
    public GameObject IceFoot2;
    public GameObject FireDamage;
    public ParticleSystem FireParticles;
    public ParticleSystem OnFireParticles;
    private bool fireOn = false;
    public Rigidbody2D Headrb;
    public Rigidbody2D LeftLowLeg;
    public Rigidbody2D RightLowLeg;
    private bool isFrozen = false;
    private bool Onlyonce1 = true;
    private bool Onlyonce2 = true;
    private bool JohnCena = false;
    private FixedJoystick joystick;
    private FixedJoystick joystick2;
    public GameObject leftarm;
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
    public LayerMask water;
    public Transform playerPos;
    public float GravitationScale = -1.5f;
    private bool direction;
    public PhotonView photonView;
    private float SaveJumpForce;
    public Transform playerPos1;
    public Transform playerPos2;
    private bool isOnWallLeft;
    private bool isInWater;
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
    public GameObject IceBall;
    public float firerate = 0.2f;
    private bool readytofire = true;
    private bool sizeBackToNormal = false;
    private bool leftarmsize = false;
    private int jumpBoost = 0;
    private int speedBoost = 0;
    private int strengthBoost = 0;
    private int healthBoost = 0;
    public LayerMask LayerToFreeze;
    public float IcefieldofImpact;
    public PhysicsMaterial2D HighFriction;
    public PhysicsMaterial2D IceFriction;
    public ParticleSystem psIce;
    public ParticleSystem psFire;
    private bool HeadOnFire = false;

    public void PlayerControler(float walljumpForce)
    {
        WalljumpForce = walljumpForce;
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
    public void Fireattack()
    {
        if (!photonView.isMine)
            photonView.RPC("ThisGuyIsOnFire", PhotonTargets.Others);
    }
    [PunRPC]
    public void ThisGuyIsOnFire()
    {
        StartCoroutine("FireHead");
    }
    private IEnumerator FireHead()
    {
        HeadOnFire = true;
        photonView.RPC("FireOn", PhotonTargets.All);
        yield return new WaitForSeconds(9);
        photonView.RPC("FireOff", PhotonTargets.All);
        HeadOnFire = false;
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
        }
    }
    public void FreezeFeet1()
    {
        if (!photonView.isMine)
            photonView.RPC("FreezeFeet2", PhotonTargets.Others);
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
    [PunRPC]
    public void OponentHealth(float thehealths)
    {
        if (Oponenthealthbar)
            Oponenthealthbar.SetHealth(thehealths);
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
    public void delete()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    [PunRPC]
    public void hidehealthbar()
    {
        GetComponentInChildren<HelthBar>().gameObject.SetActive(false);
    }
    private void Start()
    {
        HelthBar[] oponenthealthbars1 = GetComponentsInChildren<HelthBar>();
        foreach (HelthBar thehealth in oponenthealthbars1)
        {
            thehealth.SetMaxHealth(maxHealth);
            thehealth.gameObject.SetActive(true);
            thehealth.transform.parent.gameObject.SetActive(true);
            healthbar = thehealth;
        }
        foreach (damage Damagemultiplyer in GetComponentsInChildren<damage>())
            Damagemultiplyer.multiplyer = 0.5f;
        camerashake = FindObjectOfType<Camera>().GetComponent<StressReceiver>();
        OnFireParticles.Stop();
        FireParticles.Stop();
        psIce.Stop();
        currentEnergy = maxEnergy;
        readytofire = true;
        springjoint.enabled = false;
        FollowMouse[] followMouse = GetComponentsInChildren<FollowMouse>();
        foreach (FollowMouse FollowTheMouse in followMouse)
        {
            FollowTheMouse.enabled = true;
        }
        springjoint = GetComponentInChildren<SpringJoint2D>();
        cam = FindObjectOfType<Camera>();
        //healthbar = GameObject.FindGameObjectWithTag("OwnHealthBar").GetComponent<HelthBar>();
        energybar = GameObject.FindGameObjectWithTag("EnergyBar").GetComponent<HelthBar>();
        GameObject[] oponenthealthbars = GameObject.FindGameObjectsWithTag("OponentsHealthbar");
        foreach (GameObject thehealth in oponenthealthbars)
        {
            thehealth.SetActive(true);
            healthbar = thehealth.GetComponentInChildren<HelthBar>();
        }
        energybar.SetMaxHealth(maxEnergy);
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        if (Oponenthealthbar)
        {
            Oponenthealthbar.SetMaxHealth(maxHealth);
        }
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
        //PhotonNetwork.Instantiate(GravityBall.name, ShootingPoint2.position, ShootingPoint.rotation, 0);
    }

    private IEnumerator FireRate()
    {
        yield return new WaitForSeconds(firerate);
        readytofire = true;
    }
    private bool stop = true;
    public void Freeze1()
    {
        if (!photonView.isMine)
        {
            photonView.RPC("Freeze2", PhotonTargets.Others);
            Debug.Log("Freeze1");
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
                theplayers.GetComponent<PlayerController>().Damage(40);
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
    private IEnumerator YourFrozen()
    {
        isFrozen = true;
        yield return new WaitForSeconds(5);
        isFrozen = false;
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
    private void Update()
    {
        //healthbar.SetMaxHealth(maxHealth);
        Brain();
        if (isFrozen == false && Dead == false)
        {
            //photonView.RPC("Frooozen2", PhotonTargets.All);
            Headrb.mass = 0.5f;
            LeftLowLeg.mass = 1f;
            RightLowLeg.mass = 1f;
            FixedJoint2D[] freeze = GetComponentsInChildren<FixedJoint2D>();
            foreach (FixedJoint2D frozen in freeze)
            {
                if (frozen.gameObject.name != "Chest" && frozen.gameObject.name != "Neck")
                {
                    frozen.enabled = false;
                }
            }
            Balance[] balance = GetComponentsInChildren<Balance>();
            foreach (Balance theBalance in balance)
            {
                theBalance.enabled = true;
            }
            FollowMouse[] mouse = GetComponentsInChildren<FollowMouse>();
            foreach (FollowMouse follow in mouse)
            {
                follow.enabled = true;
            }
            BalanceArms[] balancearms = GetComponentsInChildren<BalanceArms>();
            foreach (BalanceArms theBalance in balancearms)
            {
                theBalance.enabled = true;
            }
        }
        else if (isFrozen == true && Dead == false)
        {
            //photonView.RPC("Frooozen1", PhotonTargets.All);
            LeftLowLeg.mass = 0.5f;
            RightLowLeg.mass = 0.5f;
            Headrb.mass = 10f;
            FixedJoint2D[] freeze = GetComponentsInChildren<FixedJoint2D>();
            foreach (FixedJoint2D frozen in freeze)
            {
                if (frozen.gameObject.name != "Chest" && frozen.gameObject.name != "Neck")
                {
                    frozen.enabled = true;
                }
            }
            Balance[] balance = GetComponentsInChildren<Balance>();
            foreach (Balance theBalance in balance)
            {
                theBalance.enabled = false;
            }
            BalanceArms[] balancearms = GetComponentsInChildren<BalanceArms>();
            foreach (BalanceArms theBalance in balancearms)
            {
                theBalance.enabled = false;
            }
            FollowMouse[] mouse = GetComponentsInChildren<FollowMouse>();
            foreach (FollowMouse follow in mouse)
            {
                follow.enabled = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            jumpBoostLevelUp();
            speedBoostLevelUp();
            strengthBoostLevelUp();
            healthBoostLevelUp();
        }
        energybar.SetHealth(currentEnergy);
        

        if (currentHealth <= 0 && Dead == false)
        {
            Dead = true;
            StartCoroutine("deadbody");
        }
        if (photonView.isMine && Dead == false && isFrozen == false)
        {
            KeyInput();
        }

    }
    private void FixedUpdate()
    {
        if (photonView.isMine && Dead == false && isFrozen == false)
        {
            KeyInput2();
        }
    }
    void KeyInput2()
    {
        if (HeadOnFire == true)
            Damage2(0.1f);
        if (fireOn == true)
            loseEnergy(0.6f);
        Vector3 Leftarmsscale = leftarm.transform.localScale;
        if (currentEnergy <= maxEnergy)
            currentEnergy += 0.07f;
        if (regenerating == true && currentHealth <= maxHealth)
        {
            currentHealth += 0.1f;
            photonView.RPC("UpdateHealthBar", PhotonTargets.Others, currentHealth);
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
    private IEnumerator sizeBack()
    {
        yield return new WaitForSeconds(20);
        sizeBackToNormal = true;
    }
    [PunRPC]
    public void GravitationChange(bool theGravitation)
    {
        Gravitation = theGravitation;
    }
    void KeyInput()
    {
        if (isInWater == true && Bubles.isPlaying == false)
            Bubles.Play();
        else if (isInWater == false && Bubles.isPlaying == true)
                Bubles.Stop();

        Rigidbody2D[] Gravity01 = GetComponentsInChildren<Rigidbody2D>();
        Balance[] Balances = GetComponentsInChildren<Balance>();
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
        if (GoRight == true)
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
        else if (GoLeft == true)
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
            if (direction == true && stomp == false)
                anim.Play("Idle");
            if (direction == false && stomp == false)
                anim.Play("Idle2");
        }
        if (GoRight == true)
        {
            if (isInWater == true)
            {
                rb.AddForce(Vector2.right * 8000 * Time.deltaTime);
            }

        }
        else if (GoLeft == true)
        {
            if (isInWater == true)
            {
                rb.AddForce(Vector2.left * 8000 * Time.deltaTime);
            }
        }
        if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") < 0) || (joystick != null && joystick.Vertical <= -0.1) && stomp == false)
        {
            if (isInWater == true)
            {
                rb.AddForce(Vector2.down * 6000 * Time.deltaTime);
            }
        }
        else if ((Input.GetAxisRaw("Vertical") != 0 && Input.GetAxisRaw("Vertical") > 0) || (joystick != null && joystick.Vertical >= -0.1) && stomp == false)
        {
            if (isInWater == true)
            {
                rb.AddForce(Vector2.up * 6000 * Time.deltaTime);
            }
        }

        if (isOnGround == true && jump == true)
        {
            jump = false;
            if (gravity == false)
                rb.AddForce(Vector2.up * jumpForce);
            else
                rb.AddForce(Vector2.down * jumpForce);
        }
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
        if (isOnGround == false && isOnWallRight == true && jump == true)
        {
            jump = false;
            rb.AddForce(Vector2.left * WalljumpForce);
            if (gravity == false)
                rb.AddForce(Vector2.up * WalljumpForce);
            else
                rb.AddForce(Vector2.down * WalljumpForce);
            direction = false;
        }
        if (isOnGround == false && isOnWallLeft == true && jump == true)
        {
            jump = false;
            rb.AddForce(Vector2.right * WalljumpForce);
            if (gravity == false)
                rb.AddForce(Vector2.up * WalljumpForce);
            else
                rb.AddForce(Vector2.down * WalljumpForce);
            direction = true;
        }

    }
    private IEnumerator DoubleJumpCooldown()
    {
        yield return new WaitForSeconds(5);
        DBCoolDown = false;
    }
    private bool DBCoolDown = false;
    private IEnumerator visible()
    {
        photonView.RPC("sidebar", PhotonTargets.Others);
        yield return new WaitForSeconds(1.5f);
        photonView.RPC("Invisibillity", PhotonTargets.Others);
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

    private void jumpBoostLevelUp()
    {
        jumpBoost += 1;
        jumpForce += 500;
        SaveJumpForce += 500;

    }

    private void speedBoostLevelUp()
    {
        speedBoost += 1;
        playerSpeed += 250;
    }
    private void strengthBoostLevelUp()
    {
        strengthBoost += 1;
        damage[] strength = GetComponentsInChildren<damage>();
        foreach (damage strength2 in strength)
        {
            strength2.multiplyer += 0.1f;
        }
    }
    private void healthBoostLevelUp()
    {
        healthBoost += 1;
        maxHealth += 25f;
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
    private IEnumerator changetheGravity()
    {
        if (MenuController.power == 4)
        {
            PlayerController.Gravitation = true;
            yield return new WaitForSeconds(15);
            PlayerController.Gravitation = false;
        }
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Head");
        if (gos != null)
        {
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            return closest;
        }
        else
            return null;
    }
    private IEnumerator ArmHit1()
    {

        armsActive = true;
        //armRotation = 220;
        StartCoroutine(ChangeSpeed(armRotation, Random.Range(180f, 230f), 0.3f));
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        //armRotation = 70;
        StartCoroutine(ChangeSpeed(armRotation, Random.Range(80f, 50f), 0.2f));
        yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        armsActive = false;
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        Onlyonce1 = true;
    }
    private IEnumerator ArmHit2()
    {
        armsActive = true;
        //armRotation = 10;
        StartCoroutine(ChangeSpeed(armRotation, Random.Range(20f, 0f), 0.3f));
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));
        //armRotation = 170;
        StartCoroutine(ChangeSpeed(armRotation, Random.Range(160f, 190f), 0.2f));
        yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
        armsActive = false;
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        Onlyonce1 = true;
    }
    private bool AttackPlayer = false;
    private void Brain()
    {
        float decision = Random.Range(1, 2);
        if (decision == 1)
        {

        }
        else if (decision == 2)
        {

        }
        if (AttackPlayer == true)
        {
            if (FindClosestEnemy() != null && !(FindClosestEnemy().transform.position.x > rb.transform.position.x))
            {
                GoLeft = true;
                GoRight = false;
            }
            else
            {
                GoLeft = false;
                GoRight = true;
            }
        }
        KI_Arms[] arms = GetComponentsInChildren<KI_Arms>();
        foreach(KI_Arms thearms in arms)
        {
            if (direction == false)
            {
                thearms.SetRotationState(armRotation * -1);
            }
            else
            {
                thearms.SetRotationState(armRotation);
            }
            thearms.SetActiveState(armsActive);
        }
        if (FindClosestEnemy() != null && Vector3.Distance(FindClosestEnemy().transform.position, rb.transform.position) <= 10f && Onlyonce1 == true)
        {
            if (Random.value <= 0.65)
                StartCoroutine("ArmHit1");
            else
                StartCoroutine("ArmHit2");
            Onlyonce1 = false;
        }
    }
    IEnumerator ChangeSpeed(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            armRotation = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        armRotation = v_end;
    }
}
