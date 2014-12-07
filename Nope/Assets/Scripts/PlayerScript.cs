using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    List<GameObject> warriors;

    List<SimulateScript> simulateSrcipts;

    void Start()
    {
        simulateSrcipts = new List<SimulateScript>();
        foreach(GameObject warrior in warriors)
        {
            simulateSrcipts.Add(warrior.GetComponent<SimulateScript>());
        }
    }

	// Use this for initialization
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

    // Update is called once per frame
    public void Simulate()
    {
        foreach (SimulateScript simulation in simulateSrcipts)
        {
            simulation.startAllSimulations();
        }
    }

    // Update is called once per frame
    public void SendSimulation()
    {
        foreach (SimulateScript simulation in simulateSrcipts)
        {
            simulation.sendActionToAll();
        }
    }
}
