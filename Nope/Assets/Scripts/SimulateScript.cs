using UnityEngine;
using System.Collections;

public class SimulateScript : MonoBehaviour 
{

	ActionsCollectionScript script;
	float startTime;


	// Use this for initialization
	void StartSimulation(ActionsCollectionScript playerActions) 
	{
		script = playerActions;
		startTime = Time.fixedTime;
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		var time = Mathf.RoundToInt(startTime - Time.fixedTime);
		var action = script.getAction(time);
		if(action!=null)
		{
			action.getAction().simulate(action.getOrigin(), action.getDestination(), action.getDuration());
		}
	}
}
