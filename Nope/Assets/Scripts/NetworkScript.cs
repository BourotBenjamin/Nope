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

    public void setReadyToSimulate()
    {

        networkView.RPC("setPlayerReadyToSimulate", RPCMode.Server, Network.player);
    }

    [RPC]
    private void setPlayerReadyToSimulate(NetworkPlayer player)
    {
        if (player.ToString() == "1")
            playerOneIsReadyToSimulate = true;
        else if (player.ToString() == "2")
            playerTwoIsReadyToSimulate = true;

        if(playerTwoIsReadyToSimulate && playerOneIsReadyToSimulate)
        {
            playerOne.SendSimulation();
            playerTwo.SendSimulation();
            playerOne.Simulate();
            playerTwo.Simulate();
        }
    }

}
