using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    [SerializeField]
    private NetworkScript networkScript;

    private bool positionSet;
    private Vector3 positionOnGame;
    private Vector2 positionOnScreen;
    private SimulateScript selectedPlayer; // TODO set selected player

    // Update is called once per frame setReadyToSimulate
	void Update () 
    {
        if (selectedPlayer!=null && !positionSet && Input.GetButton("Click"))
        {
            positionSet = true;
            positionOnGame = new Vector3(0, 0, 0); // TODO get mousePositionOnGame
            positionOnScreen = new Vector2(0, 0); // TODO get mousePositionOnScreen
        }
	}



    void OnGUI()
    {
        if (positionSet)
        {
            if (GUI.Button(new Rect(positionOnScreen.x, positionOnScreen.y, 80, 20), "WalkAction"))
            {
                WalkActionScript action = new WalkActionScript(positionOnGame, -1);
                selectedPlayer.addActionToAll(action);
                positionSet = false;
            }
        }
        else
        {
            if (GUI.Button(new Rect(positionOnScreen.x, positionOnScreen.y, 80, 20), "FIGHT !!"))
            {
                networkScript.setReadyToSimulate();
            }
        }
    }

}
