using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravityfield : MonoBehaviour
{
    public float fieldofImpact;
    public float force;
    public LayerMask LayerToHit;
    // Start is called before the first frame update
    private IEnumerator delete()
    {
        yield return new WaitForSeconds(2.5f);
        PhotonNetwork.Destroy(gameObject);
    }
    void Start()
    {
        StartCoroutine("delete");
    }
    public void gravitate(float speed)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldofImpact, LayerToHit);
        foreach (Collider2D obj in objects)
        {
            if (obj.GetComponentInParent<PlayerController>())
                obj.GetComponentInParent<PlayerController>().Damage2(0.02f);
            Vector2 direction = obj.transform.position - transform.position;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * (speed * -1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        gravitate(force);
    }
}
