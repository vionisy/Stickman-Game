using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>())
        {
            GetComponentInParent<IceDragonManager>().collided = true;
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
