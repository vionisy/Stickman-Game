using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDragonFight : MonoBehaviour
{
    public GameObject cameraa;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.playernumber == 1 && GameObject.FindGameObjectWithTag("Dragon"))
        {
            GameObject.FindGameObjectWithTag("Dragon").SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        cameraa.SetActive(true);
        Destroy(gameObject);
    }
}
