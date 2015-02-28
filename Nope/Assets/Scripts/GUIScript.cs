using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GUIScript : MonoBehaviour {

    [SerializeField]
    private NetworkScript networkScript;
    [SerializeField]
    private Camera camera;

    [SerializeField]
    RectTransform parentCanvas;

    [SerializeField]
    GameObject buttonPrefab;

    private bool positionSet;
    private SimulateScript selectedPlayer;
    private enum selected {SelectPlayer, ClickOnGui, SetDestination};
    private Collider pointed;
    private RangeScript rangeView;
    private CharactersAttributes rangeAttribute;
    private GameObject plane;
    private GameObject go;
    private aimScript aimScript;
    private Vector3 pullMarker;
    private float rayRange;
    private selected clickState;
    private ActionScript _action;
    private Vector3 positionOnGame;
    private List<GameObject> marker;
    public ActionScript action
    {
        get { return _action; }
        set { _action = value; }
    }

    void Awake()
    {
        //buttonList = new List<GameObject>();
        marker = new List<GameObject>();
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
            if (action.getName() == "WalkActionScript")
            {

                Vector3 newPos = positionOnGame - selectedPlayer.transform.position;
                newPos.y = 0;
                rayRange = rangeView.getCircleRay();
                positionOnGame = selectedPlayer.transform.position + Vector3.ClampMagnitude(newPos, rangeAttribute.mobilityRange);
                positionOnGame.y = 0.17f;
                rangeView.deleteRange();
                rangeView.addPointDest(positionOnGame);

            }
            else if (action.getName() == "WeaponActionScript")
            {   
                go = aimScript.aimDone();
                go.transform.localScale = new Vector3(0.2f, 0, 1.0f);
                marker.Add(go);
                positionOnGame.y = 0.17f;
            }
            setDestinationToAction(positionOnGame);
            clickState = selected.SelectPlayer;
            for (int i = 0; i < parentCanvas.childCount; i++)
            {
                Destroy(parentCanvas.GetChild(i).gameObject);
            }
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
                    aimScript = hit.collider.GetComponent<aimScript>();
                    displayGUI();
                }
            }
        }
    }

    private void setDestinationToAction(Vector3 dest)
    {
        // selectedPlayer.GetComponent<AnimationCharacters>().sendAnimationToAll(dest);
        action.destination = dest;
        if (pointed != null)
            pointed.transform.renderer.material.color = Color.white;
        addActionToPlayer();
    }

    private void addActionToPlayer()
    {
        selectedPlayer.addActionToAll(action);
        selectedPlayer.transform.renderer.material.color = Color.white;
        positionSet = false;
        selectedPlayer = null;
        //deleteGUI();
        clickState = selected.SelectPlayer;
    }

    private void mouseControl()
    {
        if (!networkScript.isSimulating)
        { 
            if (selectedPlayer != null && !positionSet && clickState == selected.SetDestination)
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
    }

    private void touchControl()
    {
        if (!networkScript.isSimulating)
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
    }

    // Update is called once per frame setReadyToSimulate
	void Update () 
    {
        if (Network.isClient && (!selectedPlayer || selectedPlayer.isInWaitingState()))
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

    void deleteGUI(bool resetPlayerColor)
    {
        for (int i = 0; i < parentCanvas.childCount; i++)
        {
            Destroy(parentCanvas.GetChild(i).gameObject);
        }
        if(resetPlayerColor)
        {
            selectedPlayer.renderer.material.color = Color.white;
            clickState = selected.SelectPlayer;
        }
    }

    void displayGUI()
    {
        if (selectedPlayer != null && selectedPlayer.getNBActions() < 5)
        {
            for (int i = 0; i < parentCanvas.childCount; i++)
            {
                Destroy(parentCanvas.GetChild(i).gameObject);
            }
            int j = 0;
            int length = selectedPlayer.enabledActions.ToArray().Length + 1;
            foreach (string s in selectedPlayer.enabledActions)
            {
                GameObject g = (GameObject)Instantiate(buttonPrefab);
                var cacheScript = g.GetComponent<GUIButtonScript>();
                cacheScript.MainRectTransform.SetParent(parentCanvas);
                cacheScript.MainRectTransform.localPosition = Vector3.zero;
                cacheScript.MainRectTransform.anchorMin = new Vector3(0f, 1f - (j + 1) / (float)length);
                cacheScript.MainRectTransform.anchorMax = new Vector3(1f, 1f - j / (float)length);
                cacheScript.MainRectTransform.offsetMin = new Vector3(0f, 0f);
                cacheScript.MainRectTransform.offsetMax = new Vector3(0f, 0f);
                cacheScript.MainRectTransform.localScale = Vector3.one;
                cacheScript.Text.text = s;
                j++;
                var str = s;
                //var num = i; // this is done in order to prevent variable scoping bug in lambdas defined in a loop in Mono version < 4
                cacheScript.ButtonScript.onClick.AddListener(() => clickButton(str, selectedPlayer.gameObject));
            }
            var btn = ((GameObject)Instantiate(buttonPrefab)).GetComponent<GUIButtonScript>();
            btn.MainRectTransform.SetParent(parentCanvas);
            btn.MainRectTransform.localPosition = Vector3.zero;
            btn.MainRectTransform.anchorMin = new Vector3(0f, 1f - (j + 1) / (float)length);
            btn.MainRectTransform.anchorMax = new Vector3(1f, 1f - j / (float)length);
            btn.MainRectTransform.offsetMin = new Vector3(0f, 0f);
            btn.MainRectTransform.offsetMax = new Vector3(0f, 0f);
            btn.MainRectTransform.localScale = Vector3.one;
            btn.Text.text = "Cancel";
            btn.ButtonScript.onClick.AddListener(() => deleteGUI(true));
        }
        else
        {
            selectedPlayer.renderer.material.color = Color.white;
            clickState = selected.SelectPlayer;
        }
    }

    void clickButton(string s, GameObject selectedPlayer)
    {
        System.Type type = System.Type.GetType(s);
        object o = System.Activator.CreateInstance(type);
        action = (ActionScript)o;
        if (action.isDestinationNeeded())
        {
            if (action.getName() == "WalkActionScript")
            {
                rangeView = selectedPlayer.GetComponent<RangeScript>();
                rangeAttribute = selectedPlayer.GetComponent<CharactersAttributes>();
                rangeView.addRange(rangeAttribute.mobilityRange, rangeAttribute.mobilityRange, new Vector3(selectedPlayer.transform.position.x, 0.15f, selectedPlayer.transform.position.z));
            }
            if (action.getName() == "WeaponActionScript")
            {
                rangeAttribute = selectedPlayer.GetComponent<CharactersAttributes>();
                rangeView = selectedPlayer.GetComponent<RangeScript>();
                aimScript.setValues(rangeView, 1f, rangeAttribute.attackRange, new Vector3(selectedPlayer.transform.position.x, 0.15f, selectedPlayer.transform.position.z));
            }
            clickState = selected.SetDestination;
        }
        else
        {
            positionSet = true;
            addActionToPlayer();
        }
        deleteGUI(false);
    }

    public void fight()
    {
        if (!networkScript.isSimulating && !networkScript.isWaiting)
        {
            foreach (GameObject g in marker)
            {
                Destroy(g);
            }
            networkScript.setReadyToSimulate();
            clickState = selected.SelectPlayer;
        }
    }


}
