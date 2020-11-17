using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class HackerController : MonoBehaviour
{
    public float hackDelay = 15f;
    public float hackRange = 1f;
    public float timePressed;
    GameObject[] players;
    private Button hackButton;

    void Start()
    {
        hackButton = GameManager.instance.transform.Find("Canvas/HackButton").GetComponent<Button>();
        hackButton.gameObject.SetActive(true);
        hackButton.onClick.AddListener(Hack);
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    void Hack()
    {
        if(GetCountdown() < 0 && InRange())
        {
            ButtonCountdown();
            GameObject victime = GetNearestPlayer();
            PhotonView pv = victime.GetComponent<PhotonView>();
            pv.RPC("GetHacked", pv.Owner);
        }
    }


    void ButtonCountdown()
    {
        timePressed = Time.time;
    }

    float GetCountdown()
    {
        return hackDelay - Time.time + timePressed;
    }

    void Update()
    {
        hackButton.interactable = (InRange() && GetCountdown() < 0);
    }

    //verifie si un joueur est a porté du hacker
    bool InRange()
    {
        foreach(GameObject joueur in players)
        {
            if(Vector3.Distance(transform.position, joueur.transform.position) <= hackRange)
                return true;
        }
        return false;
    }

    GameObject GetNearestPlayer()
    {
        float minDist = 10000f;
        GameObject nearestPlayer = null;
        foreach(GameObject player in players)
        {
            if(player != gameObject)
            {
                float dist = Vector3.SqrMagnitude(transform.position - player.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestPlayer = player;
                }
            }
        }
        return nearestPlayer;
    }
}
