using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalWallScale : MonoBehaviour
{
    void Start()
    {
        UpdateWallScale();
    }

    /*
    void Update()
    {
        UpdateWallScale();
    }
    */

    private void UpdateWallScale()
    {
        if(transform.parent != null)
        {
            float scale = 1f / transform.parent.localScale.y;
            transform.localScale = new Vector3(1, scale, 1);
            transform.localPosition = Vector3.up * ((scale / 2f) - 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        if (transform.parent != null)
        {
            Gizmos.color = new Color(1, 0, 0, 0.2f);
            Gizmos.DrawCube(transform.parent.position + Vector3.down * ((transform.parent.localScale.y / 2f) + 0.5f), Vector3.one);

            Gizmos.color = new Color(0, 1, 1, 0.2f);
            Gizmos.DrawCube(transform.parent.position + Vector3.up * ((transform.parent.localScale.y / 2f) + 0.5f), Vector3.one);
        }
    }
}
