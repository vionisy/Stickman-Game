using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowdownFactor;
    public KeyCode key;
    private bool Ok;
    public void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (Ok == true)
            {
                Ok = false;
            }
            else
            {
                Ok = true;
            }
        }
        if (Ok == true)
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        else
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }

    }

}
