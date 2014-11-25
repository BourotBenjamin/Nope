using UnityEngine;
using System.Collections;

// Represent an action
public abstract class ActionScript : MonoBehaviour {

	private int duration;
    private Vector3 destination;
    private bool started;
    private SimulateScript simulation;

    public ActionScript(Vector3 destination, int duration = -1)
    {
        this.duration = duration;
        this.destination = destination;
    }

    public ActionScript(Vector3 destination)
    {
        this.duration = -1;
        this.destination = destination;
    }

    public void setSimulation(SimulateScript simulation)
    {
        if (this.simulation == null)
            this.simulation = simulation;
    }

	public int getDuration() 
	{
		return this.duration;
	}

	public Vector3 getDestination() 
	{
		return this.destination;
    }
    public bool getStarted()
    {
        return started;
    }

    // Override it
    // Starts an action
    public void simulate(Vector3 destination, int duration)
    {
        started = true;
    }

    // Override it
    // Stops an action
    public void endSimulation()
    {
        started = false;
        simulation.simulateActionAtNextIndex();
    }
	
}
