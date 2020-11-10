using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController me;

    public float speed = 4; //vitesse en m/s

    private Rigidbody2D body;
    private Animator anim;
    private float lastDirX = 1;

    private void Start()
    {
        me = this;
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
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        dir *= speed;

        body.velocity = dir;
    }

    public Vector2 GetVelocity()
    {
        return body.velocity;
    }
}
