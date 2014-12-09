using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    List<GameObject> warriors;

    private NetworkPlayer _owner ;
    public  NetworkPlayer owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    List<SimulateScript> simulateSrcipts;

    void Start()
    {
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
