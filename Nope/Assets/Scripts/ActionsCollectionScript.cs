using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActionsCollectionScript : MonoBehaviour {

	Dictionary<int, ActionScript> actions;
	// Use this for initialization
	void Start () 
	{
		actions = new Dictionary<int, ActionScript>();
	}

	public void addAction(int time, Vector3 destination, AbstractActionScript action, int duration)
	{
		actions[time] = new ActionScript(time, destination, action, duration);
	}

	public ActionScript getAction(int time)
	{
		if (actions.ContainsKey(time)) 
		{
			return actions[time];
		} 
		else 
		{
			return null;
		}
	}
}
