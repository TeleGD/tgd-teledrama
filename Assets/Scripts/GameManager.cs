using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks
{
	public GameObject testFace;

	public override void OnLeftRoom()
	{
		Debug.Log("OnLeftRoom (local)");

		// back to main menu
		SceneManager.LoadScene("Menu");
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("OnDisconnectedFromPhoton");

		// back to main menu
		SceneManager.LoadScene("Menu");
	}

	public void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		Debug.Log("OnPhotonInstantiate " + info.Sender);
	}

	public void OnPhotonPlayerConnected(Player player)
	{
		GameObject go = Instantiate(testFace);
		testFace.transform.GetChild(0).GetComponent<TextMesh>().text = player.NickName;
		Debug.Log("OnPhotonPlayerConnected: " + player);
	}

	public void OnPhotonPlayerDisconnected(Player player)
	{
		Debug.Log("OnPlayerDisconneced: " + player);
	}

	public void OnFailedToConnectToPhoton()
	{
		Debug.Log("OnFailedToConnectToPhoton");

		// back to main menu
		SceneManager.LoadScene("Menu");
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(4, 4, 150, 24), "Ping : " + PhotonNetwork.GetPing() + "ms");
	}
}
