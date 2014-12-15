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
        Debug.LogError(Network.player + " ready");
    }

    [RPC]
    private void setPlayerReadyToSimulate(NetworkPlayer player)
    {
        if (player.ToString().Equals("0"))
        {
            playerOneIsReadyToSimulate = true;
            Debug.LogError("0 is ready so I wait");
        }
        else if (player.ToString().Equals("1"))
        {
            playerTwoIsReadyToSimulate = true;
            Debug.LogError("0 is ready so I wait");
        }
        else
        {
            Debug.LogError(Network.player.ToString());
        }

        if(playerTwoIsReadyToSimulate && playerOneIsReadyToSimulate)
        {
            Debug.LogError("Everyone is ready");
            playerOne.SendSimulation();
            playerTwo.SendSimulation();
            playerOne.Simulate();
            playerTwo.Simulate();
        }
    }

}
