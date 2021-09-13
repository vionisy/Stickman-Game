using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VulcanoDamage : MonoBehaviour
{
    public float damagemultiplyer = 1;
    private bool damage = false;
    private PlayerController player;
    private PhotonView photonView;
    private TheBraaiiinnn Ai;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        damage = true;
        player = collision.gameObject.GetComponentInParent<PlayerController>();
        Ai = collision.gameObject.GetComponentInParent<TheBraaiiinnn>();

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        damage = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (player && damage == true)
        {
            if (player.photonView.isMine)
                player.Damage2(10f * damagemultiplyer);
            //else
                //player.Damage(10f * damagemultiplyer);
        }
        if (Ai && damage == true && Ai.isBoss == false)
        {
            if (Ai.photonView.isMine)
                Ai.Damage(10f);
        }
    }
}
