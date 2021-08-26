using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Manages The Situation that you won a game
public class YouWon : MonoBehaviour
{
    public float prize;
    public void Continue()
    {
        PhotonNetwork.LoadLevel("MainMenu");
    }
    // Start is called before the first frame update
    void Start()
    {
        MenuController.money += prize;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
