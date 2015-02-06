using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

    [SerializeField]
    private NetworkScript networkScript;
    [SerializeField]
    private Camera camera;

    private bool positionSet;
    private Vector3 positionOnGame;
    private SimulateScript selectedPlayer;
    private bool ended;
    private bool win;
    private enum selected {SelectPlayer, ClickOnGui, SetDestination};
    private Collider pointed;
    private planeRangeScript rangeView;
    private selected clickState;
    private ActionScript _action;
    public ActionScript action
    {
        get { return _action; }
        set { _action = value; }
    }

    void Start()
    {
        ended = false;
        win = false;
        clickState = selected.SelectPlayer;
        
    }

    private void setDestRaycast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                pointed = hit.collider;
                hit.collider.renderer.material.color = Color.red;
            }
            positionOnGame = hit.point;
            setDestinationToAction(positionOnGame);

            clickState = selected.SelectPlayer;
        }
    }

    private void setSelectionRaycast(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                
                SimulateScript ss = hit.collider.GetComponent<SimulateScript>();
                if (hit.collider.GetComponent<SimulateScript>().owner == Network.player)
                {
                    clickState = selected.ClickOnGui;
                    hit.collider.renderer.material.color = Color.blue;
                    if (selectedPlayer != null)
                        selectedPlayer.transform.renderer.material.color = Color.white;
                    selectedPlayer = ss;
                    rangeView = hit.collider.GetComponent<planeRangeScript>();
                    rangeView.addRange(2, 2, new Vector3(hit.point.x, 0.17f, hit.point.z));
                }
            }
        }
    }

    private void setDestinationToAction(Vector3 dest)
    {
        selectedPlayer.GetComponent<AnimationCharacters>().sendAnimationToAll(dest);
        selectedPlayer.transform.renderer.material.color = Color.white;
        action.destination = dest;
        selectedPlayer.addActionToAll(action);
        positionSet = false;
        selectedPlayer = null;
        if (pointed != null)
            pointed.transform.renderer.material.color = Color.white;
    }

    private void mouseControl()
    {
        if (selectedPlayer != null && !positionSet && (clickState == selected.SetDestination))
        {
            positionSet = true;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            setDestRaycast(ray);
        }
        else if (clickState == selected.SelectPlayer)
        {

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            setSelectionRaycast(ray);
        }
    }

    private void touchControl()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            if (selectedPlayer != null && !positionSet)
            {
                positionSet = true;
                Ray ray = camera.ScreenPointToRay(touch.position);
                setDestRaycast(ray);
            }
            else
            {

                Ray ray = camera.ScreenPointToRay(touch.position);
                setSelectionRaycast(ray);
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

    // Update is called once per frame setReadyToSimulate
	void Update () 
    {
        if (Network.isClient && !ended && (!selectedPlayer || selectedPlayer.isInWaitingState()))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseControl();
            }
            if(Input.touchCount > 0)
            {
                touchControl();
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
                    int i  = 20;
                    if (selectedPlayer != null && selectedPlayer.getNBActions() >= 5)
                    {
                        positionSet = false;
                        GUI.Label(new Rect(0, 20, 1000, 20), "You have too much actions.");
                    }
                    else if (clickState == selected.ClickOnGui)
                    {
                        action = null;
                        
                        foreach (string s in selectedPlayer.enabledActions)
                        {
                            if (GUI.Button(new Rect(0, i, 120, 20),s))
                            {
                                System.Type type = System.Type.GetType(s);
                                object o = System.Activator.CreateInstance(type);
                                action = (ActionScript)o;
                            }
                            if (action != null) 
                            {
                                clickState = selected.SetDestination;
                                break;
                            }
                            
                            i += 20;
                        }/*
                        if (GUI.Button(new Rect(0, 20, 120, 20), "WalkAction"))
                        {
                            action = new WalkActionScript();
                            clickState = selected.SetDestination;
                        }
                        if (GUI.Button(new Rect(0, 40, 120, 20), "WeaponAction"))
                        {
                            action = new WeaponActionScript();
                            clickState = selected.SetDestination;
                        }*/
                        if (GUI.Button(new Rect(0, i, 120, 20), "Cancel"))
                        {
                            selectedPlayer.transform.renderer.material.color = Color.white;
                            selectedPlayer = null;
                            clickState = selected.SelectPlayer;
                        }
                    }
                    if (GUI.Button(new Rect(0, 0, 120, 20), "FIGHT !!"))
                    {
                        Debug.LogError("I am ready");
                        networkScript.setReadyToSimulate();
                        clickState = selected.SelectPlayer;
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



}
