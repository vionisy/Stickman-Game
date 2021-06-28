using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuController : MonoBehaviour
{
    [SerializeField] private string VersionName = "0.1";
    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;

    [SerializeField] private GameObject StartButton;

    [SerializeField] private InputField UsernameInput;
    [SerializeField] private InputField JoinGameInput;

    public static float power = 0;

    private bool username = false;
    

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersionName);
    }

    private void Start()
    {
        UsernameMenu.SetActive(true);
    }
    private void Update()
    {
        if (power != 0)
        {
            if (UsernameInput.text.Length >= 1)
            {
                StartButton.SetActive(true);
            }
            else
            {
                StartButton.SetActive(false);
            }
        }
      
    }

    
    public void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }
    public void setPower(float Power)
    {
        power = Power;
    }

    public void Close()
    {
        Application.Quit();
    }

    public void SetUserName()
    {
        UsernameMenu.SetActive(false);
        PhotonNetwork.playerName = UsernameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Map1");
    }

}