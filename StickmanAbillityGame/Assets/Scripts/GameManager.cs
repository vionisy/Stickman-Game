using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool CameraFollow = true;
    public Vector3 spawnposition = new Vector3(-599.2f, 11, 0);
    public GameObject HandyCanvas;
    public GameObject PlayerPreafab;
    public GameObject KI;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public GameObject MenuScreen;
    public float respwanTime = 5;
    public static bool HandyControllsOn = false;
    static public bool E_pressed;
    static public bool Q_pressed;
    public bool KITest = false;
    public static float playernumber;
    public PhotonView photonView;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && KITest == true)
            //StartCoroutine("spawnKI");
        if (Input.GetKeyDown(KeyCode.Escape) && MenuScreen.active == false)
            MenuScreen.SetActive(true);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            GameObject Head = player.GetComponentInChildren<CircleCollider2D>().gameObject;
            if (player.GetComponent<PlayerController>().ownplayernumber == playernumber && CameraFollow == true)
            {
                SceneCamera.transform.position = new Vector3(Head.transform.position.x, Head.transform.position.y, -10);
            }
        }
    }
    public void SetSpawnpoint(Vector3 posi)
    {
        spawnposition = posi;
    }
    private void Awake()
    {
        GameCanvas.SetActive(true);
    }
    private void Start()
    {
        playernumber = PhotonNetwork.playerList.Length;
    }
    [PunRPC]
    public void InstansiatePlayer()
    {
        if (playernumber == 1)
        {
            float randomValue = Random.Range(-20f, 20f);
            PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(spawnposition.x + randomValue, spawnposition.y + 15), Quaternion.identity, 0);
        }
    }
    public void SpawnPlayer()
    {
        photonView.RPC("InstansiatePlayer", PhotonTargets.All);   
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(true);
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respwanTime);
        float randomValue = Random.Range(-20f, 20f);
        PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(spawnposition.x + randomValue, spawnposition.y + 15), Quaternion.identity, 0);
    }
    public IEnumerator spawnKI()
    {
        yield return new WaitForSeconds(0.3f);
        float randomValue = Random.Range(-10f, 10f);
        PhotonNetwork.Instantiate(KI.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y + 15), Quaternion.identity, 0);
    }
    public void reset()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject thePlayer in player)
        {
            if (thePlayer.GetComponent<PhotonView>().isMine)
            {
                thePlayer.GetComponent<PlayerController>().Damage(100000);
            }
        }
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
