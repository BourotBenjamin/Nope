using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerScript : MonoBehaviour {

    [SerializeField]
    List<GameObject> characterPrefab;
    [SerializeField]
    List<string>     prefabByName;


    [SerializeField]
    bool isServer = true;
    [SerializeField]
    PlayerScript p1;
    [SerializeField]
    PlayerScript p2;

    int nbPlayer = 0;

    bool p1Vacant = true;
    bool p2Vacant = true;

    bool isP1Instancied = false;
    bool isP2Instancied = false;

    NetworkView _nV;

	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
        _nV = this.GetComponent<NetworkView>();

        if (isServer)
        {
            initServer();
            string test ="warrior";
            p1.addCharacterList(test);
            p2.addCharacterList(test);
            p1.addCharacterList(test);
            p2.addCharacterList(test);
            p1.addCharacterList(test);
            p2.addCharacterList(test);
            p1.addCharacterList(test);
            p2.addCharacterList(test);
            /*foreach (string s in p1.charactersList)
            {
                p1.addCharacter(characterPrefab.ToArray()[prefabByName.IndexOf(s)]);
            }
            foreach (string s in p2.charactersList)
            {
                p2.addCharacter(characterPrefab.ToArray()[prefabByName.IndexOf(s)]);
            }*/

            p1.addCharacterPos(1);
            p2.addCharacterPos(2);
            p1.InstantiateChar(characterPrefab, prefabByName,1);
            p2.InstantiateChar(characterPrefab, prefabByName, 2);
            p1.setSimulate();
            p2.setSimulate();
        }
        else
        {
            connectToServer();
        }
	}

    void OnPlayerConnected(NetworkPlayer player)
    {
        if (p1Vacant)
        {
            p1Vacant = false;
            setPlayer(player, p1, p2, 1);
            p1.owner = player;
            foreach (var i in p1.simulateSrcipts)
            {
                i.setOwnerInSimulateScript(player);
            }
        }
        else if (p2Vacant)
        {
            p2Vacant = false;
            setPlayer(player, p2, p1, 2);
            p2.owner = player;
            foreach (var i in p2.simulateSrcipts)
            {
                i.setOwnerInSimulateScript(player);
            }
        }
    }



    void OnPlayerDisconnected(NetworkPlayer p)
    {
        if (p1.owner == p)
        {
            p1.owner = new NetworkPlayer();
            p1Vacant = true;
            _nV.RPC("removeOwner", RPCMode.All, 1);
        }
        else if (p2.owner == p)
        {
            p2.owner = new NetworkPlayer();
            p2Vacant = true;
            _nV.RPC("removeOwner", RPCMode.All, 2);
        }
    }

    [RPC]
    void assignPlayer(NetworkPlayer player, int playerNum)
    {
        if (playerNum == 1)
            p1.setOwner(player);
        else if (playerNum == 2)
            p2.setOwner(player);
    }

    [RPC]
    void removeOwner(int numPlayer)
    {
        if (numPlayer == 1)
        {
            p1.owner = new NetworkPlayer();
        }
        else if (numPlayer == 2)
        {
            p2.owner = new NetworkPlayer();
        }
    }

    public void setPlayer(NetworkPlayer player, PlayerScript assign ,PlayerScript disable , int playerNum)
    {
        assign.setOwner(player);
        _nV.RPC("assignPlayer", RPCMode.OthersBuffered, player, playerNum);
    }

    void initServer()
    {
        Network.InitializeSecurity();
        bool useNat = !Network.HavePublicAddress();
        Network.InitializeServer(2, 6600, useNat);
        MasterServer.RegisterHost("Nope", "nope nope nope", "let's play");
    }

    void connectToServer()
    {
        HostData[] data = MasterServer.PollHostList();
        string tmpIp = "";
        foreach (var element in data)
        {
            
            int i = 0;
            while (i < element.ip.Length)
            {
                tmpIp = element.ip[i] + " ";
                i++;
            }
           
        }
        Network.Connect(tmpIp, 6600);
    }

    
}
