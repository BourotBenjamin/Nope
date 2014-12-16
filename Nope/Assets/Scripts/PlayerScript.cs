using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _warriors;
    public List<GameObject> warriors
    {
        get { return _warriors; }
        set { _warriors = value; }
    }

    [SerializeField]
    private List<string> _charactersList;
    public List<string> charactersList
    {
        get { return _charactersList; }
        set { _charactersList = value; }
    }

    [SerializeField]
    private List<Vector3> _characterPos;
    public List<Vector3> characterPos
    {
        get { return _characterPos; }
        set { _characterPos = value; }
    }

    bool spawned;

    [SerializeField]
    private NetworkPlayer _owner ;
    public  NetworkPlayer owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    private List<SimulateScript> _simulateSrcipts;
    public List<SimulateScript> simulateSrcipts
    {
        get { return _simulateSrcipts; }
        set { _simulateSrcipts = value; }
    }

    NetworkView _nV;

    void Start()
    {
        _nV = GetComponent<NetworkView>();
        simulateSrcipts = new List<SimulateScript>();
        characterPos = new List<Vector3>();        
    }

    public void addCharacterList(string character)
    {
        charactersList.Add(character);
    }

    public void addCharacterPos(int nbPlayer)
    {
        for (int i = 0; i< charactersList.ToArray().Length ; i++)
        {
            characterPos.Add(new Vector3(i % 2 == 0? 2 * i : -5 * i, 0.5f, nbPlayer == 1 ? -MapGeneratorScript.GroundHeight * 5+2 : MapGeneratorScript.GroundHeight * 5-2));
        }
    }

    public void addCharacter(GameObject character)
    {
        warriors.Add(character);
    }

    public void setCharOwner(NetworkPlayer p)
    {
        foreach (var i in simulateSrcipts)
        {
            i.setOwnerInSimulateScript(p);
            Debug.LogError(i.owner);
        }
    }

    public void setSimulate()
    {
        for (int i = 0; i < warriors.ToArray().Length; i++)
        {
            simulateSrcipts.Add(warriors.ToArray()[i].GetComponent<SimulateScript>());
        }
    }

    public void InstantiateChar(int group)
    {
        int j = 0;
        for (int i = 0; i < warriors.ToArray().Length; i++ )
        {
            warriors.ToArray()[i].GetComponent<SimulateScript>().id = j++;
            warriors.Add((GameObject)Network.Instantiate((GameObject)warriors.ToArray()[i], characterPos.ToArray()[i], Quaternion.identity,group));
            warriors.Remove(warriors.ToArray()[i]);
            //warriors.ToArray()[i].networkView.owner = owner;
            //simulateSrcipts.Add(warriors.ToArray()[i].GetComponent<SimulateScript>());
        }
    }

    void OnConnectedToServer()
    {

    }


    void Update()
    {
        //Debug.LogError(owner);
    }

    public void setOwner(NetworkPlayer p)
    {
        if (owner.ToString().Equals("0"))
        {
            owner = p;
        }
    }

    public void Disable(NetworkPlayer p)
    {
        if(owner != p)
            this.enabled = false;
    }
    public void EntityDied(GameObject warrior) 
    {
        warriors.Remove(warrior);
        Object.Destroy(warrior);
        SimulateScript sw =warrior.GetComponent<SimulateScript>();
        charactersList.Remove(charactersList.ToArray()[sw.id]);
        simulateSrcipts.Remove(sw);
        if(warriors.Count == 0)
        {
            //TODO Died
        }
	}
    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Debug.Log("New object instantiated by " + info.sender);
    }
    public void Simulate()
    {
        foreach (SimulateScript simulation in simulateSrcipts)
        {
            simulation.startAllSimulations();
        }
    }

    public void SendSimulation()
    {
        foreach (SimulateScript simulation in simulateSrcipts)
        {
            simulation.sendActionToAll();
        }
    }
}
