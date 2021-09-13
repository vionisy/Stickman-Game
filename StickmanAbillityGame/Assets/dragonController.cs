using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragonController : MonoBehaviour
{
    public VulcanoDamage firedamage;
    public ParticleSystem fire;
    public GameObject target;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public int state = 2;
    public bool fireEnabled;
    public float speed = 40;
    void Start()
    {
        StartCoroutine("StateController");
    }

    // Update is called once per frame
    void Update()
    {
        if (fireEnabled == true)
        {
            firedamage.enabled = true;
            fire.Play();
        }
        else
        {
            firedamage.enabled = false;
            fire.Stop();
        }
        if (state == 1)
        {
            target.SetActive(true);
            target.transform.position = FindClosestEnemy().transform.position;
            if (transform.position.y > FindClosestEnemy().transform.position.y + 10)
            {
                rb.AddForce(Vector2.up * -10 * Time.deltaTime);
            }
            else if (transform.position.y < FindClosestEnemy().transform.position.y + 10)
            {
                rb.AddForce(Vector2.up * 10 * Time.deltaTime);
            }
            else
            {
                if (rb.velocity.y > 0)
                    rb.velocity -= new Vector2(0, 0.3f);
                else if (rb.velocity.y < 0)
                    rb.velocity += new Vector2(0, 0.3f);
            }
            if (Vector2.Distance(FindClosestEnemy().transform.position, transform.position) >= 20)
            {
                if (FindClosestEnemy().transform.position.x >= transform.position.x)
                {
                    rb.AddForce(Vector2.right * speed * Time.deltaTime);
                }
                else if (FindClosestEnemy().transform.position.x <= transform.position.x)
                {

                    rb.AddForce(Vector2.right * -speed * Time.deltaTime);
                }
            }
            else
            {
                if (rb.velocity.x > 0)
                    rb.velocity -= new Vector2(0.3f, 0);
                else if (rb.velocity.x < 0)
                    rb.velocity += new Vector2(0.3f, 0);
            }
            if (FindClosestEnemy().transform.position.x >= transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (FindClosestEnemy().transform.position.x <= transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else if (state == 2)
        {
            target.SetActive(false);
            if (transform.position.y > FindClosestEnemy().transform.position.y + 18)
            {
                rb.AddForce(Vector2.up * -10 * Time.deltaTime);
            }
            else if (transform.position.y < FindClosestEnemy().transform.position.y + 18)
            {
                rb.AddForce(Vector2.up * 10 * Time.deltaTime);
            }
            else
            {
                if (rb.velocity.y > 0)
                    rb.velocity -= new Vector2(0, 0.3f);
                else if (rb.velocity.y < 0)
                    rb.velocity += new Vector2(0, 0.3f);
            }
            if (FindClosestEnemy().transform.position.x >= transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (FindClosestEnemy().transform.position.x <= transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (Vector2.Distance(FindClosestEnemy().transform.position, transform.position) >= 30)
            {
                if (FindClosestEnemy().transform.position.x >= transform.position.x)
                {
                    rb.AddForce(Vector2.right * 50 * Time.deltaTime);
                }
                else if (FindClosestEnemy().transform.position.x <= transform.position.x)
                {

                    rb.AddForce(Vector2.right * -50 * Time.deltaTime);
                }
            }
        }
    }
    private IEnumerator StateController()
    {
        state = 2;
        yield return new WaitForSeconds(Random.Range(8, 15));
        state = 1;
        yield return new WaitForSeconds(Random.Range(5, 7));
        fireEnabled = true;
        yield return new WaitForSeconds(Random.Range(3, 7));
        fireEnabled = false;
        float randomness = Random.Range(1, 2);
        if (randomness == 2)
        {
            fireEnabled = true;
            yield return new WaitForSeconds(Random.Range(2, 5));
            fireEnabled = false;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine("StateController");
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Head");
        if (gos != null)
        {
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in gos)
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = go;
                    distance = curDistance;
                }
            }
            return closest;
        }
        else
            return null;
    }
}
