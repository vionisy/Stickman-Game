using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fligh : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    private PhotonView photonView;
    private bool stop = false;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        rb.velocity = transform.right * Speed;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponentInParent<PlayerController>())
        {
            stop = true;
            GetComponent<FixedJoint2D>().enabled = true;
            if (collision.gameObject.GetComponent<Rigidbody2D>())
            {                 
                GetComponent<FixedJoint2D>().connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
                photonView.RPC("connectRigidbody", PhotonTargets.Others, collision.gameObject.GetComponent<Rigidbody2D>());
            }
            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
            foreach(GameObject PLAYER in player)
            {
                if (PLAYER.GetComponent<PlayerController>().photonView.isMine)
                    PLAYER.GetComponent<PlayerController>().Grapple(transform.position, GetComponent<Rigidbody2D>());
            }
        }
    }
    [PunRPC]
    public void connectRigidbody(Rigidbody2D RB)
    {
        Debug.Log("connect");
        GetComponent<FixedJoint2D>().connectedBody = RB;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    private void Update()
    {
        //if (stop == true)
            //rb.velocity = new Vector3(0, 0, 0);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().Line(transform.position);
        }
        //if ()
        //player.Line(transform.position);
        if (Input.GetKeyUp(KeyCode.Mouse0) && MenuController.power == 1)
            PhotonNetwork.Destroy(gameObject);
    }
}
