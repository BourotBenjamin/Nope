using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerScript : MonoBehaviour {

    [SerializeField]
    List<GameObject> characterPrefab;
    [SerializeField]
    List<string>     prefabByName;

    [SerializeField]
    bool useMastserServer = true;
    [SerializeField]
    string localServerIp;
    [SerializeField]
    PlayerScript p1;
    [SerializeField]
    PlayerScript p2;
    [SerializeField]
    RectTransform serverCanevas;
    [SerializeField]
    GameObject buttonPrefab;

    private string[] serversIps;
    private int[] serversPorts;
    private int[] serversUsers;
    private GameObject[] serverBtns;
    private float lastServerListLoad = 0f;


    bool p1Vacant = true;
    bool p2Vacant = true;
    bool p1WarriorsExists = false;
    bool p2WarriorsExists = false;

    private bool clientConnected;


    NetworkView _nV;

	// Use this for initialization
	void Start () {
        serverBtns = new GameObject[0];
        Application.runInBackground = true;
        _nV = this.GetComponent<NetworkView>();

        if (GameOptionsScript.isServer)
        {
            initServer();
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

    void OnConnectedToServer()
    {
        object[] data = new object[4];
        data[0] = Network.player;
        data[1] = GameOptionsScript.warriors[0];
        data[2] = GameOptionsScript.warriors[1];
        data[3] = GameOptionsScript.warriors[2];
        this.networkView.RPC("SendWarriors", RPCMode.Server, data);
    }

    [RPC]
    void SendWarriors(NetworkPlayer player, string warrior1, string warrior2, string warrior3)
    {
        if (p1Vacant)
        {
            p1.addCharacterList(warrior1);
            p1.addCharacterList(warrior2);
            p1.addCharacterList(warrior3);
            p1.addCharacterPos(1);
            p1.InstantiateChar(characterPrefab, prefabByName, 1);
            p1.setSimulate();
            p1Vacant = false;
            setPlayer(player, p1, p2, 1);
            p1.owner = player;
            if (!p1WarriorsExists)
            {
                p1WarriorsExists = true;
                foreach (var i in p1.simulateSrcipts)
                {
                    i.setOwnerInSimulateScript(player);
                }
            }
        }
        else if (p2Vacant)
        {
            p2.addCharacterList(warrior1);
            p2.addCharacterList(warrior2);
            p2.addCharacterList(warrior3);
            p2.addCharacterPos(2);
            p2.InstantiateChar(characterPrefab, prefabByName, 2);
            p2.setSimulate();
            p2Vacant = false;
            setPlayer(player, p2, p1, 2);
            p2.owner = player;
            if (!p2WarriorsExists)
            {
                p2WarriorsExists = true;
                foreach (var i in p2.simulateSrcipts)
                {
                    i.setOwnerInSimulateScript(player);
                }
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
        if (!clientConnected && !GameOptionsScript.isServer)
        {
            if (Time.timeSinceLevelLoad - lastServerListLoad > 5f)
            {
                lastServerListLoad = Time.timeSinceLevelLoad;
                MasterServer.ClearHostList();
                MasterServer.RequestHostList("Nope");
            }
            var allBtnChanged = false;
            HostData[] data = MasterServer.PollHostList();
            if (serverBtns.Length != data.Length)
            {
                serversPorts = new int[data.Length];
                serversIps = new string[data.Length];
                serversUsers = new int[data.Length];
                for (int k = 0; k < serverBtns.Length; k++)
                    Destroy(serverBtns[k]);
                serverBtns = new GameObject[data.Length];
                allBtnChanged = true;
            }
            int i = 0;
            while (i < data.Length)
            {
                var btnChanged = false;
                string tmpIp = "";
                int j = 0;
                int port = data[i].port;
                while (j < data[i].ip.Length)
                {
                    tmpIp = data[i].ip[j] + " ";
                    j++;
                }
                if (port != serversPorts[i])
                {
                    serversPorts[i] = port;
                    btnChanged = true;
                }
                if (tmpIp != serversIps[i])
                {
                    serversIps[i] = tmpIp;
                    btnChanged = true;
                }
                if (data[i].connectedPlayers != serversUsers[i])
                {
                    serversUsers[i] = data[i].connectedPlayers;
                    btnChanged = true;
                }

                if (btnChanged || allBtnChanged)
                {
                    if (serverBtns[i] != null)
                        Destroy(serverBtns[i]);
                    var btn = ((GameObject)Instantiate(buttonPrefab)).GetComponent<GUIButtonScript>();
                    btn.MainRectTransform.SetParent(serverCanevas);
                    btn.MainRectTransform.localPosition = Vector3.zero;
                    btn.MainRectTransform.anchorMin = new Vector3(0f, 1f - (i + 1) / (float)data.Length);
                    btn.MainRectTransform.anchorMax = new Vector3(1f, 1f - i / (float)data.Length);
                    btn.MainRectTransform.offsetMin = new Vector3(0f, 0f);
                    btn.MainRectTransform.offsetMax = new Vector3(0f, 0f);
                    btn.MainRectTransform.localScale = Vector3.one;
                    btn.Text.text = tmpIp + " (" + (data[i].connectedPlayers - 1) + " / 2)";
                    btn.ButtonScript.onClick.AddListener(() =>
                    {
                        Network.Connect(tmpIp, port);
                        clientConnected = true;
                        for (int k = 0; k < serverBtns.Length; k++)
                            Destroy(serverBtns[k]);

                    });
                    serverBtns[i] = btn.gameObject;
                }
                i++;
            }
        }
    }

    
}
