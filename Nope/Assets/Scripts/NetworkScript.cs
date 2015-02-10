﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkScript : MonoBehaviour 
{

    private bool playerOneIsReadyToSimulate;
    private bool playerTwoIsReadyToSimulate;

    [SerializeField]
    private PlayerScript playerOne;
    [SerializeField]
    private PlayerScript playerTwo;
    private GUIScript gui;

    public bool playerOneIsSimulating;
    public bool playerTwoIsSimulating;
    public bool isSimulating;

    void Start()
    {
        gui = this.gameObject.GetComponent<GUIScript>();
    }

    [RPC]
    public void PlayerDied(NetworkPlayer owner)
    {
        Time.timeScale = 0;
        playerOne.stopGame();
        playerTwo.stopGame();
        if(owner == Network.player)
        {
            gui.setLose();
        }
        else
        {
            gui.setWin();
        }
    }

    public void setReadyToSimulate()
    {
        networkView.RPC("setPlayerReadyToSimulate", RPCMode.Server, Network.player);
    }

    [RPC]
    private void setPlayerReadyToSimulate(NetworkPlayer player)
    {
        if (playerOne.owner == player)
        {
            playerOneIsReadyToSimulate = true;
        }
        else if (playerTwo.owner == player)
        {
            playerTwoIsReadyToSimulate = true;
        }

        if(playerTwoIsReadyToSimulate && playerOneIsReadyToSimulate)
        {
            playerOneIsSimulating = true;
            playerTwoIsSimulating = true;
            isSimulating = true;
            playerOne.SendSimulation();
            playerTwo.SendSimulation();
            playerOne.Simulate();
            playerTwo.Simulate();
            playerTwoIsReadyToSimulate = false;
            playerOneIsReadyToSimulate = false;
        }
    }

    public void SimulationEnded(PlayerScript player)
    {
        if (player == playerOne)
            playerOneIsSimulating = false;
        if (player == playerTwo)
            playerTwoIsSimulating = false;
        if (!playerOneIsSimulating && !playerTwoIsSimulating)
            isSimulating = false;
    }

}
