using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strechinngFix : MonoBehaviour
{
    public GameObject parent;
    private Vector3 offset;
    private bool begin = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("lol");
    }

    // Update is called once per frame
    void Update()
    {
        if (begin == true)
        {
            transform.position = parent.transform.position - offset;
        }
    }
    IEnumerator lol()
    {
        yield return new WaitForSeconds(1);
        offset = parent.transform.position - transform.position;
        begin = true;
    }
}
