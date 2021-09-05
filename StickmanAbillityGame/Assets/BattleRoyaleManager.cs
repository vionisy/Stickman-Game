using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class BattleRoyaleManager : MonoBehaviour
{
    public bool battleRoyale = true;
    private bool foundOne = false;
    private Vector3 SpawnPosition;
    public Transform[] Spawnpoints;
    public GameObject HandyCanvas;
    public GameObject PlayerPreafab;
    public GameObject SceneCamera;
    public Transform Cameraposition;
    public GameObject MenuScreen;
    public float respwanTime = 5;
    public static bool HandyControllsOn = false;
    static public bool E_pressed;
    static public bool Q_pressed;
    public PhotonView photonView;
    public Canvas deadScreen;
    
    PhotonPlayer[] allPlayers;
    int myNumberInRoom;
    private void Update()
    {
        if (PhotonNetwork.playerList.Length == 1)
        {
            //PhotonNetwork.LoadLevel("YouWon");
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            GameObject Head = player.GetComponentInChildren<CircleCollider2D>().gameObject;
            if (player.GetComponent<PhotonView>() && player.GetComponent<PhotonView>().isMine)
                SceneCamera.transform.position = new Vector3(Head.transform.position.x, Head.transform.position.y, -10);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && MenuScreen.active == false)
            MenuScreen.SetActive(true);
    }
    private void Awake()
    {
        if (battleRoyale == true)
            MenuController.power = 0;
        PhotonNetwork.room.IsOpen = false;
        SpawnPlayer();
    }
    public void dead_screen()
    {
        deadScreen.enabled = true;
    }
    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-20f, 20f);
        PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(Spawnpoints[MenuManager.ownPlayerNumber - 1].transform.position.x + randomValue, Spawnpoints[MenuManager.ownPlayerNumber - 1].transform.position.y), Quaternion.identity, 0);
        Debug.Log(MenuManager.ownPlayerNumber);
        SceneCamera.SetActive(true);
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respwanTime);
        float randomValue = Random.Range(-150f, 500f);
        PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(this.transform.position.x + randomValue, this.transform.position.y), Quaternion.identity, 0);
    }
    public void reset()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject thePlayer in player)
        {
            if (thePlayer.GetComponent<PhotonView>().isMine)
            {
                thePlayer.GetComponent<PlayerController>().delete();
            }
        }
        float randomValue = Random.Range(-1f, 1f);
        PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
    }
    public void OpenMenu()
    {
        MenuScreen.SetActive(true);
    }
    public void CloseMenu()
    {
        MenuScreen.SetActive(false);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }
    public void HandyControlls()
    {
        HandyControllsOn = true;
        HandyCanvas.SetActive(true);
    }
    public void ComputerControlls()
    {
        HandyControllsOn = false;
        HandyCanvas.SetActive(false);
    }
    public void Epressed()
    {
        E_pressed = true;
    }
    public void Qpressed()
    {
        Q_pressed = true;
    }
}
