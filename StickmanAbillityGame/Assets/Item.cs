using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private int UpgradeType;
    CircleCollider2D playerEntered;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerEntered = FindClosestEnemy().gameObject.GetComponentInParent<PlayerController>().GetComponentInChildren<CircleCollider2D>();
        if (Vector2.Distance(playerEntered.transform.position, transform.position) <= 10)
        {
            transform.position += ((playerEntered.transform.position - transform.position) * 0.034f);
        }
        if (Vector2.Distance(playerEntered.transform.position, transform.position) <= 1f)
        {
            UpgradeType = Random.Range(1, 4);
            if (UpgradeType == 1)
                playerEntered.GetComponentInParent<PlayerController>().speedBoostLevelUp();
            else if (UpgradeType == 2)
                playerEntered.GetComponentInParent<PlayerController>().jumpBoostLevelUp();
            else if (UpgradeType == 3)
                playerEntered.GetComponentInParent<PlayerController>().strengthBoostLevelUp();
            else if (UpgradeType == 4)
                playerEntered.GetComponentInParent<PlayerController>().healthBoostLevelUp();
            PhotonNetwork.Destroy(gameObject);
        }
    }
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Chest");
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
