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

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        cam = FindObjectOfType<Camera>();
    }


    void Update()
    {
        if(photonView.isMine)
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

            Vector3 playerpos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 90);
            Vector3 difference = playerpos - transform.position;
            float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;
            if (Input.GetKey(mousebutton))
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