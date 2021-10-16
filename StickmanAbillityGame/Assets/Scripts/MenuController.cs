using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public static float money;
    private bool start = false;
    private bool back = false;
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
    public static float selectedgamemode = 0;
    

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
            if (selectedgamemode != 0 && start == true)
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
    public void Back()
    {
        if (MenuNumber >= 1)
            MenuNumber -= 1;
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
        selectedgamemode = Gamemode;
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
        PhotonNetwork.JoinRandomRoom(null, 10, MatchmakingMode.FillRoom, new TypedLobby(selectedgamemode.ToString(), default), null);
        
    }
    

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, new TypedLobby(selectedgamemode.ToString(), default), null);
    }

    private void OnJoinedRoom()
    {
        if (selectedgamemode == 2)
            PhotonNetwork.LoadLevel("MenuRoyale");
        else if (selectedgamemode == 1)
            PhotonNetwork.LoadLevel("DuellMenu");
        else if (selectedgamemode == 3)
            PhotonNetwork.LoadLevel("Map1");
        else if (selectedgamemode == 4)
            PhotonNetwork.LoadLevel("Map2");
        else if (selectedgamemode == 5)
            PhotonNetwork.LoadLevel("Map3");
        else if (selectedgamemode == 6)
            PhotonNetwork.LoadLevel("Map4");
    }

}