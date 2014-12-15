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
    private ActionScript currentAction = null;

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
        this.networkView.RPC("addAction", RPCMode.Server, action.getArrayOfParams());
        addAction(action.getName(), action.getDestination(), action.getDuration());
    }

    public void sendActionToAll()
    {
        object[] actions = new object[this.actions.Count];
        int i = 0;
        foreach(ActionScript action in this.actions)
        {
            actions[i] = action.getArrayOfParams();
            i++;
        }
        this.networkView.RPC("setActions", RPCMode.All, actions);
    }

    [RPC]
    public void setActions(object[] actions)
    {
        this.actions.Clear();
        foreach(object[] action in actions)
        {
            WalkActionScript actionObject = new WalkActionScript((Vector3) action[1], (int) action[2]);
            this.actions.Add(actionObject);
        }
    }

    [RPC]
    private void addAction(string actionName, Vector3 destination, int duration)
    {
        WalkActionScript action = new WalkActionScript(destination, duration); 
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
        Debug.LogError("Start");
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
                currentAction = action;
                action.simulate(action.getDestination(), action.getDuration());
            }
        }
	}

    // Stops the simulation
    public void stopActions()
     {
        ended = true;
        var action = this.getAction(currentActionsIndex);
        if (action != null && action.getStarted())
        {
            currentAction = null;
            action.endSimulation();
        }
     }

    void FixedUpdate()
    {
        if (currentAction != null)
            currentAction.FixedUpdate(this.transform);
    }
	
}
