using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    private bool damage = false;
    private PlayerController player;
    private PhotonView photonView;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage = true;
        Debug.Log("lol");
        photonView = collision.gameObject.GetComponentInParent<PhotonView>();
        player = collision.gameObject.GetComponentInParent<PlayerController>();
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        damage = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (photonView && !photonView.isMine && player && damage == true)
        {
            player.Damage(1.2f);
        }
    }
}
