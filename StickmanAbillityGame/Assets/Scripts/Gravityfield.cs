using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravityfield : MonoBehaviour
{
    public PhotonView photonView;
    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    // Start is called before the first frame update
    private IEnumerator delete()
    {
        yield return new WaitForSeconds(2.5f);
        if (photonView.isMine)
        {
            Debug.Log("Destroy");
            //PhotonNetwork.Destroy(gameObject);
        }
    }
    void Start()
    {
        if (photonView.isMine)
        {
            StartCoroutine("delete");
        }
    }
    public void gravitate(float speed)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayerToHit);
        foreach (Collider2D obj in objects)
        {
            PlayerController player = obj.GetComponentInParent<PlayerController>();
            if (player && !photonView.isMine)
            { 
                if (player.photonView.isMine)
                {
                    player.Damage2(0.008f);
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * (speed * -1));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("LOOOOOOOOOOOOOOOOOLLLLLLLLL");
        gravitate(force);
    }
}
