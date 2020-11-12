using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackerController : MonoBehaviour
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
        button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Hack);
    }

    void Hack(){
        if(GetCountdown() < 0){
            ButtonCountdown();
        }
    }


    void DisableButton(){
        button.GetComponent<UnityEngine.UI.Button>().interactable = false;
    }

    void EnableButton(){
        button.GetComponent<UnityEngine.UI.Button>().interactable = true;
    }

    void ButtonCountdown(){
        DisableButton();
        timePressed = Time.time;
        Invoke("enableButton", hackDelay);
    }

    float GetCountdown(){
        return hackDelay - Time.time + timePressed;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GetCountdown());
        if(Input.GetKeyDown(KeyCode.O)){
            ButtonCountdown();
        }
    }

    //verifie si un joueur est a porté du hacker
    bool InRange(){
        foreach(GameObject joueur in joueurs){
            if(Vector3.Distance(this.transform.position, joueur.transform.position) <= hackRange) return true;
        }
        return false;
    }

    GameObject GetClosestPlayer(){
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
