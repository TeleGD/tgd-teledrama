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
}
