using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;

//fait fonctionner le menu
public class LobbyManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks, IConnectionCallbacks
{
	public Transform canvas;
	private Transform loadingScreen;
	private Transform tabs;
	private bool isLoading;

	public Transform roomUi;
	private string roomName;

	private const string version = "0.6";

	void Start()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
		Cursor.lockState = CursorLockMode.None;

		InitUI();
		ConnectToPhoton();
	}

	#region Interface graphique
	//#########################
	//#  INTERFACE GRAPHIQUE  #
	//#########################

	//Initialise l'interface graphique
	private void InitUI()
	{
		loadingScreen = canvas.Find("Loading");
		tabs = canvas.Find("Panel/Tabs");

		if (PlayerPrefs.GetString("playerName", "Player") == "Player")
			PhotonNetwork.NickName = "Joueur" + Random.Range(1, 999);
		else
			PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
		roomName = "Salon " + Random.Range(1, 999);

		GameObject.Find("Player Name").GetComponent<InputField>().text = PhotonNetwork.NickName;
		GameObject.Find("Room Name").GetComponent<InputField>().text = roomName;
		GameObject.Find("Stats").GetComponent<Text>().text = PlayerPrefs.GetInt("playCount", 0) + " parties jouées";
		GameObject.Find("Version").GetComponent<Text>().text = "Version : " + version;
		GameObject.Find("Create Tab").SetActive(false);

		ChangeVolume(PlayerPrefs.GetInt("volume", 50) / 100f);
		GameObject.Find("Volume Slider").GetComponent<Slider>().value = PlayerPrefs.GetInt("volume", 50) / 100f;

		GameObject.Find("SystemInfo").GetComponent<Text>().text = "Le jeu tourne sur " + SystemInfo.graphicsDeviceName;
	}

	//permet de naviguer entre les différents onglets du menu
	public void SwitchTab(int tabId)
	{
		//empeche les debordements de liste
		if (tabId > tabs.childCount)
			return;

		for (var i = 0; i < tabs.childCount; i++)
		{
			tabs.GetChild(i).gameObject.SetActive(i == tabId);
		}
	}
	
	void SetLoadingScreen(string loadingText)
	{
		if (loadingText == "")  //Si le texte est vide, on ferme l'ecran de chargement
		{
			isLoading = false;
			loadingScreen.gameObject.SetActive(false);
		}
		else
		{
			isLoading = true;
			loadingScreen.gameObject.SetActive(true);
			loadingScreen.GetChild(0).GetComponent<Text>().text = loadingText;
		}
	}

	//anime l'écran de chargement
	private void Update()
	{
		if (!isLoading) return;
		loadingScreen.GetChild(1).eulerAngles += new Vector3(0, 0, Time.deltaTime * -300);
		loadingScreen.GetChild(2).eulerAngles += new Vector3(0, 0, Time.deltaTime * 200);
	}

	//Ces fonctions sont appelés quand on appuie sur les boutons dans l'interface

	public void CreateRoom()
	{
		ChangePlayerName(GameObject.Find("Player Name").GetComponent<InputField>().text);
		if(PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = 10 }, null))
			SetLoadingScreen("Création du salon...");
	}

	public void ConnectToRoomByName()
	{
		PhotonNetwork.JoinRoom(roomName);
		SetLoadingScreen("Connection au salon...");
	}

	public void ConnectToRoom(string targetName)
	{
		ChangePlayerName(GameObject.Find("Player Name").GetComponent<InputField>().text);
		PhotonNetwork.JoinRoom(targetName);
		SetLoadingScreen("Connection au salon...");
	}

	public void ChangePlayerName(string newName)
	{
		if (newName == "") return;

		PhotonNetwork.NickName = newName;
		PlayerPrefs.SetString("playerName", newName);
	}

	public void ChangeRoomName(string newName)
	{
		roomName = newName;
	}

	public void ChangeVolume(float newVolume)
	{
		AudioListener.volume = newVolume;
		GameObject.Find("Volume Slider/Label").GetComponent<Text>().text = "Volume : " + Mathf.CeilToInt(newVolume * 100) + "%";
		PlayerPrefs.SetInt("volume", Mathf.CeilToInt(newVolume * 100));
	}

	#endregion

	#region Photon
	//############
	//#  PHOTON  #
	//############

	private void ConnectToPhoton()
	{
		if(!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.GameVersion = "TeleDrama_" + version;
			PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "TeleDrama_" + version;
			PhotonNetwork.ConnectUsingSettings();
			SetLoadingScreen("Connexion au serveur...");
		}
		else
			PhotonNetwork.JoinLobby();
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		Debug.Log("OnRoomListUpdate : " + roomList.Count + " rooms");

		if (roomList.Count == 0) // Aucune room trouvee
		{
			roomUi.GetChild(0).gameObject.SetActive(true); //message de liste vide

			for (var i = 1; i < roomUi.childCount; i++)
			{
				roomUi.GetChild(i).gameObject.SetActive(false);
			}
		}
		else //une room ou plus trouvee
		{
			roomUi.GetChild(0).gameObject.SetActive(false); //message de liste vide

			for (var i = 1; i < roomUi.childCount; i++)
			{
				roomUi.GetChild(i).gameObject.SetActive(i < roomList.Count + 1);
			}

			byte childId = 1;
			foreach (var room in roomList)
			{
				roomUi.GetChild(childId).GetChild(0).GetComponent<Text>().text = room.Name;
				roomUi.GetChild(childId).GetChild(1).GetComponent<Text>().text = room.PlayerCount + " / " + room.MaxPlayers + " joueurs";
				roomUi.GetChild(childId).GetComponent<Button>().onClick.RemoveAllListeners();
				roomUi.GetChild(childId).GetComponent<Button>().onClick.AddListener(delegate { ConnectToRoom(room.Name); });
				childId++;
			}
		}
	}

	//Callbacks de Photon

	public override void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		PhotonNetwork.LoadLevel("Game");
	}

	public override void OnConnectedToMaster()
	{
		SetLoadingScreen("Connection au lobby...");
		PhotonNetwork.OfflineMode = false;
		PhotonNetwork.JoinLobby();
	}

	//connexion au master server terminée
	public override void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby");
		SetLoadingScreen("");
	}

	#endregion
}
