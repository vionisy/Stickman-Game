using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    public float targetRotation;
    public Rigidbody2D rb;
    public float force;
    public float stop;
    public KeyCode mousebutton;
    float smoothRotation;

    public void disable()
    {
        force = 0;
        Debug.Log("stop");
    }
    public void Update()
    {
        if (!Input.GetKey(mousebutton))
        {
            if (Input.GetKey(KeyCode.Q))
            {
                if (smoothRotation < 180)
                {
                    smoothRotation += 0.3f;
                }
            }
            else
            {
                if (smoothRotation > 1)
                {
                    
                    smoothRotation -= 0.3f;
                }
            }

            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation - smoothRotation, force * Time.fixedDeltaTime));

        }
    }
}
