using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    private bool start = false;
    [SerializeField] private string VersionName = "0.1";
    [SerializeField] private TMP_InputField JoinGameInput;
    [SerializeField] private TMP_InputField UserNameInput;
    [SerializeField] private GameObject StartButton;
    private float MenuNumber = 1;
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject GamemodeCanvas;
    [SerializeField] private GameObject PickPowerCanvas;
    [SerializeField] private GameObject JoinServerCanvas;

    public static float power = 0;
    public static float gamemode = 0;
    private bool username = false;
    

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersionName);
    }
    public void Play()
    {
        start = true;
    }
    private void Update()
    {
        if (MenuNumber == 1)
        {
            if (start == true)
            {
                start = false;
                MenuNumber = 2;
            }
            MainMenuCanvas.SetActive(true);
            GamemodeCanvas.SetActive(false);
            PickPowerCanvas.SetActive(false);
            JoinServerCanvas.SetActive(false);
        }
        else if (MenuNumber == 2)
        {
            if (gamemode != 0 && start == true)
            {
                start = false;
                MenuNumber = 3;
            }
            MainMenuCanvas.SetActive(false);
            GamemodeCanvas.SetActive(true);
            PickPowerCanvas.SetActive(false);
            JoinServerCanvas.SetActive(false);
        }
        else if (MenuNumber == 3)
        {
            if (power != 0 && start == true)
            {
                start = false;
                MenuNumber = 4;
            }
            MainMenuCanvas.SetActive(false);
            GamemodeCanvas.SetActive(false);
            PickPowerCanvas.SetActive(true);
            JoinServerCanvas.SetActive(false);
        }
        else if (MenuNumber == 4)
        {
            MainMenuCanvas.SetActive(false);
            GamemodeCanvas.SetActive(false);
            PickPowerCanvas.SetActive(false);
            JoinServerCanvas.SetActive(true);
        }
    }
    public void Quit()
    {
        Application.Quit();
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
        PhotonNetwork.playerName = UserNameInput.text;
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
        else if (gamemode == 3)
            PhotonNetwork.LoadLevel("KI_Test");
        Debug.Log("joyning");
    }

}