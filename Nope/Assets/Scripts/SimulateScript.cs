﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Store and launch actions
public class SimulateScript : MonoBehaviour 
{

    private int currentActionsIndex;
    private List<ActionScript> actions;
    private bool ended;

    // Initialise the script
    public void Start()
    {
        actions = new List<ActionScript>();
    }

    // Add an action to the simulation
    public void addAction(ActionScript action)
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
    public void StartSimulation()
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
