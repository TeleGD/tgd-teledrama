using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    public int gameIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.me.gameObject)
            GameManager.instance.GetComponent<MinigameManager>().StartGame(gameIndex);
    }

    private void OnDrawGizmos()
    {
        Collider2D coll = GetComponent<Collider2D>();
        if(coll != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.4f);
            Gizmos.DrawCube(coll.bounds.center, coll.bounds.extents * 2);
        }
        
    }
}
