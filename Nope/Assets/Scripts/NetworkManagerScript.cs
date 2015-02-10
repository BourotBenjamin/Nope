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
    bool useMastserServer = true;
    [SerializeField]
    string localServerIp;
    [SerializeField]
    PlayerScript p1;
    [SerializeField]
    PlayerScript p2;


    bool p1Vacant = true;
    bool p2Vacant = true;

    private bool clientConnected;


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

            p1.addCharacterPos(1);
            p2.addCharacterPos(2);
            p1.InstantiateChar(characterPrefab, prefabByName,1);
            p2.InstantiateChar(characterPrefab, prefabByName, 2);
            p1.setSimulate();
            p2.setSimulate();
        }
        else
        {
            if (useMastserServer)
            {
                MasterServer.RequestHostList("Nope");
                clientConnected = false;
            }
            else
            {
                clientConnected = true;
                Network.Connect(localServerIp, 6600);
            }
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

    void Update()
    {
        if (!clientConnected && !isServer)
        {
            HostData[] data = MasterServer.PollHostList();
            int i = 0;
            while (i < data.Length)
            {
                string tmpIp = "";
                int j = 0;
                while (j < data[i].ip.Length)
                {
                    tmpIp = data[i].ip[j] + " ";
                    j++;
                }
                if (data[i].connectedPlayers < 3)
                {
                    Network.Connect(tmpIp, data[i].port);
                    clientConnected = true;
                    break;
                }
                i++;
            }
        }
    }

    
}
