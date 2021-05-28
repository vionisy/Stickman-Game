using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage : MonoBehaviour
{
    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    public KeyCode mousebutton;
    private float speed;
    private Vector3 oldPosition;
    public PhotonView photonView;
    private StressReceiver camerashake;
    private void Start()
    {
        camerashake = FindObjectOfType<Camera>().GetComponent<StressReceiver>();
    }
    void FixedUpdate()
    {
        speed = Vector3.Distance(oldPosition, transform.position) * 100f;
        oldPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (speed >= 10)
        {
            if (Input.GetKey(mousebutton)) //&& collision.gameObject.tag == "Head" || collision.gameObject.tag == "Chest")
            {
                speed -= 10;
                if (speed >= 35)
                {
                    Debug.Log("CameraShake");
                    StartCoroutine("CameraShake");
                    speed += 10;
                }
                collision.gameObject.GetComponentInParent<PlayerController>().Damage(speed * 0.4f);
                photonView.RPC("knockback", PhotonTargets.Others, speed);
            }
        }
    }
    private IEnumerator CameraShake()
    {
        camerashake.InduceStress(1);
        yield return new WaitForSeconds(0.1f);
        camerashake.InduceStress(0);
    }
    [PunRPC]
    public void knockback(float speed)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayerToHit);
        foreach (Collider2D obj in objects)
        {
            Debug.Log("Knockback");
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * (speed * 4));
        }
    }
}