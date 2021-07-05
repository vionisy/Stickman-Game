using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && KITest == true)
            StartCoroutine("spawnKI");
        if (Input.GetKeyDown(KeyCode.Escape) && MenuScreen.active == false)
            MenuScreen.SetActive(true);
    }
    private void Awake()
    {
        GameCanvas.SetActive(true);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-1f, 1f);
        PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
        GameCanvas.SetActive(false);
        SceneCamera.SetActive(true);
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respwanTime);
        float randomValue = Random.Range(-10f, 10f);
        PhotonNetwork.Instantiate(PlayerPreafab.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
    }
    public IEnumerator spawnKI()
    {
        yield return new WaitForSeconds(0.3f);
        float randomValue = Random.Range(-10f, 10f);
        PhotonNetwork.Instantiate(KI.name, new Vector2(this.transform.position.x * randomValue, this.transform.position.y), Quaternion.identity, 0);
    }
    public void reset()
    {
        GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject thePlayer in player)
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
