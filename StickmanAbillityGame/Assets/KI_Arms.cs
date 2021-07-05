using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KI_Arms : MonoBehaviour
{
    public float Maxspeed = 35;
    private float speed = 35;
    public Rigidbody2D rb;
    public Camera cam;
    public KeyCode mousebutton;
    public PhotonView photonView;
    private TheBraaiiinnn playerController;
    private FixedJoystick joystick;
    private float rotationZ;
    private bool active = false;
    private bool active2 = false;
    private void Awake()
    {
        playerController = GetComponentInParent<TheBraaiiinnn>();
    }


    void Update()
    {
        FixedJoystick[] fixedJoysticks = FindObjectsOfType<FixedJoystick>();
        if (GameManager.HandyControllsOn == true)
            foreach (FixedJoystick joysticks in fixedJoysticks)
            {
                if (joysticks.tag == "Joystick2")
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
            else
            {
                active2 = false;
            }
            if (photonView.isMine && (active == true || active2 == true))
            {
                rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.fixedDeltaTime));
            }
        }

        if (playerController.Dead == true)
        {
            speed = 0;
            Maxspeed = 0;
        }
        else if (playerController.Dead == false)
        {
            speed = 35;
            Maxspeed = 35;
        }
    }
    public void SetActiveState(bool state)
    {
        active = state;
        if (state == true)
            gameObject.GetComponent<BalanceArms>().stopittrue();
        else
            gameObject.GetComponent<BalanceArms>().stopitfalse();
    }
    public void SetRotationState(float angle)
    {
        rotationZ = angle;
    }
}