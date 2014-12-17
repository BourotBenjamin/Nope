using UnityEngine;
using System.Collections;

// Represent an action
public abstract class ActionScript
{

	protected int duration;
    protected Vector3 destination;
    protected bool started;
    protected SimulateScript simulation;
    protected float startTime;

    public ActionScript(Vector3 destination, int duration)
    {
        this.duration = duration;
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

    // Override it and call this
    // Starts an action
    public void simulate(Vector3 destination, int duration)
    {
        Debug.LogError("Start");
        started = true;
        this.startTime = Time.time;
    }

    // Override it and call this
    // Stops an action
    public void endSimulation()
    {
        started = false;
        simulation.simulateActionAtNextIndex();
    }


    public abstract object[] getArrayOfParams();
    public abstract string getName();
    public abstract void FixedUpdate(Transform transform, Rigidbody rigidbody);
}
