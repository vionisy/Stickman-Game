using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ItemPrefab;
    private float random;
    // Start is called before the first frame update
    void Start()
    {
        random = Random.Range(1, 2);
        if (random == 1)
        {
            Debug.Log("Spawn");
            PhotonNetwork.Instantiate(ItemPrefab.name, new Vector2(transform.position.x, transform.position.y), Quaternion.identity, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
