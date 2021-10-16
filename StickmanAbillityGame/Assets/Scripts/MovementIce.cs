using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementIce : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    public PhotonView photonView;
    void Start()
    {
        rb.velocity = transform.right * Speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerController>() && photonView.isMine)
        {
            collision.gameObject.GetComponentInParent<PlayerController>().Freeze1();
        }
        if (collision.gameObject.GetComponentInParent<TheBraaiiinnn>() && photonView.isMine)
        {
            Debug.Log("adawd");
            collision.gameObject.GetComponentInParent<TheBraaiiinnn>().Freeze1();
        }
        if (photonView.isMine && !collision.GetComponentInParent<IceDragonManager>())
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
    void Update()
    {
    }
}
