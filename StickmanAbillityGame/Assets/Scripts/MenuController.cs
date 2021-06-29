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
    public static float gamemode = 0;
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
        if (power != 0 && gamemode != 0)
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
    public void setGamemode(float Gamemode)
    {
        gamemode = Gamemode;
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
        PhotonNetwork.JoinRandomRoom(null, 10, MatchmakingMode.FillRoom, new TypedLobby(gamemode.ToString(), default), null);
        
    }
    

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, new TypedLobby(gamemode.ToString(), default), null);
    }

    private void OnJoinedRoom()
    {
        if (gamemode == 2)
            PhotonNetwork.LoadLevel("MenuRoyale");
        else if (gamemode == 1)
            PhotonNetwork.LoadLevel("Map1");
        Debug.Log("joyning");
    }

}