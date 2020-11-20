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
}
