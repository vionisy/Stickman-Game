using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceArms : MonoBehaviour
{
    private FixedJoystick joystick;
    public float targetRotation;
    public Rigidbody2D rb;
    public float force;
    public float stop;
    public KeyCode mousebutton;
    public PhotonView photonView;
    float smoothRotation;
    public void disable()
    {
        force = 0;
        Debug.Log("stop");
    }
    public void FixedUpdate()
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
        if (!Input.GetKey(mousebutton) && photonView.isMine && joystick == null)
        {
            if (PlayerController.Gravitation == true)
            {
                if (smoothRotation < 180)
                {
                    smoothRotation += 2f;
                }
            }
            else
            {
                if (smoothRotation > 1)
                {

                    smoothRotation -= 2f;
                }
            }

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation - smoothRotation, force * Time.fixedDeltaTime));
        }
        else if (joystick != null && !(joystick.Vertical != 0 || joystick.Horizontal != 0) && photonView.isMine)
        {
            if (PlayerController.Gravitation == true)
            {
                if (smoothRotation < 180)
                {
                    smoothRotation += 2f;
                }
            }
            else
            {
                if (smoothRotation > 1)
                {

                    smoothRotation -= 2f;
                }
            }

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation - smoothRotation, force * Time.fixedDeltaTime));
        }
    }
}