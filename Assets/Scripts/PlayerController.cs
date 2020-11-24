using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController me;

    public float speed = 4; //vitesse en m/s

    private bool isMine;
    private Rigidbody2D body;
    private Animator anim;
    private float lastDirX = 1;

    //initalise le joueur une fois que le network player est fonctionnel
    public void InitPlayer(bool isMe)
    {
        if (isMe)
            me = this; //référence unique vers notre joueur

        this.enabled = true;
        isMine = isMe;

        body = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        bool moving = body.velocity.sqrMagnitude > 0.2f;
        anim.SetBool("moving", moving);

        if(Mathf.Abs(body.velocity.x) > 0.2f)
        {
            float dirSign = Mathf.Sign(body.velocity.x);
            if (dirSign != lastDirX)
                transform.GetChild(0).localScale = new Vector3(dirSign, 1, 1);
            lastDirX = dirSign;
        }
    }

    private void FixedUpdate()
    {
        if (!isMine) //si ce joueur n'est pas le notre, on ne contrôle pas sa direction
            return;

        if(MinigameManager.isGameRunning)
        {
            body.velocity = Vector2.zero;
            return;
        }    

        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (dir.sqrMagnitude > 1)
            dir.Normalize(); //empeche le joueur de se déplacer plus vite en diagonale
        dir *= speed;

        body.velocity = dir;
    }

    public Vector2 GetVelocity()
    {
        return body.velocity;
    }
}
