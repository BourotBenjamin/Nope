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
    int nbTrapMin = 7;
    [SerializeField]
    int nbTrapMax = 10;
    [SerializeField]
    GameObject trapPrefab;
    [SerializeField]
    Material groundMaterial;

    [SerializeField]
    GameObject [] prefabList ;

    private NetworkView _nV;

    static private int hCode = 0;
    static public int HCode
    {
        get { return hCode; }
    }
    static private int groundWidth = 0;
    static public int GroundWidth
    {
        get { return groundWidth; }
    }
    static private int groundHeight = 0;
    static public int GroundHeight
    {
        get { return groundHeight; }
    }
    static private int nbRoom = 0;
    static public int NBRoom
    {
        get { return nbRoom; }
    }
    static private int sizeRoom = 0;
    static public int SizeRoom
    {
        get { return sizeRoom; }
    }
    static private int nbTrap = 0;
    static public int NBtrap
    {
        get { return nbTrap; }
    }

    private Vector3[] posRoom;
    private Vector3[] posTrap;
    private bool[] trapStatus;

    private bool isLoaded = false;

    [RPC]
    void SetHcode(int code, bool [] trapStats)
    {
        if(!isLoaded)
        {
            hCode = code;
            SetValues();
            SetPositions();
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
        _nV.RPC("SetHcode", RPCMode.AllBuffered, hCode, trapStatus);
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
        nbTrap = hCode % (nbTrapMax - nbTrapMin) + nbTrapMin;
        //////////////////////////////////////////////////////////////
        posRoom = new Vector3[nbRoom];
        posTrap = new Vector3[nbTrap];

        trapStatus = new bool[nbTrap];
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
        ground.tag = "Ground";
        ground.renderer.material = groundMaterial;

        GameObject wall1 =GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall1.transform.position = new Vector3(0, 0, groundHeight * 5);
        wall1.transform.localScale = new Vector3(groundWidth*10, 1f, 0.2f);
        wall1.tag = "Wall";
        GameObject wall11 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall11.transform.position = new Vector3(0, 0, -groundHeight * 5);
        wall11.transform.localScale = new Vector3(groundWidth*10, 1f, 0.2f);
        wall11.tag = "Wall";
        GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall2.transform.position = new Vector3(groundWidth * 5, 0, 0);
        wall2.transform.localScale = new Vector3(0.2f, 1f, groundHeight*10);
        wall2.tag = "Wall";
        GameObject wall22 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall22.transform.position = new Vector3(-groundWidth * 5, 0, 0);
        wall22.transform.localScale = new Vector3(0.2f, 1f, groundHeight*10);
        wall22.tag = "Wall";


        /*for (int i = -groundWidth; i <= groundWidth; i++)
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

        }*/

        for (int i = 0; i < nbRoom; i++)
        {
            GameObject room =(GameObject) GameObject.Instantiate(prefabList[1], posRoom[i], new Quaternion());
            room.tag = "Room";
        }
        for (int i = 0; i < nbRoom; i++)
        {
            if(trapStatus[i])
            {
                Instantiate(trapPrefab, posTrap[i], Quaternion.identity);
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
