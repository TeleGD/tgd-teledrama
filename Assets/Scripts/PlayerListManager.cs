using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using System.Linq;
using Photon.Realtime;

public class PlayerListManager : MonoBehaviourPun
{
    public static PlayerListManager instance;

    //dictionnaire associant des ActorNuber à une structure PlayerData
    //on peut alors récupérer les données avec playerList.Get(info.Sender.ActorNumber)
    public Dictionary<int, PlayerData> playerList = new Dictionary<int, PlayerData>();

    //liste des couleurs disponnibles, permet de ne pas avoir deux fois la même couleur
    private List<int> availableColorsIds = new List<int>();

    public RectTransform playerListUI;
    private bool playerListUIActive = true;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            //initialise les couleurs disponibles
            Color[] colors = GameManager.instance.playerColors;
            for (int i = 0; i < colors.Length; i++)
            {
                availableColorsIds.Add(i);
            }
        }
    }

    //ajoute un joueur à la liste des joueurs et lui assigne une couleur
    [PunRPC]
    public void AddPlayer(int viewID, PhotonMessageInfo info)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        print("Adding Player " + info.Sender.NickName);

        PlayerData data = new PlayerData(); //crée la structure

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

        //assignation de la couleur
        Vector3 c = GameManager.instance.GetVectorColor(data.colorIndex);
        data.playerView.RPC("SetPlayerColor", RpcTarget.AllBuffered, c);

        photonView.RPC("AddPlayerToList", RpcTarget.AllBuffered, info.Sender.ActorNumber, data);
    }

    [PunRPC]
    public void AddPlayerToList(int key, PlayerData data)
    {
        playerList.Add(key, data);
        UpdatePlayerListUI();
    }

    [PunRPC]
    public void UpdatePlayerInList(int key, PlayerData data)
    {
        if (playerList.Keys.Contains(key))
        {
            playerList[key] = data;
            UpdatePlayerListUI();
        }
    }

    public void AssignRoles()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        int pcount = playerList.Count;

        if (pcount < 3)
            return;
        
        int hackerCount = 1 + Mathf.FloorToInt(pcount / 6f);
        //index 0 = directeur, index 1 à n = hacker
        int[] rolesIndexes = TGDUtils.RandomIntegers(1 + hackerCount, pcount);

        int i = 0;
        Dictionary<int, PlayerData> clone = new Dictionary<int, PlayerData>(playerList);
        foreach (KeyValuePair<int, PlayerData> entry in clone)
        {
            //choix du role
            int role = 0;
            if (i == rolesIndexes[0])
                role = (int)GameManager.Role.Director;
            else
            {
                //on parcourt les hackers
                for(int j = 1; j < rolesIndexes.Length; j++)
                {
                    if(i == rolesIndexes[j])
                        role = (int)GameManager.Role.Hacker;
                }
            }

            //mise à jour du dictionnaire
            PlayerData data = entry.Value;
            data.role = role;
            playerList[entry.Key] = data;

            //envoie au joueur son role
            data.playerView.RPC("SetRole", data.playerView.Owner, role);
            photonView.RPC("UpdatePlayerInList", RpcTarget.OthersBuffered, entry.Key, data);

            i++;
        }
    }

    public void KickPlayer(int actorNumber)
    {
        PhotonView targetView = playerList[actorNumber].playerView;
        targetView.RPC("KickPlayer", RpcTarget.AllBuffered);
        playerList[actorNumber].isAlive = false;
        photonView.RPC("UpdatePlayerInList", RpcTarget.AllBuffered, actorNumber, playerList[actorNumber]);
    }

    //UI

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playerListUIActive = !playerListUIActive;
            playerListUI.gameObject.SetActive(playerListUIActive);
        }
    }

    public void UpdatePlayerListUI()
    {
        while(playerList.Count > playerListUI.childCount)
        {
            GameObject line = Instantiate(playerListUI.GetChild(0).gameObject, playerListUI);
            line.GetComponent<RectTransform>().anchoredPosition = Vector2.down * (8 + (playerListUI.childCount - 1) * 44);
            playerListUI.sizeDelta = new Vector2(400, 12 + (playerListUI.childCount * 44));
        }

        bool isDirector = GameManager.instance.GetMyRole() == GameManager.Role.Director;
        int i = 0;
        foreach (KeyValuePair<int, PlayerData> entry in playerList)
        {
            playerListUI.GetChild(i).Find("Name").GetComponent<Text>().text = entry.Value.playerView.Owner.NickName;
            playerListUI.GetChild(i).Find("Role").GetComponent<Text>().text = GameManager.instance.GetRoleName((GameManager.Role)entry.Value.role);
            playerListUI.GetChild(i).Find("Role").gameObject.SetActive(!entry.Value.isAlive);
            playerListUI.GetChild(i).Find("Dead").gameObject.SetActive(!entry.Value.isAlive);

            Button btn = playerListUI.GetChild(i).Find("Kick").GetComponent<Button>();
            btn.gameObject.SetActive(isDirector && entry.Value.isAlive);
            if(isDirector && entry.Value.isAlive)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(delegate { KickPlayer(entry.Key); });
            }
            
            i++;
        }
    }
}

//classe contenant les informations utiles du joueur
[System.Serializable]
public class PlayerData
{
    public int colorIndex;
    public int role;
    public PhotonView playerView;
    public bool isAlive = true;

    public static object Deserialize(byte[] data)
    {
        PlayerData result = new PlayerData();
        result.colorIndex = data[0];
        result.role = data[1];
        int viewID = (data[2] << 8) + data[3];
        Debug.Log("Finding photon view with id " + viewID);
        result.playerView = PhotonView.Find(viewID);
        result.isAlive = data[4] != 0;
        return result;
    }

    public static byte[] Serialize(object obj)
    {
        PlayerData target = (PlayerData)obj;
        int viewID = target.playerView.ViewID;
        return new byte[] { (byte)target.colorIndex, (byte)target.role, (byte)(viewID >> 8), (byte)viewID, (byte)(target.isAlive ? 1 : 0) };
    }
}