using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public bool countdownstarted = false;
    public Text playernumber;
    public Text TimeUntillStart;
    private bool starting = false;
    public float minplayers;
    public float maxplayers;
    static public int ownPlayerNumber;
    // Start is called before the first frame update
    void Start()
    {
        ownPlayerNumber = PhotonNetwork.playerList.Length;
        PhotonNetwork.room.IsOpen = true;
        Debug.Log("joyned");
    }

    // Update is called once per frame
    void Update()
    {

        playernumber.text = "Players: " + PhotonNetwork.playerList.Length.ToString() + "/" + maxplayers.ToString();
        if (PhotonNetwork.playerList.Length >= minplayers && starting == false && countdownstarted == false)
        {
            StartCoroutine("StartGame");
            starting = true;
        }
        if (PhotonNetwork.playerList.Length >= maxplayers)
        {
            PhotonNetwork.LoadLevel("BattleRoyale");
        }
    }
    private IEnumerator StartGame()
    {
        if (countdownstarted == false)
        {
            Debug.Log("Start1");
            yield return new WaitForSeconds(1);
            TimeUntillStart.enabled = true;
            GetComponent<PhotonView>().RPC("changeCountdown", PhotonTargets.All, 5f);
            yield return new WaitForSeconds(1);
            GetComponent<PhotonView>().RPC("changeCountdown", PhotonTargets.All, 4f);
            yield return new WaitForSeconds(1);
            GetComponent<PhotonView>().RPC("changeCountdown", PhotonTargets.All, 3f);
            yield return new WaitForSeconds(1);
            GetComponent<PhotonView>().RPC("changeCountdown", PhotonTargets.All, 2f);
            yield return new WaitForSeconds(1);
            GetComponent<PhotonView>().RPC("changeCountdown", PhotonTargets.All, 1f);
            yield return new WaitForSeconds(1);
            GetComponent<PhotonView>().RPC("changeCountdown", PhotonTargets.All, 0f);

        }
    }

    [PunRPC]
    public void changeCountdown(float countdown)
    {
        TimeUntillStart.text = countdown.ToString();
        countdownstarted = true;
        if (countdown == 0)
            PhotonNetwork.LoadLevel("BattleRoyale");
    }
}
