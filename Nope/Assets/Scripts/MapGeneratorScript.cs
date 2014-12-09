using UnityEngine;
using System.Collections;

public class MapGeneratorScript : MonoBehaviour {


    
   
    [SerializeField]
    string _seed;
    [SerializeField]
    int minGroundWidth = 2 ;
    [SerializeField]
    int maxGroundWidth = 6;
    [SerializeField]
    int minGroundHeight = 2;
    [SerializeField]
    int maxGroundHeight = 5;
    [SerializeField]
    int nbRoomMin = 2;
    [SerializeField]
    int nbRoomMax = 7;
    [SerializeField]
    int sizeRoomMin = 1;
    [SerializeField]
    int sizeRoomMax = 3;
    [SerializeField]
    int nbChestMin = 4;
    [SerializeField]
    int nbChestMax = 10;
    [SerializeField]
    int nbTrapMin = 7;
    [SerializeField]
    int nbTrapMax = 10;

    [SerializeField]
    GameObject [] prefabList ;

    private NetworkView _nV;

    private int hCode = 0;
    private int groundWidth = 0;
    private int groundHeight = 0;
    private int nbRoom = 0;
    private int sizeRoom = 0;
    private int nbChest = 0;
    private int nbTrap = 0;

    private Vector3[] posRoom;
    private Vector3[] posChest;
    private Vector3[] posTrap;
    private GameObject[] chests;
    private GameObject[] traps;
    private bool[] chestStatus;
    private bool[] trapStatus;

    private bool isLoaded = false;

    [RPC]
    void SetHcode(int code, bool [] chestStats, bool [] trapStats)
    {
        if(!isLoaded)
        {
            hCode = code;
            SetValues();
            SetPositions();
            chestStatus = chestStats;
            trapStatus = trapStats;
            createMap();
        }
        
    }

	// Use this for initialization
    void OnServerInitialized()
    {
        if(Network.isServer)
        {
            _nV = this.GetComponent<NetworkView>();

            _seed = getRandomString();
            Debug.Log(_seed);
            hCode = _seed.GetHashCode();
            if (hCode < 0)
                hCode = -hCode;

            SetValues();

            SetPositions();

            createMap();
        }
        
    }

    void OnPlayerConnected()
    {
        _nV.RPC("SetHcode", RPCMode.AllBuffered, hCode, chestStatus, trapStatus);
    }

    void SetValues()
    {
        //////////////////////////////////////////////////////////////
        // Set the actual vars of the map depending on the seed string
        //////////////////////////////////////////////////////////////
        groundWidth = hCode % (maxGroundWidth - minGroundWidth) + minGroundWidth;
        groundHeight = hCode % (maxGroundHeight - minGroundHeight) + minGroundHeight;
        nbRoom = hCode % (nbRoomMax - nbRoomMin) + nbRoomMin;
        sizeRoom = hCode % (sizeRoomMax - sizeRoomMin) + sizeRoomMin;
        nbChest = hCode % (nbChestMax - nbChestMin) + nbChestMin;
        nbTrap = hCode % (nbTrapMax - nbTrapMin) + nbTrapMin;
        //////////////////////////////////////////////////////////////
        posRoom = new Vector3[nbRoom];
        posChest = new Vector3[nbChest];
        posTrap = new Vector3[nbTrap];

        chestStatus = new bool[nbChest];
        trapStatus = new bool[nbTrap];

        chests = new GameObject[nbChest];
        traps = new GameObject[nbTrap];
    }

