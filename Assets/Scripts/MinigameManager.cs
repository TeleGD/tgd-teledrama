﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    public GameObject miniGamesRoot;
    public RectTransform contentHolder;
    public GameObject[] games;
    public static bool isGameRunning = false;

    private void Start()
    {
        instance = this;
        isGameRunning = false;
    }

    public void StartGame(int index)
    {
        miniGamesRoot.SetActive(true);
        isGameRunning = true;
        Instantiate(games[index], contentHolder);
    }

    public void CloseGame()
    {
        if(contentHolder.childCount > 0)
            Destroy(contentHolder.GetChild(0).gameObject);
        miniGamesRoot.SetActive(false);
        isGameRunning = false;
    }

    public void WinGame()
    {
        Debug.Log("Jeu gagné !");
        CloseGame();
    }
}
