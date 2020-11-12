using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;

public class HackerController : MonoBehaviour
{
    private GameObject button;
    public float hackDelay = 15f;
    public float hackRange = 1f;
    public float timePressed;
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
    private UnityEngine.UI.Button buttonController;
    // Start is called before the first frame update

    void Start()
    {
        button = GameManager.instance.transform.Find("Canvas/HackButton").gameObject;
        button.SetActive(true);
        buttonController = button.GetComponent<UnityEngine.UI.Button>();
        buttonController.onClick.AddListener(Hack);
    }

    void Hack(){
        if(GetCountdown() < 0 && InRange()){
            ButtonCountdown();
            GameObject victime = GetClosestPlayer();
            PhotonView pv = victime.GetComponent<PhotonView>();
            pv.RPC("GetHacked", pv.Owner);
        }
    }


    void ButtonCountdown(){
        timePressed = Time.time;
    }

    float GetCountdown(){
        return hackDelay - Time.time + timePressed;
    }

    // Update is called once per frame
    void Update()
    {

        buttonController.interactable = (InRange() && GetCountdown() < 0);
        
    }

    //verifie si un joueur est a porté du hacker
    bool InRange(){
        foreach(GameObject joueur in players){
            if(Vector3.Distance(this.transform.position, joueur.transform.position) <= hackRange) return true;
        }
        return false;
    }

    GameObject GetClosestPlayer(){
        float minDist = -1f;
        GameObject closest = null;
        foreach(GameObject joueur in players){
            float dist = Vector3.Distance(this.transform.position, joueur.transform.position);
            if( dist <= minDist || minDist < 0) {
                minDist = dist;
                closest = joueur;
            }
        }
        return closest;
    }

}
