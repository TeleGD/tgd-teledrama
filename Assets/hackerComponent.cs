﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hackerComponent : MonoBehaviour
{
    private GameObject button;
    public float hackDelay = 15f;
    public float hackRange = 1f;
    public float timePressed;
    GameObject[] joueurs = GameObject.FindGameObjectsWithTag("Player");
    // Start is called before the first frame update

    void Start()
    {
        button = GameObject.Find("Canvas").transform.Find("HackButton").gameObject;
        button.SetActive(true);
    }


    void disableButton(){
        button.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    void enableButton(){
        button.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    void buttonCountdown(){
        disableButton();
        timePressed = Time.time;
        Invoke("enableButton", hackDelay);
    }

    float getCountdown(){
        return hackDelay - Time.time + timePressed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(getCountdown());
        if(Input.GetKeyDown(KeyCode.O)){
            buttonCountdown();
        }
    }

    //verifie si un joueur est a porté du hacker
    bool inRange(){
        foreach(GameObject joueur in joueurs){
            if(Vector3.Distance(this.transform.position, joueur.transform.position) <= hackRange) return true;
        }
        return false;
    }

    GameObject getClosestPlayer(){
        float minDist = -1f;
        GameObject closest = null;
        foreach(GameObject joueur in joueurs){
            float dist = Vector3.Distance(this.transform.position, joueur.transform.position);
            if( dist <= minDist || minDist < 0) {
                minDist = dist;
                closest = joueur;
            }
        }
        return closest;
    }

}
