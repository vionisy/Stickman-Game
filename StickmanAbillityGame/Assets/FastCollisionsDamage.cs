using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastCollisionsDamage : MonoBehaviour
{
    private Vector3 oldPosition;
    private float speed;
    void FixedUpdate()
    {
        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (speed >= 20)
        {
            if (!collision.gameObject.GetComponentInParent<PlayerController>())
            {
                speed -= 10;
                if (!collision.gameObject.GetComponentInParent<PlayerController>())
                {
                    GetComponentInParent<PlayerController>().Damage2(speed * 0.6f);
                }
                //collision.gameObject.GetComponentInParent<PlayerController>().Damage(speed * 0.4f);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
