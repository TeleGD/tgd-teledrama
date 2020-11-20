using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public RectTransform contentHolder;
    public GameObject[] games;

    public void StartGame(int index)
    {
        contentHolder.parent.gameObject.SetActive(true);
        Instantiate(games[index], contentHolder);
    }

    public void CloseGame()
    {
        if(contentHolder.childCount > 0)
            Destroy(contentHolder.GetChild(0).gameObject);
        contentHolder.parent.gameObject.SetActive(false);
    }
}
