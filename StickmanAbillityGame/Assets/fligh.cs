using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fligh : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    private PhotonView photonView;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb.velocity = transform.right * Speed;
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponentInParent<PlayerController>())
        {
            rb.velocity = new Vector3(0, 0, 0);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Grapple(transform.position);
        }
    }
    [PunRPC]
    public void stopGrapling()
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Line(transform.position);
        if (Input.GetKeyUp(KeyCode.Mouse0) && MenuController.power == 1)
            photonView.RPC("stopGrapling", PhotonTargets.All);
    }
}
