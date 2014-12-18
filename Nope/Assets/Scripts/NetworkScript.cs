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
    private GUIScript gui;

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
        Debug.LogError(Network.player + " ready");
    }

    [RPC]
    private void setPlayerReadyToSimulate(NetworkPlayer player)
    {
        if (playerOne.owner == player)
        {
            playerOneIsReadyToSimulate = true;
            Debug.LogError("1 is ready so I wait");
        }
        else if (playerTwo.owner == player)
        {
            playerTwoIsReadyToSimulate = true;
            Debug.LogError("2 is ready so I wait");
        }

        if(playerTwoIsReadyToSimulate && playerOneIsReadyToSimulate)
        {
            Debug.LogError("Everyone is ready");
            playerOne.SendSimulation();
            playerTwo.SendSimulation();
            playerOne.Simulate();
            playerTwo.Simulate();
            playerTwoIsReadyToSimulate = false;
            playerOneIsReadyToSimulate = false;
        }
    }

}
