﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Store and launch actions
public class SimulateScript : MonoBehaviour 
{
    private int currentActionsIndex;
    private List<ActionScript> actions;
    private bool ended;
    private bool animating;
    private int _id;
    private ActionScript currentAction = null;
    private PlayerScript player;
    private int nbActions;
    [SerializeField]
    private List<string> _enabledActions;
    [SerializeField]
    private CharactersAttributes life;
    public List<string> enabledActions
    {
        get { return _enabledActions; }
        set { _enabledActions = value; }
    }
    NetworkView _nV;

    [SerializeField]
    private NetworkPlayer _owner;
    public NetworkPlayer owner
    {
        get { return _owner; }
        set { _owner = value; }
    }

    public int id
    {
        get { return _id; }
        set { _id = value; }
    }

    public bool isInWaitingState()
    {
        return !ended && !animating;
    }

    public void setPlayer(PlayerScript player)
    {
        this.player = player;
    }

    // Initialise the script
    public void Awake()
    {
        life = this.GetComponent<CharactersAttributes>();
        actions = new List<ActionScript>();
        nbActions = 0;
        ended = false;
        animating = false;
    }

    public int getNBActions()
    {
        return nbActions;
    }

    // Add an action to the simulation
    public void addActionToAll(ActionScript action)
    {
        this.networkView.RPC("addAction", RPCMode.Server, action.getArrayOfParams());
        //addAction(action.getName(), action.getDestination(), action.getDuration());
        //_nV.RPC("addActionp", RPCMode.Server, action.GetType(), action.getDestination(), action.getDuration());
        //addActionp(action.GetType(), action.getDestination(), action.getDuration());
        nbActions++;
    }
    public void setOwnerInSimulateScript(NetworkPlayer p)
    {
        this.networkView.RPC("setOwnerInClient", p, p);
    }

    [RPC]
    void setOwnerInClient(NetworkPlayer p)
    {
        owner = p;
    }

    public void sendActionToAll()
    {
        foreach(ActionScript action in this.actions)
        {
            this.networkView.RPC("addAction", RPCMode.Others, action.getArrayOfParams());
        }
    }

    /*[RPC]
    private void addActionp(System.Type actionType, Vector3 destination, int duration)
    {
        ActionScript action = null;
        foreach (ActionScript a in enabledActions)
        {
            if(a.GetType().Equals(actionType))
            {
                action = a;
                break;
            }
        }
        action.setSimulation(this);
        actions.Add(action);
    }*/

    [RPC]
    private void addAction(string actionName, Vector3 destination, int duration)
    {
        ActionScript action = null;
        foreach (string s in enabledActions)
        {
            if (s == actionName)
            {
                System.Type type = System.Type.GetType(s);
                object o = System.Activator.CreateInstance(type);
                action = (ActionScript)o;
                action.destination = destination;
                break;
            }
        }
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
        animating = true;
        currentActionsIndex = -1;
        simulateActionAtNextIndex();
        nbActions = 0;
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
            else
            {
                animating = false;
                if (Network.isServer)
                {
                    player.SimulationEnded();
                }
                actions.Clear();
            }
        }
        else if (Network.isServer)
        {
            player.SimulationEnded();
        }
	}

    // Stops the simulation
    public void stopActions()
     {
        animating = false;
        ended = true;
        currentAction = null;
        if (currentActionsIndex >= 0)
        {
            var action = this.getAction(currentActionsIndex);
            if (action != null && action.getStarted())
            {
                action.endSimulation();
            }
        }
        actions.Clear();
        if (Network.isServer)
        {
            player.SimulationEnded();
        }
     }

    [RPC]
    public void warriorHurt(int hp)
    {
        if (life.hurt(hp))
        {
            if (Network.isServer)
            {
                player.EntityDied(this.gameObject);
            }
            this.stopActions();
            Destroy(this.gameObject);
        }
    }

    [RPC]
    public void warriorHealed(NetworkPlayer healerPlayer)
    {
        if (healerPlayer == this.owner)
        {
            life.heal();
        }
    }

    void FixedUpdate()
    {
        if (currentAction != null)
        {
            currentAction.doAction(this.transform, this.rigidbody);
        }
    }
	
}
