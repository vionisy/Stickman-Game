using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject PlayerPreafab;
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    public float respwanTime = 5;

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
}
