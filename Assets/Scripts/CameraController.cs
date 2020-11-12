using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float spectatorSpeed = 10; //vitesse en m/s

    private void Update()
    {
        if(PlayerController.me != null)
        {
            Transform target = PlayerController.me.transform;
            Vector3 targetPos = (target.position + (Vector3)PlayerController.me.GetVelocity() * 0.5f) + Vector3.back * 10;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3);
        }
        else
        {
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (dir.sqrMagnitude > 1)
                dir.Normalize(); //empeche la caméra de se déplacer plus vite en diagonale
            transform.position += (Vector3)dir * spectatorSpeed * Time.deltaTime;
        }
    }
}
