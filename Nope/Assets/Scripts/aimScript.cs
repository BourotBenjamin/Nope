using UnityEngine;
using System.Collections;

public class aimScript : MonoBehaviour {

    private RangeScript attackRange;
    float sizex;
    float sizez;
    Vector3 pos;
    bool started = false;
    bool first = false;
    GameObject rangeIndicator;
    Vector3 point;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (started)
        {
            if(!first)
            {
                rangeIndicator = attackRange.addRangeAttack(sizex, sizez, pos);
                first = true;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                point = hit.point; // ray.origin + (ray.direction * 15);
                point.y = 0f;
                Vector3 v = transform.position - point;
                v.y = 0;
                transform.rotation = Quaternion.LookRotation(v);
                //rangeIndicator.transform.localPosition =transform.position + Vector3.ClampMagnitude((point - transform.position),1f);
            }
        }
        
       
	}

    public void setValues(RangeScript r, float sizeX, float sizeZ, Vector3 pos)
    {
        attackRange = r;
        sizex = sizeX;
        sizez = sizeZ;
        this.pos = pos;
        started = true;

    }

    public GameObject aimDone()
    {
        started = false;
        //Destroy(rangeIndicator);
        //rangeIndicator.transform.localScale = new Vector3(0.2f, 0, 1.0f);
        
        return rangeIndicator;
    }

}
