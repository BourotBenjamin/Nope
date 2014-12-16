﻿using UnityEngine;
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
        if (Network.isClient)
        {
            if (Input.GetMouseButtonDown(2))
            {
                if (selectedPlayer != null && !positionSet)
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
                        if (hit.collider.tag == "Player")
                        {
                            SimulateScript ss = hit.collider.GetComponent<SimulateScript>();
                            if (hit.collider.GetComponent<SimulateScript>().owner == Network.player)
                            {
                                //Debug.LogError(playerSelected.networkView.group);
                                hit.collider.renderer.material.color = Color.blue;
                                if (selectedPlayer != null)
                                    selectedPlayer.transform.renderer.material.color = Color.white;
                                selectedPlayer = ss;
                            }
                        }
                    }
                }
            }
        }
	}



    void OnGUI()
    {
       if (Network.isClient)
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
                if (GUI.Button(new Rect(0, 0, 80, 20), "FIGHT !!"))
                {
                    Debug.LogError("I am ready");
                    networkScript.setReadyToSimulate();
                }
            }
       }
    }

}
