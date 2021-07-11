using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textureSync : MonoBehaviour
{
    public Transform Parent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            transform.position = Parent.position;
            transform.rotation = Parent.rotation;
            transform.localScale = Parent.localScale;
        }
    }
}
