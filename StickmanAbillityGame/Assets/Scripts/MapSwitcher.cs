using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MapSwitcher : MonoBehaviour
{

    public string SceneLoading;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("p"))
        {

            SceneManager.LoadScene(SceneLoading);

        }
    }
}
