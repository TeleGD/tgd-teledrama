using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks, IConnectionCallbacks, IInRoomCallbacks
{
	public bool exitIfNoMultiplayer = true;
	public Transform spawnPos;

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

	private void SpawnPlayer()
	{
		PhotonNetwork.Instantiate("Player", spawnPos.position + (Vector3)(Random.insideUnitCircle * 3), Quaternion.identity, 0);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Menu");
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

	private void OnGUI()
	{
		GUI.Label(new Rect(4, 4, 150, 24), "Ping : " + PhotonNetwork.GetPing() + "ms");
	}
}
