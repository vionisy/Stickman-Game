using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float speed = 300;
    public Rigidbody2D rb;
    public Camera cam;
    public KeyCode mousebutton;

    private void Awake()
    {
        cam = FindObjectOfType<Camera>();
    }
    

    void Update()
    {
        
        Vector3 playerpos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 90);
        Vector3 difference = playerpos - transform.position;
        float rotationZ = Mathf.Atan2(difference.x, -difference.y) * Mathf.Rad2Deg;
        if (Input.GetKey(mousebutton))
        {
            rb.MoveRotation(Mathf.LerpAngle(rb.rotation, rotationZ, speed * Time.fixedDeltaTime));
        }

    }
}