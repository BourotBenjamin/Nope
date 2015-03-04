using UnityEngine;
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
    [SerializeField]
    private GUIButtonScript fightButton;

    public bool playerOneIsSimulating;
    public bool playerTwoIsSimulating;
    public bool isSimulating;
    public bool isWaiting;

    void OnConnectedToServer()
    {
        fightButton.gameObject.SetActive(true);
    }

    [RPC]
    public void PlayerDied(NetworkPlayer owner)
    {
        Time.timeScale = 0;
        playerOne.stopGame();
        playerTwo.stopGame();
        if(owner == Network.player)
        {
            Application.LoadLevel("winScene");
        }
        else
        {
            Application.LoadLevel("loseScene");
        }
    }

    public void setReadyToSimulate()
    {
        isWaiting = true;
        networkView.RPC("setPlayerReadyToSimulate", RPCMode.All, Network.player);
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
            playerTwoIsReadyToSimulate = false;
            playerOneIsReadyToSimulate = false;
            if(Network.isServer)
            {
                playerOne.SendSimulation();
                playerTwo.SendSimulation();
                playerOne.Simulate();
                playerTwo.Simulate();
            }
            else
            {
                isWaiting = false;
            }
        }
    }

    public void SimulationEnded(PlayerScript player)
    {
        if (player == playerOne)
            playerOneIsSimulating = false;
        if (player == playerTwo)
            playerTwoIsSimulating = false;
        if (!playerOneIsSimulating && !playerTwoIsSimulating)
        {
            isSimulating = false;
            isWaiting = false;
        }
    }

    public void SimulationStarted(PlayerScript player)
    {
        if (player == playerOne)
            playerOneIsSimulating = true;
        if (player == playerTwo)
            playerTwoIsSimulating = true;
        isSimulating = true;
    }

}
