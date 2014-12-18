using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    [SerializeField]
    private NetworkScript networkScript;
    [SerializeField]
    private Camera camera;

    private bool positionSet;
    private Vector3 positionOnGame;
    private Vector3 positionOnScreen;
    private SimulateScript selectedPlayer; // TODO set selected player
    private bool ended;
    private bool win; 

    void Start()
    {
        ended = false;
        win = false;
    }

    // Update is called once per frame setReadyToSimulate
	void Update () 
    {
        if (Network.isClient && !ended)
        {
            if (Input.GetMouseButtonDown(2))
            {
                if (selectedPlayer != null && !positionSet)
                {
                    positionSet = true;
                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        hit.collider.renderer.material.color = Color.red;
                        positionOnGame = hit.point; // TODO get mousePositionOnGame
                        positionOnScreen = new Vector2(0,0); // TODO get mousePositionOnScreen

                    }
                }
                else
                {

                    Ray ray = camera.ScreenPointToRay(Input.mousePosition);
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
            if(Input.touchCount > 0)
            { 
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    if (selectedPlayer != null && !positionSet)
                    {
                        positionSet = true;
                        Ray ray = camera.ScreenPointToRay(touch.position);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            hit.collider.renderer.material.color = Color.red;
                            positionOnGame = hit.point; // TODO get mousePositionOnGame
                            positionOnScreen = new Vector2(0, 0); // TODO get mousePositionOnScreen

                        }
                    }
                    else
                    {

                        Ray ray = camera.ScreenPointToRay(touch.position);
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
	}



    void OnGUI()
    {
        if (Network.isClient)
        {
            if (!ended)
            {
                if (positionSet)
                {
                    if (GUI.Button(new Rect(0, 0, 80, 20), "WalkAction"))
                    {
                        WalkActionScript action = new WalkActionScript(positionOnGame, -1);
                        selectedPlayer.addActionToAll(action);
                        positionSet = false;
                    }
                    if (GUI.Button(new Rect(0, 20, 80, 20), "WeaponAction"))
                    {
                        WeaponActionScript action = new WeaponActionScript(positionOnGame, -1);
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
            else
            {
                if (win)
                    GUI.Label(new Rect(0, 0, 100, 20), "You win ! :)");
                else
                    GUI.Label(new Rect(0, 0, 100, 20), "You lose ! :'(");
            }
        }
    }

    public void setWin()
    {
        ended = true;
        win = true;
    }
    public void setLose()
    {
        ended = true;
    }

}
