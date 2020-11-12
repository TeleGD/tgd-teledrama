using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks, IConnectionCallbacks, IInRoomCallbacks
{
	public static GameManager instance;

	public enum Roles { Student, Hacker, Director };

	public bool exitIfNoMultiplayer = true;
	public Color[] playerColors;
	public Transform spawnPos;

	public bool isGameStarted = false;

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
			Invoke("SpawnPlayer", 1);

		transform.Find("Canvas/Master").gameObject.SetActive(PhotonNetwork.IsMasterClient);
	}

	//instancie le joueur dans le salon
	private void SpawnPlayer()
	{
		if(!isGameStarted)
			PhotonNetwork.Instantiate("Player", spawnPos.position + (Vector3)(Random.insideUnitCircle * 3), Quaternion.identity, 0);
	}

	public void MasterStartGameButton()
	{
		if (PlayerListManager.instance.playerList.Count < 3)
			return;

		photonView.RPC("StartGame", RpcTarget.AllBuffered);
		PlayerListManager.instance.AssignRoles();
		transform.Find("Canvas/Master").gameObject.SetActive(false);
	}

	[PunRPC]
	public void StartGame()
	{
		isGameStarted = true;
		if (PlayerController.me != null)
			PlayerController.me.transform.position = spawnPos.position + (Vector3)(Random.insideUnitCircle * 3);
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
