using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPreafab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public GameObject MenuScreen;
    public float respwanTime = 5;
    private void Update()
    {
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
        Debug.Log("Respawn");
        yield return new WaitForSeconds(respwanTime);
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
}
