using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public ParticleSystem ps;
    public float Speed;
    public Rigidbody2D rb;
    public GameObject gravityfield;
    public PhotonView photonView;
    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    public bool gravitation = false;
    void Start()
    {
        ps.Stop();
        rb.velocity = transform.right * Speed;
    }
    private IEnumerator delete()
    {
        yield return new WaitForSeconds(3f);
        if (photonView.isMine)
        {
            Debug.Log("Destroy");
            PhotonNetwork.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gravitation == false)
        {
            StartCoroutine("delete");
            gravitation = true;
            ps.Play();
            //PhotonNetwork.Instantiate(gravityfield.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
            //PhotonNetwork.Destroy(gameObject);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }
    void FixedUpdate()
    {
        if (gravitation == true)
        {
            gravitate(force);
            rb.velocity = new Vector3(0, 0, 0);
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
                    //player.Damage(0.008f);
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * (speed * -1));
                }
            }
            TheBraaiiinnn Ki = obj.GetComponentInParent<TheBraaiiinnn>();
            if (Ki != null)
            {
                if (Ki.photonView.isMine)
                {
                    //player.Damage(0.008f);
                    Vector2 direction = obj.transform.position - transform.position;
                    obj.GetComponent<Rigidbody2D>().AddForce(direction * (speed * -1));
                }
            }
        }
    }
}
