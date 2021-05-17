using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float speed = 300;
    public Rigidbody2D rb;
    public Camera cam;
    public KeyCode mousebutton;
    public PhotonView photonView;

    private void Awake()
    {
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
                speed = 300;
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
    }
}