﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class NetworkedPlayer : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D body;
	private Vector2 targetPlayerPos;
	public TextMesh nameRenderer;
	public LayerMask visiblityMask;

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
			bool visible = true;
			if(PlayerController.me != null)
			{
				if (Physics2D.Linecast(transform.position + Vector3.down * 0.3f, PlayerController.me.transform.position + Vector3.down * 0.3f, visiblityMask))
					visible = false;
			}
			float scale = Mathf.Lerp(transform.localScale.x, visible ? 1f : 0f, Time.deltaTime * 8);
			scale = (scale < 0.1f && !visible) ? 0f : ((scale > 0.9f && visible) ? 1f : scale); 
			transform.localScale = Vector3.one * scale;
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
	public void GetHacked()
	{
		if(GameManager.instance.GetMyRole() == GameManager.Role.Student)
			Invoke("ShowHack", Random.Range(10, 20));
	}

	private void ShowHack()
	{
		GameManager.instance.transform.Find("Canvas/Hacked").gameObject.SetActive(true);
		PlayerListManager.instance.SyncHackedStatus(true);
	}

	[PunRPC]
	public void KickPlayer()
	{
		if (PlayerController.me != null && PlayerController.me.gameObject == gameObject)
			PlayerController.me = null;
		gameObject.SetActive(false);
	}
}
