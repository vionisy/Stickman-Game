using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulcanospawner : MonoBehaviour
{
    public GameObject Vulcano;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (GameManager.playernumber == 1)
        {
            Debug.Log("Vulcano");
            PhotonNetwork.Instantiate(Vulcano.name, transform.position, Quaternion.identity, 0);       
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
}
