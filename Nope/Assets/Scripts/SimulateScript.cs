using UnityEngine;
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
    private float startTime;
    private int nbActions;

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
    public void Start()
    {
        _nV = this.GetComponent<NetworkView>();
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
        _nV.RPC("addAction", RPCMode.Server, action.getArrayOfParams());
        addAction(action.getName(), action.getDestination(), action.getDuration());
        nbActions++;
    }
    public void setOwnerInSimulateScript(NetworkPlayer p)
    {
        _nV.RPC("setOwnerInClient", p, p);
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
            _nV.RPC("addAction", RPCMode.Others, action.getArrayOfParams());
        }
    }

    [RPC]
    private void addAction(string actionName, Vector3 destination, int duration)
    {
        ActionScript action = null;
        switch(actionName)
        {
            case "WalkActionScript":
                action = new WalkActionScript(destination, duration);
                break;
            case "WeaponActionScript":
                action = new WeaponActionScript(destination, duration, player.fireBallPrefab);
                break;
            case "MineDetectorActionScript":
                action = new MineDetectorActionScript(destination, duration);
                break;
            case "TrapActionScript":
                action = new TrapActionScript(destination, duration, player.minePrefab);
                break;
            case "StandActionScript":
                action = new StandActionScript(destination, duration);
                break;
            case "BowActionScript":
                action = new BowActionScript(destination, duration, player.arrowPrefab);
                break;
            case "MeteorActionScript":
                action = new MeteorActionScript(destination, duration, player.meteorPrefab);
                break;
            case "SwordActionScript":
                action = new SwordActionScript(destination, duration);
                break;
            default:
                return;
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
        startTime = Time.time;
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
                player.SimulationEnded();
                actions.Clear();
            }
        }
	}

    // Stops the simulation
    public void stopActions()
     {
        animating = false;
        ended = true;
        var action = this.getAction(currentActionsIndex);
        if (action != null && action.getStarted())
        {
            currentAction = null;
            action.endSimulation();
        }
        player.SimulationEnded();
     }

    [RPC]
    public void warriorDies()
    {
        this.stopActions();
        player.EntityDied(this.gameObject);
    }

    void FixedUpdate()
    {
        if (currentAction != null)
        {
            currentAction.FixedUpdate(this.transform, this.rigidbody);
        }
    }
	
}