    void SetPositions()
    {
        //////////////////////////////////////////////////////////////
        //Set room, treasure and trap position on the map
        //////////////////////////////////////////////////////////////
        
        for (int i = 0; i < nbRoom; i++)
        {
            int x = ((hCode + (i * i) + 1) * nbRoomMax);
            x = (x > 0 ? x : -x) % (groundWidth * 2 * 5);
            x = x - groundWidth * 5;
            x += (x == (groundWidth) * 5) ? -1 : (x == (-groundWidth) * 5) ? 1 : 0;

            int z = ((hCode + ((i + 1) * i) + 1) * nbRoomMin);
            z = (z > 0 ? z : -z) % (groundHeight * 2 * 5);
            z = z - groundHeight * 5;
            z += (z == (groundHeight) * 5) ? -1 : (z == (-groundHeight) * 5) ? 1 : 0;
            posRoom[i] = new Vector3(x, 0.5f, z);
        }

        for (int i = 0; i < nbChest; i++)
        {
            int x = ((hCode + (i * i) + 1) * (nbChestMax * nbChestMin));
            x = (x > 0 ? x : -x) % (groundWidth * 2 * 5 - 2);
            x = x - (groundWidth) * 5;
            x += (x == (groundWidth) * 5) ? -1 : (x == (-groundWidth) * 5) ? 1 : 0;

            int z = ((hCode + ((i + 1) * i) + 1) * (nbChestMin + nbChestMax));
            z = (z > 0 ? z : -z) % (groundHeight * 2 * 5 - 2);
            z = z - (groundHeight) * 5;
            z += (z == (groundHeight) * 5) ? -1 : (z == (-groundHeight) * 5) ? 1 : 0;
            posChest[i] = new Vector3(x, 0.5f, z);
            chestStatus[i] = true;
        }
        for (int i = 0; i < nbTrap; i++)
        {
            int x = ((hCode + (i * i) + 1) * (nbTrapMax * nbTrapMin));
            x = (x > 0 ? x : -x) % (groundWidth * 2 * 5 - 2);
            x = x - (groundWidth) * 5;
            x += (x == (groundWidth) * 5) ? -1 : (x == (-groundWidth) * 5) ? 1 : 0;

            int z = ((hCode + ((i + 1) * i) + 1) * (nbTrapMin + nbTrapMax));
            z = (z > 0 ? z : -z) % (groundHeight * 2 * 5 - 2);
            z = z - (groundHeight) * 5;
            z += (z == (groundHeight) * 5) ? -1 : (z == (-groundHeight) * 5) ? 1 : 0;
            posTrap[i] = new Vector3(x, 0.5f, z);
            trapStatus[i] = true;
        }
        //////////////////////////////////////////////////////////////
    }

    void createMap()
    {
        

        //////////////////////////////////////////////////////////////
        //Create the map
        //////////////////////////////////////////////////////////////
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = new Vector3(0, 0, 0);
        ground.transform.localScale = new Vector3(groundWidth, 1, groundHeight);
        ground.isStatic = true;

        //GameObject wall1 =(GameObject) GameObject.Instantiate(prefabList[0], new Vector3(0, 0, groundHeight * 5), new Quaternion());
        //wall1.transform.localScale = new Vector3(groundWidth, 1f, 0.2f);
        for (int i = -groundWidth; i <= groundWidth; i++)
        {
            for (int j = -groundHeight; j <= groundHeight; j++)
            {
                if (i == -groundWidth || i == groundWidth)
                {
                    GameObject.Instantiate(prefabList[0], new Vector3(i * 5, 0, j * 5), new Quaternion());
                }
                if (j == -groundHeight || j == groundHeight)
                {
                    GameObject.Instantiate(prefabList[1], new Vector3(i * 5, 0, j * 5), new Quaternion());
                }
            }

        }

        for (int i = 0; i < nbRoom; i++)
        {
            GameObject.Instantiate(prefabList[1], posRoom[i], new Quaternion());
        }
        for (int i = 0; i < nbChest; i++)
        {
            if(chestStatus[i])
            {
                chests[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                chests[i].transform.position = posChest[i];
                chests[i].isStatic = true;
            }
            
        }
        for (int i = 0; i < nbRoom; i++)
        {
            if(trapStatus[i])
            {
                traps[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                traps[i].transform.position = posTrap[i];
                traps[i].isStatic = true;
            }
            
        }
        //////////////////////////////////////////////////////////////
        isLoaded = true;
    }

    string getRandomString ()
    {
        int nbCharInString = Random.Range(0, 15);
        string result = "";
        for (int i = 0; i < nbCharInString; i++ )
        {
            result += (char)Random.Range(0, 255);
        }
        return result;    
    }
	// Update is called once per frame
	void Update () {
	    
	}
}
