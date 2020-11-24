using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDebug : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawCube(transform.position + Vector3.down * 0.5f, new Vector3(transform.localScale.x, 1));
        Gizmos.DrawCube(transform.position + Vector3.left * ((transform.localScale.x / 2f) + 0.5f) + Vector3.up * 0.5f, Vector3.one);
        Gizmos.DrawCube(transform.position + Vector3.right * ((transform.localScale.x / 2f) + 0.5f) + Vector3.up * 0.5f, Vector3.one);
    }
}
