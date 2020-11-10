using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks, IConnectionCallbacks, IInRoomCallbacks
{
	public static GameManager instance;

	public bool exitIfNoMultiplayer = true;
	public Color[] playerColors;
	public Transform spawnPos;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if(PhotonNetwork.CurrentRoom == null) //le joueur n'est pas connecté
		{
			if(exitIfNoMultiplayer)
				SceneManager.LoadScene("Menu");
		}
		else
			SpawnPlayer();
	}

	//instancie le joueur dans le salon
	private void SpawnPlayer()
	{
		PhotonNetwork.Instantiate("Player", spawnPos.position + (Vector3)(Random.insideUnitCircle * 3), Quaternion.identity, 0);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			PhotonNetwork.Disconnect();
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
	}

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom");

		SceneManager.LoadScene("Menu");
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("OnDisconnectedFromPhoton : " + cause);

		SceneManager.LoadScene("Menu");
	}

	public override void OnPlayerEnteredRoom(Player player)
	{
		Debug.Log("OnPlayerEnteredRoom: " + player);
	}

	public override void OnPlayerLeftRoom(Player player)
	{
		Debug.Log("OnPlayerLeftRoom: " + player);
	}

	//récupère la couleur demandée dans la liste et la transforme en vecteur
	public Vector3 GetVectorColor(int index)
	{
		if (index < 0)
			return Vector3.one * 0.5f; //gris

		index = index % playerColors.Length;
		Color c = playerColors[index];
		return new Vector3(c.r, c.g, c.b);
	}

	//affiche le ping à l'écran
	private void OnGUI()
	{
		GUI.Label(new Rect(4, 4, 150, 24), "Ping : " + PhotonNetwork.GetPing() + "ms");
	}
}
