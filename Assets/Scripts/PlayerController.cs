using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController me;

    public float speed = 4; //vitesse en m/s

    private Rigidbody2D body;

    private void Start()
    {
        me = this;
        body = GetComponent<Rigidbody2D>();
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
