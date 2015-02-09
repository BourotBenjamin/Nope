using UnityEngine;
using System.Collections;

// Represent an action
public abstract class ActionScript
{

	protected int duration;
    protected Vector3 _destination;
    public Vector3 destination
    {
        get { return _destination; }
        set { _destination = value;}
    }
    protected bool started;
    protected SimulateScript simulation;
    protected float startTime;

    protected bool destinationNeeded;

    public bool isDestinationNeeded()
    {
        return destinationNeeded;
    }

    public ActionScript()
    {
        this.duration = -1;
    }

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
    public abstract void doAction(Transform transform, Rigidbody rigidbody);

    /*public ActionScript actionButton( int yOffset)
    {
        if (GUI.Button(new Rect(0, yOffset, 120, 20), getName()))
        {
            return this;
        }
        else
        {
            return null;
        }
    }*/
}
