using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scoremanager : MonoBehaviour
{
    public TextMeshProUGUI ScoreTextmesh;
    private float currentscore = 0;
    // Start is called before the first frame update
    void Start()
    {
        ScoreTextmesh.text = "Score: " + currentscore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addScore(float value)
    {
        currentscore += value;
        ScoreTextmesh.text = "Score: " + currentscore.ToString();
    }
}
