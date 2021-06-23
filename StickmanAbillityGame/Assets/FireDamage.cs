using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("lol");
        PhotonView photonView = collision.gameObject.GetComponentInParent<PhotonView>();
        PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
        if (photonView && !photonView.isMine && player)
        {
            player.Damage(0.08f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
