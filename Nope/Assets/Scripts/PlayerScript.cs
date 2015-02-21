using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _warriors;
    [SerializeField]
    private NetworkScript network;
    [SerializeField]
    public GameObject arrowPrefab;
    [SerializeField]
    public GameObject minePrefab;
    [SerializeField]
    public GameObject meteorPrefab;
    [SerializeField]
    public GameObject fireBallPrefab;

    public int simulationsLaunched;
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

    void Start()
    {
        simulationsLaunched = 0;
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
        }
    }

    public void setSimulate()
    {
        for (int i = 0; i < warriors.ToArray().Length; i++)
        {
            SimulateScript sim = warriors.ToArray()[i].GetComponent<SimulateScript>();
            simulateSrcipts.Add(sim);
            sim.setPlayer(this);
        }
    }

    public void InstantiateChar(List<GameObject> characterPrefab, List<string> prefabByName,int group)
    {
        int j = 0, i = 0;
        foreach (string s in charactersList)
        {
            GameObject newCharacter = (GameObject)Network.Instantiate((GameObject)characterPrefab.ToArray()[prefabByName.IndexOf(s)], characterPos.ToArray()[i], Quaternion.identity, group);
            newCharacter.GetComponent<SimulateScript>().id = j++;
            warriors.Add(newCharacter);
            i++;
        }
    }

    public void setOwner(NetworkPlayer p)
    {
        if (owner.ToString().Equals("0"))
        {
            owner = p;
        }
    }

    public void EntityDied(GameObject warrior) 
    {
        warriors.Remove(warrior);
        SimulateScript sw =warrior.GetComponent<SimulateScript>();
        sw.stopActions();
        //charactersList.Remove(charactersList.ToArray()[sw.id]);
        simulateSrcipts.Remove(sw);
        if(Network.isServer && warriors.Count == 0)
        {
            network.networkView.RPC("PlayerDied", RPCMode.Others, owner);
        }
	}

    public void stopGame()
    {
        foreach(SimulateScript sim in simulateSrcipts)
        {
            sim.stopActions();
        }
    }

    public void Simulate()
    {
        foreach (SimulateScript simulation in simulateSrcipts)
        {
            ++simulationsLaunched;
            simulation.startAllSimulations();
        }
        this.networkView.RPC("SimulationStarted", RPCMode.All, simulationsLaunched);
    }

    [RPC]
    public void SimulationStarted(int nb)
    {
        simulationsLaunched = nb;
        network.SimulationStarted(this);
        if (simulationsLaunched == 0)
        {
            network.SimulationEnded(this);
        }
    }

    public void SimulationEnded()
    {
        --simulationsLaunched;
        if (simulationsLaunched == 0)
        {
            this.networkView.RPC("SimulationStarted", RPCMode.All, 0);
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
