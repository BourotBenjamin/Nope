using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    [SerializeField]
    private NetworkScript networkScript;

    private bool positionSet;
    private Vector3 positionOnGame;
    private Vector3 positionOnScreen;
    private SimulateScript selectedPlayer; // TODO set selected player

    // Update is called once per frame setReadyToSimulate
	void Update () 
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (selectedPlayer!=null && !positionSet)
            { 
                positionSet = true;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    hit.collider.renderer.material.color = Color.red;
                    positionOnGame = hit.point; // TODO get mousePositionOnGame
                    positionOnScreen = Input.mousePosition; // TODO get mousePositionOnScreen
                }
            }
            else
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.tag == "Player")
                    {
                        hit.collider.renderer.material.color = Color.blue;
                        selectedPlayer.transform.renderer.material.color = Color.white;
                        selectedPlayer = hit.collider.GetComponent<SimulateScript>();
                    }
                }
            }
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
