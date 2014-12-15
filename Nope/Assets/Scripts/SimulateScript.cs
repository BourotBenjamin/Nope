using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Store and launch actions
public class SimulateScript : MonoBehaviour 
{

    private int currentActionsIndex;
    private List<ActionScript> actions;
    private bool ended;
    private int _id;
    public int id
    {
        get { return _id; }
        set { _id = value; }
    }
    // Initialise the script
    public void Start()
    {
        actions = new List<ActionScript>();
    }

    // Add an action to the simulation
    public void addActionToAll(ActionScript action)
    {
        this.networkView.RPC("addAction", RPCMode.Server, action);
        addAction(action);
    }

    public void sendActionToAll()
    {
        this.networkView.RPC("setActions", RPCMode.All, actions);
    }

    [RPC]
    public void setActions(List<ActionScript> actions)
    {
        this.actions = actions;
    }

    [RPC]
    private void addAction(ActionScript action)
    {
        action.setSimulation(this);
        actions.Add(action);
    }

    private ActionScript getAction(int index)
    {
        if (actions.Count > index)
        {
            return actions[index];
        }
        else
        {
            return null;
        }
    }

    // Starts simulation
    public void startAllSimulations()
    {
        this.networkView.RPC("startSimulation", RPCMode.All);
    }

    // Starts simulation
    [RPC]
    public void startSimulation()
    {
        ended = false;
        currentActionsIndex = -1;
        simulateActionAtNextIndex();
    }

    // Starts the next action
     public void simulateActionAtNextIndex()
    {
        if (!ended)
        {
            currentActionsIndex += 1;
            var action = this.getAction(currentActionsIndex);
            if (action != null && !action.getStarted())
            {
                action.simulate(action.getDestination(), action.getDuration());
            }
        }
	}

    // Stops the simulation
    public void stopActions()
     {
        ended = true;
        var action = this.getAction(currentActionsIndex);
        if (action != null && !action.getStarted())
        {
            action.endSimulation();
        }
     }
	
}
