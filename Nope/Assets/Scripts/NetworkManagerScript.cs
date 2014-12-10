using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {

    [SerializeField]
    bool isServer = true;
    [SerializeField]
    PlayerScript p1;
    [SerializeField]
    PlayerScript p2;

    int nbPlayer = 0;

    bool p1Vacant = true;
    bool p2Vacant = true;

    NetworkView _nV;

	// Use this for initialization
	void Start () {
        Application.runInBackground = true;
        _nV = this.GetComponent<NetworkView>();

        if (isServer)
        {
            initServer();
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
            setPlayer(player, p1, p2, 1);
            p1Vacant = false;
        }
        else if (p2Vacant)
        {
            setPlayer(player, p2, p1, 2);
            p2Vacant = false;
        }
    }

    void OnPlayerDisconnected(NetworkPlayer p)
    {
        if (p1.owner == p)
        {
            p1.owner = new NetworkPlayer();
            p1Vacant = true;
        }
        else if (p2.owner == p)
        {
            p2.owner = new NetworkPlayer();
            p2Vacant = true;
        }
        nbPlayer--;
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
    void disablePScript(NetworkPlayer player, int playerNum)
    {
        if (playerNum == 1)
            p2.Disable(player);
         else if (playerNum == 2)
            p1.Disable(player);
    }

    public void setPlayer(NetworkPlayer player, PlayerScript assign ,PlayerScript disable , int playerNum)
    {
        assign.setOwner(player);
        //assign.Disable();
        //disable.Disable();

        _nV.RPC("assignPlayer", RPCMode.OthersBuffered, player, playerNum);
        _nV.RPC("disablePScript", RPCMode.OthersBuffered, player, playerNum);

        nbPlayer ++;
    }

	// Update is called once per frame
	void Update () {
	}

    void initServer()
    {
        Network.InitializeSecurity();
        bool useNat = !Network.HavePublicAddress();
        Network.InitializeServer(2, 6600, useNat);
        //MasterServer.RegisterHost("Nope", "nope nope nope", "let's play");
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
        //Network.Connect("192.168.0.43", 6600);
        Network.Connect(tmpIp, 6600);
    }

    
}
