using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnpoint : MonoBehaviour
{
    public bool isUsed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawn()
    {
        GetComponent<PhotonView>().RPC("spawn2", PhotonTargets.AllBuffered);
    }
    [PunRPC]
    public void spawn2()
    {
        isUsed = true;
    }
}
