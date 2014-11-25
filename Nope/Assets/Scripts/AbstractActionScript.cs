using UnityEngine;
using System.Collections;

public abstract class AbstractActionScript : MonoBehaviour 
{

	private bool started;

	public bool getStarted()
	{
		return started;
	}

	public void simulate(Vector3 destination, int duration)
	{
		started = true;
	}

	public void endSimulation()
	{
		started = false;
	}
}
