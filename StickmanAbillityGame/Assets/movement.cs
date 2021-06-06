using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    void Start()
    {
        rb.velocity = transform.right * Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerDamage = collision.gameObject.GetComponentInParent<PlayerController>();
        if (playerDamage)
        {
            playerDamage.Damage(10);
        }
        PhotonNetwork.Destroy(gameObject);
    }
    void Update()
    {
        
    }
}
