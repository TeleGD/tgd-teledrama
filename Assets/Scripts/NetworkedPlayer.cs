using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkedPlayer : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D body;
	private Vector2 targetPlayerPos;
	public TextMesh nameRenderer;

    private void Start()
    {
		//initalise le controlleur du joueur en indiquant si ce joueur nous appartient
		GetComponent<PlayerController>().InitPlayer(photonView.IsMine);
		if (photonView.IsMine)
			PlayerListManager.instance.photonView.RPC("AddPlayer", RpcTarget.MasterClient, photonView.ViewID);

        gameObject.name = photonView.Owner.NickName; //nom du joueur dans l'éditeur

        body = GetComponent<Rigidbody2D>();
		targetPlayerPos = transform.position;

		//affichage du nom du joueur
		nameRenderer.text = photonView.Owner.NickName;
		nameRenderer.transform.parent.GetChild(1).localScale = nameRenderer.GetComponent<Renderer>().bounds.size;
	}


	//synchronise la position et direction du joueur 10 fois par seconde
	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting) //Mon joueur : on envoie les donnees
		{
			stream.SendNext((Vector2)transform.position);
			if (body != null)
				stream.SendNext(body.velocity);
		}
		else //Autre joueur : on recoit les donnees
		{
			targetPlayerPos = (Vector2)stream.ReceiveNext();
			if (body != null)
				body.velocity = (Vector2)stream.ReceiveNext();
		}
	}

	private void Update()
	{
		//interpolation des autres joueurs
		if (!photonView.IsMine && body != null)
		{
			transform.position = Vector2.Lerp(transform.position, targetPlayerPos + body.velocity * 0.2f, Time.deltaTime * 5);
		}
	}

	[PunRPC]
	public void SetPlayerColor(Vector3 color)
	{
		transform.Find("Model/Body").GetComponent<Renderer>().material.color = new Color(color.x, color.y, color.z);
	}

	[PunRPC]
	public void SetRole(int role)
	{
		GameManager.instance.SetMyRole((GameManager.Role)role);
		
		if (role == (int)GameManager.Role.Hacker)
			gameObject.AddComponent<HackerController>();

		PlayerListManager.instance.UpdatePlayerListUI();
	}

	[PunRPC]
	public void GetHacked(){
		Debug.Log("LMAOOOOO tu t'es fait pirater");
	}

	[PunRPC]
	public void KickPlayer()
	{
		if (PlayerController.me != null && PlayerController.me.gameObject == gameObject)
			PlayerController.me = null;
		gameObject.SetActive(false);
	}
}
