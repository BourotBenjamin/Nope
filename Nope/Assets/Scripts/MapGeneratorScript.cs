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
	// Use this for initialization
	void Start () {
        _seed = getRandomString();
        Debug.Log(_seed);
        int hCode = _seed.GetHashCode();
        if (hCode < 0)
            hCode = -hCode;
        if(Network.isServer)
        {
            //Send hCode to client
        }
        //////////////////////////////////////////////////////////////
        // Set the actual vars of the map depending on the seed string
        //////////////////////////////////////////////////////////////
        int groundWidth  = hCode % (maxGroundWidth  - minGroundWidth)  + minGroundWidth;
        int groundHeight = hCode % (maxGroundHeight - minGroundHeight) + minGroundHeight;
        int nbRoom = hCode % (nbRoomMax - nbRoomMin) + nbRoomMin;
        int sizeRoom = hCode % (sizeRoomMax - sizeRoomMin) + sizeRoomMin;
        int nbChest = hCode % (nbChestMax - nbChestMin) + nbChestMin;
        int nbTrap = hCode % (nbTrapMax - nbTrapMin) + nbTrapMin;
        //////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////
        //Set room, treasure and trap position on the map
        //////////////////////////////////////////////////////////////
        Vector3[] posRoom  = new Vector3[nbRoom];
        Vector3[] posChest = new Vector3[nbChest];
        Vector3[] posTrap  = new Vector3[nbTrap];
        for (int i = 0; i < nbRoom; i++)
        {
            int x = ((hCode + (i * i) + 1) * nbRoomMax);
            x = (x > 0 ? x : -x) % (groundWidth * 2 * 5);
            x = x -groundWidth * 5;
            x += (x == (groundWidth) * 5) ? -1 : (x == (-groundWidth) * 5) ? 1 : 0;
            int z = ((hCode + ((i+1) * i) + 1) * nbRoomMin);
            z = (z > 0 ? z : -z) % (groundHeight* 2 * 5);
            z = z - groundHeight * 5;
            z += (z == (groundHeight) * 5) ? -1 : (z == (-groundHeight) * 5) ? 1 : 0;
            posRoom[i] = new Vector3(x, 0.5f, z);
        }

        for (int i = 0; i < nbChest; i++)
        {
            int x = ((hCode + (i * i) + 1) * (nbChestMax * nbChestMin));
            x = (x > 0 ? x : -x) % (groundWidth * 2 * 5-2);
            x = x - (groundWidth) * 5;
            x += (x == (groundWidth) * 5) ? -1 : (x == (-groundWidth) * 5) ? 1 : 0;
            int z = ((hCode + ((i + 1) * i) + 1) * (nbChestMin + nbChestMax));
            z = (z > 0 ? z : -z) % (groundHeight * 2 * 5-2);
            z = z - (groundHeight) * 5;
            z += (z == (groundHeight) * 5) ? -1 : (z == (-groundHeight) * 5) ? 1 : 0;
            posChest[i] = new Vector3(x, 0.5f, z);
        }
        for (int i = 0; i < nbTrap; i++)
        {
            int x = ((hCode + (i * i) + 1) * (nbTrapMax * nbTrapMin));
            x = (x > 0 ? x : -x) % (groundWidth * 2 * 5-2);
            x = x - (groundWidth) * 5;
            x +=(x == (groundWidth) * 5 )? - 1 : (x == (-groundWidth) * 5 )? 1 : 0;
            int z = ((hCode + ((i + 1) * i) + 1) * (nbTrapMin + nbTrapMax));
            z = (z > 0 ? z : -z) % (groundHeight * 2 * 5-2);
            z = z - (groundHeight) * 5;
            z += (z == (groundHeight) * 5) ? -1 : (z == (-groundHeight) * 5) ? 1 : 0;
            posTrap[i] = new Vector3(x, 0.5f, z);
        }
        //////////////////////////////////////////////////////////////

        //////////////////////////////////////////////////////////////
        //Create the map
        //////////////////////////////////////////////////////////////
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.transform.position = new Vector3(0, 0, 0);
        ground.transform.localScale = new Vector3(groundWidth, 1, groundHeight);
        ground.isStatic = true;
        for (int i = -groundWidth; i <= groundWidth; i++)
        {
            for (int j = -groundHeight; j <= groundHeight; j++)
            {
                if (i == -groundWidth || i == groundWidth)
                {
                    GameObject.Instantiate(prefabList[0], new Vector3(i*5, 0, j*5), new Quaternion());
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
            GameObject chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
            chest.transform.position = posChest[i];
            chest.isStatic = true;
        }
        for (int i = 0; i < nbRoom; i++)
        {
            GameObject trap = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            trap.transform.position = posTrap[i];
            trap.isStatic = true;
        }
        //////////////////////////////////////////////////////////////
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
