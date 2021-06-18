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
    private bool active = false;
    private bool active2 = false;
    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        cam = FindObjectOfType<Camera>();
    }


    void Update()
    {
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
        if (photonView.isMine)
        {
            if (Time.timeScale == 0.5)
            {
                speed = 5;
            }
            else if (Time.timeScale == 1)
            {
                speed = Maxspeed;
            }
            else if (Time.timeScale != 1 && Time.timeScale != 0.5)
            {
                speed = 2;
            }
            if (joystick == null && Input.GetKey(mousebutton))
            {
                active = true;
            }
            else
            {
                active = false;
            }
            if (joystick != null && (joystick.Vertical != 0 || joystick.Horizontal != 0))
            {
                active2 = true;
            }
            else
            {
                active2 = false;
            }
            Vector3 playerpos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 90);
            Vector3 difference = playerpos - transform.position;
            if(GameManager.HandyControllsOn == false)
                rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;
            else if (GameManager.HandyControllsOn == true)
                rotationZ = Mathf.Atan2(joystick.Direction.x, -joystick.Direction.y) * Mathf.Rad2Deg;
            if (photonView.isMine && (active == true || active2 == true))
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.fixedDeltaTime));
            }
        }
    
        if(playerController.Dead == true)
        {
            speed = 0;
        }
        else if(playerController.Dead == false)
        {
            speed = 300;
        }

    
    }
}