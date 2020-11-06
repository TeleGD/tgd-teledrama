using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void Update()
    {
        if(PlayerController.me != null)
        {
            Transform target = PlayerController.me.transform;
            Vector3 targetPos = (target.position + (Vector3)PlayerController.me.GetVelocity() * 0.5f) + Vector3.back * 10;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3);
        }
    }
}
