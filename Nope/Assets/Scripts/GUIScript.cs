using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    [SerializeField]
    private NetworkScript networkScript;
    [SerializeField]
    private Camera camera;

    private bool positionSet;
    private Vector3 positionOnGame;
    private SimulateScript selectedPlayer; // TODO set selected player
    private bool ended;
    private bool win;
    private Collider pointed;

    void Start()
    {
        ended = false;
        win = false;
    }

    // Update is called once per frame setReadyToSimulate
	void Update () 
    {
        if (Network.isClient && !ended && (!selectedPlayer || selectedPlayer.isInWaitingState()))
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
                        if (hit.collider.tag == "Player")
                        {
                            pointed = hit.collider;
                            hit.collider.renderer.material.color = Color.red;
                        }
                        positionOnGame = hit.point; // TODO get mousePositionOnGame
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

                            if (hit.collider.tag == "Player")
                            {
                                pointed = hit.collider;
                                hit.collider.renderer.material.color = Color.red;
                            }
                            //hit.collider.renderer.material.color = Color.red;
                            positionOnGame = hit.point; 
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
                if (selectedPlayer == null || selectedPlayer.isInWaitingState())
                {
                    if (selectedPlayer != null && selectedPlayer.getNBActions() >= 5)
                    {
                        positionSet = false;
                        GUI.Label(new Rect(0, 20, 1000, 20), "You have too much actions.");
                    }
                    else if (positionSet)
                    {
                        if (GUI.Button(new Rect(0, 20, 120, 20), "WalkAction"))
                        {
                            selectedPlayer.transform.renderer.material.color = Color.white;
                            WalkActionScript action = new WalkActionScript(positionOnGame, -1);
                            selectedPlayer.addActionToAll(action);
                            positionSet = false;
                            selectedPlayer = null;
                            if(pointed != null)
                                pointed.transform.renderer.material.color = Color.white;
                        }
                        if (GUI.Button(new Rect(0, 40, 120, 20), "WeaponAction"))
                        {
                            WeaponActionScript action = new WeaponActionScript(positionOnGame, -1);
                            selectedPlayer.addActionToAll(action);
                            positionSet = false;
                            selectedPlayer = null;
                            selectedPlayer.transform.renderer.material.color = Color.white;
                            if (pointed != null)
                                pointed.transform.renderer.material.color = Color.white;
                        }
                        if (GUI.Button(new Rect(0, 60, 120, 20), "Cancel"))
                        {
                            selectedPlayer.transform.renderer.material.color = Color.white;
                            selectedPlayer = null;
                        }
                    }
                    if (GUI.Button(new Rect(0, 0, 120, 20), "FIGHT !!"))
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
