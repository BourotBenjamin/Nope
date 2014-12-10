using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    List<GameObject> warriors;

    [SerializeField]
    GameObject goTest;
    bool spawned;
    private NetworkPlayer _owner ;
    public  NetworkPlayer owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    List<SimulateScript> simulateSrcipts;

    void OnConnectedToServer()
    {
        if(owner.ToString().Equals("1") && !spawned)
        {
            warriors.Add((GameObject)Network.Instantiate(goTest,new Vector3((float)(-MapGeneratorScript.GroundWidth*5), 0.2f,(float) (-MapGeneratorScript.GroundHeight*5)), new Quaternion(),1));
            spawned = true;
        }
            
        else if (owner.ToString().Equals("2") && !spawned)
        {
            warriors.Add((GameObject)Network.Instantiate(goTest,new Vector3((float)(MapGeneratorScript.GroundWidth*5), 0.2f,(float) (MapGeneratorScript.GroundHeight*5)), new Quaternion(),2));
            spawned = true;
        }
        
        simulateSrcipts = new List<SimulateScript>();
        foreach(GameObject warrior in warriors)
        {
            simulateSrcipts.Add(warrior.GetComponent<SimulateScript>());
        }
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
        simulateSrcipts.Remove(warrior.GetComponent<SimulateScript>());
        if(warriors.Count == 0)
        {
            //TODO Died
        }
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
