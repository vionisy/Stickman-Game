using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceArms : MonoBehaviour
{
    public float targetRotation;
    public Rigidbody2D rb;
    public float force;
    public float stop;
    public KeyCode mousebutton;
    public PhotonView photonView;

    public void disable()
    {
        force = 0;
        Debug.Log("stop");
    }
    public void Update()
    {
        if (!Input.GetKey(mousebutton) && photonView.isMine)
        {
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));
        }
    }
}