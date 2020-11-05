using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IMatchmakingCallbacks, IConnectionCallbacks, IInRoomCallbacks
{
	public GameObject testFace;

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

		//normalement il faut utiliser photoninstantiate pour que ça soit synchro chez tout le monde
		GameObject go = Instantiate(testFace, new Vector3(Random.Range(-5, 5), Random.Range(-3, 3), 0), Quaternion.identity);
		go.transform.GetChild(0).GetComponent<TextMesh>().text = player.NickName;
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
