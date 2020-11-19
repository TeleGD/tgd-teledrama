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
    public float hackRange = 2f;
    private float timePressed;
    private GameObject[] players;
    private Button hackButton;

    private void Start()
    {
        hackButton = GameManager.instance.transform.Find("Canvas/HackButton").GetComponent<Button>();
        hackButton.gameObject.SetActive(true);
        hackButton.onClick.AddListener(Hack);
        players = GameObject.FindGameObjectsWithTag("Player");
        ButtonCountdown();
    }

    public void Hack()
    {
        if(GetCountdown() < 0)
        {
            GameObject victim = GetNearestPlayer();
            if(victim != null)
            {
                ButtonCountdown();
                PhotonView pv = victim.GetComponent<PhotonView>();
                pv.RPC("GetHacked", pv.Owner);
                hackButton.interactable = false;
                hackButton.GetComponent<Image>().color = Color.white;
            }
        }
    }


    private void ButtonCountdown()
    {
        timePressed = Time.time;
    }

    private float GetCountdown()
    {
        return hackDelay - Time.time + timePressed;
    }

    private void Update()
    {
        if(GetCountdown() < 0)
        {
            GameObject nearest = GetNearestPlayer();
            hackButton.interactable = (nearest != null);
            if (nearest != null)
            {
                int actorNumber = nearest.GetComponent<PhotonView>().Owner.ActorNumber;
                hackButton.GetComponent<Image>().color = PlayerListManager.instance.GetPlayerColor(actorNumber);
            }
            else
                hackButton.GetComponent<Image>().color = Color.white;

            hackButton.transform.GetChild(0).GetComponent<Text>().text = "HACK";
        }
        else
        {
            hackButton.transform.GetChild(0).GetComponent<Text>().text = "HACK\n" + Mathf.CeilToInt(GetCountdown()) + "s";
        } 
    }

    private GameObject GetNearestPlayer()
    {
        GameObject nearestPlayer = null;
        float minDist = hackRange * hackRange; //sqrt
        foreach (GameObject player in players)
        {
            if(player != null && player != gameObject && player.activeSelf)
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
