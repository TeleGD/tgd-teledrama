using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerListManager : MonoBehaviourPun
{
    public static PlayerListManager instance;

    //dictionnaire associant des ActorNuber à une structure PlayerData
    //on peut alors récupérer les données avec playerList.Get(info.Sender.ActorNumber)
    public Dictionary<int, PlayerData> playerList = new Dictionary<int, PlayerData>();

    //liste des couleurs disponnibles, permet de ne pas avoir deux fois la même couleur
    public List<int> availableColorsIds = new List<int>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            this.enabled = false;
            return;
        }

        //initialise les couleurs disponibles
        Color[] colors = GameManager.instance.playerColors;
        for(int i = 0; i < colors.Length; i++)
        {
            availableColorsIds.Add(i);
        }
    }

    //ajoute un joueur à la liste des joueurs et lui assigne une couleur
    [PunRPC]
    public void AddPlayer(int viewID, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        print("Adding Player " + info.Sender.NickName);

        PlayerData data; //crée la structure

        if(availableColorsIds.Count > 0)
        {
            int selectedColor = Random.Range(0, availableColorsIds.Count);
            data.colorIndex = availableColorsIds[selectedColor];
            availableColorsIds.RemoveAt(selectedColor);
            print("Selected color " + selectedColor);
        }
        else
            data.colorIndex = -1; //pas de couleur dispo, devrait être géré comme du gris

        data.role = 0;
        data.playerView = PhotonView.Find(viewID);
        playerList.Add(info.Sender.ActorNumber, data);

        //assignation de la couleur
        Vector3 c = GameManager.instance.GetVectorColor(data.colorIndex);
        data.playerView.RPC("SetPlayerColor", RpcTarget.AllBuffered, c);
    }

    public void AssignRoles()
    {
        int directorIndex = Random.Range(0, playerList.Count); //choisi le directeur

        int i = 0;
        Dictionary<int, PlayerData> clone = new Dictionary<int, PlayerData>(playerList);
        foreach (KeyValuePair<int, PlayerData> entry in clone)
        {
            //choix du role
            int role = 0;
            if (i == directorIndex)
                role = (int)GameManager.Roles.Director;
            else
                role = Random.Range(0, 2);

            //mise à jour du dictionnaire
            PlayerData data = entry.Value;
            data.role = role;
            playerList[entry.Key] = data;

            //envoie au joueur son role
            data.playerView.RPC("SetRole", data.playerView.Owner, role);

            i++;
        }
    }

    //structure contenant les informations utiles du joueur
    public struct PlayerData
    {
        public int colorIndex;
        public int role;
        public PhotonView playerView;
    }
}
