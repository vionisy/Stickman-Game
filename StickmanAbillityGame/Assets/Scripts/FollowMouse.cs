using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float Maxspeed = 35;
    private float speed = 35;
    public Rigidbody2D rb;
    public Camera cam;
    public KeyCode mousebutton;
    public PhotonView photonView;
    private PlayerController playerController;
    private FixedJoystick joystick;
    private float rotationZ;
    public bool active = false;
    private bool active2 = false;
    private Vector3 playerpos;
    private void Awake()
    {
        playerpos.z = 90;
        playerController = GetComponentInParent<PlayerController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    void FixedUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Dragonfightcam") && GameObject.FindGameObjectWithTag("Dragonfightcam").activeSelf == true)
        {
            cam = GameObject.FindGameObjectWithTag("Dragonfightcam").GetComponent<Camera>();
        }
        FixedJoystick[] fixedJoysticks = FindObjectsOfType<FixedJoystick>();
        if (GameManager.HandyControllsOn == true)
            foreach (FixedJoystick joysticks in fixedJoysticks)
            {
                if(joysticks.tag == "Joystick2")
                {
                    joystick = joysticks;
                }
            }
        else
            joystick = null;
        if (joystick == null && Input.GetKey(mousebutton) && GetComponentInParent<PlayerController>().ownplayernumber == GameManager.playernumber)
        {
            active = true;
            photonView.RPC("setactive", PhotonTargets.MasterClient, true);
        }
        else if (GetComponentInParent<PlayerController>().ownplayernumber == GameManager.playernumber)
        {
            active = false;
            photonView.RPC("setactive", PhotonTargets.MasterClient, false);
        }
        if (joystick != null && (joystick.Vertical != 0 || joystick.Horizontal != 0))
        {
            photonView.RPC("setactive2", PhotonTargets.MasterClient, true);
        }
        else
        {
            photonView.RPC("setactive2", PhotonTargets.MasterClient, false);
        }
        if (GetComponentInParent<PlayerController>().ownplayernumber == GameManager.playernumber && active == true)
        {
             Vector2 valueToSend = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
             photonView.RPC("mousefollow1", PhotonTargets.MasterClient, valueToSend.x);
             photonView.RPC("mousefollow2", PhotonTargets.MasterClient, valueToSend.y);
        }
        
        if (playerController.Dead == true)
        {
            speed = 0;
            Maxspeed = 0;
        }
        else if(playerController.Dead == false)
        {
            speed = 35;
            Maxspeed = 35;
        }
        if (GameManager.playernumber == 1)
        { 
            Vector3 difference = playerpos - transform.position;
            if (GameManager.HandyControllsOn == false)
                rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;
            else if (GameManager.HandyControllsOn == true)
                rotationZ = Mathf.Atan2(joystick.Direction.x, -joystick.Direction.y) * Mathf.Rad2Deg;
            if (GameManager.playernumber == 1 && (active == true || active2 == true))
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.fixedDeltaTime));
            }
        }
    }
    [PunRPC]
    public void mousefollow1(float thepos)
    {
        playerpos.x = thepos;
    }
    [PunRPC]
    public void mousefollow2(float thepos)
    {
        playerpos.y = thepos;
    }
    [PunRPC]
    public void setactive(bool state)
    {
        active = state;
        if (active == true)
            GetComponent<BalanceArms>().stopittrue();
        else if (active == false)
            GetComponent<BalanceArms>().stopitfalse();
    }
    [PunRPC]
    public void setactive2(bool state)
    {
        active2 = state;
    }
}