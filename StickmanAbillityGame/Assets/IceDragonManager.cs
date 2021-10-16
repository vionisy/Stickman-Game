using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDragonManager : MonoBehaviour
{
    public GameObject iceBallPrefab;
    public float speed;
    public Rigidbody2D rb;
    public Animator anim;
    private float Followingoffset = 35;
    public int state = 1;
    public GameObject ShootingPoint;
    public bool collided = false;
    private bool startedCurutene = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StateChanger");
    }
    // Update is called once per frame
    void Update()
    {
        if (rb.transform.position.x - FindClosestEnemy().transform.position.x <= 7 && collided == true) 
        {
            rb.AddForce(Vector2.right * speed * Time.deltaTime * 2);
            anim.Play("IceDragonTailAttack");
            if (startedCurutene == false)
                startedCurutene = true;
        }
        else if (FindClosestEnemy().transform.position.x <= rb.transform.position.x - Followingoffset)
        {
            rb.AddForce(Vector2.left * speed * Time.deltaTime);
            anim.Play("IceDragonWalkLeft");
            //rb.transform.rotation = Quaternion.Euler(0, 0, 0);
            //transform.eulerAngles = new Vector3(
            //transform.eulerAngles.x,
            //0,
            //transform.eulerAngles.z);
        }
        else if (FindClosestEnemy().transform.position.x >= rb.transform.position.x - Followingoffset + 10)
        {
            anim.Play("IceDragonWalkLeft");
            rb.AddForce(Vector2.right * speed * Time.deltaTime);
            //transform.eulerAngles = new Vector3(
            //transform.eulerAngles.x,
            //180,
            //transform.eulerAngles.z);
        }
        else
        {
            Debug.Log("stop");
        }
        if (state == 1)
        {
            Followingoffset = 35;
        }
        else if (state == 2)
        {
            Followingoffset = 0;
        }
        //transform.position.x = new Vector3(Quaternion.identity, rb.transform.position.x, Quaternion.identity);
        //transform.position = rb.transform.position;
        
    }
    private IEnumerator WaitForcollided()
    {
        yield return new WaitForSeconds(2.3f);
        collided = false;
    }
    private IEnumerator StateChanger()
    {        
        if (state == 1)
        {
            PhotonNetwork.Instantiate(iceBallPrefab.name, ShootingPoint.transform.position, ShootingPoint.transform.rotation, 0);
            yield return new WaitForSeconds(Random.Range(5, 10));
            PhotonNetwork.Instantiate(iceBallPrefab.name, ShootingPoint.transform.position, ShootingPoint.transform.rotation, 0);
            yield return new WaitForSeconds(Random.Range(5, 10));
            PhotonNetwork.Instantiate(iceBallPrefab.name, ShootingPoint.transform.position, ShootingPoint.transform.rotation, 0);
            yield return new WaitForSeconds(Random.Range(5, 10));
            PhotonNetwork.Instantiate(iceBallPrefab.name, ShootingPoint.transform.position, ShootingPoint.transform.rotation, 0);
            state = 2;
        }
        else if (state == 2)
        {
            yield return new WaitForSeconds(Random.Range(10, 15));
            state = 1;
        }
        StartCoroutine("StateChanger");
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
