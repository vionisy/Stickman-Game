using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSpawner : MonoBehaviour
{
    public GameObject Dragon;
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (FindClosestEnemy() && Vector2.Distance(FindClosestEnemy().transform.position, transform.position) <= 70)
        {
            if (GameManager.playernumber == 1)
            {
                PhotonNetwork.Instantiate(Dragon.name, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity, 0);
            }
            Destroy(gameObject);
        }
    }
}
