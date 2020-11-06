using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkedPlayer : MonoBehaviourPun, IPunObservable
{
    private Rigidbody2D body;
	private Vector2 targetPlayerPos;

    private void Start()
    {
        if(photonView.IsMine)
        {
            GetComponent<PlayerController>().enabled = true;
        }

        gameObject.name = photonView.Owner.NickName;
        body = GetComponent<Rigidbody2D>();
		targetPlayerPos = transform.position;
		transform.Find("Name").GetComponent<TextMesh>().text = photonView.Owner.NickName;
	}

	void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.IsWriting)
		{
			//Mon joueur : on envoie les donnees
			stream.SendNext((Vector2)transform.position);
			if (body != null)
				stream.SendNext(body.velocity);
		}
		else
		{
			//Autre joueur : on recoit les donnees
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
}
