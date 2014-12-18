using UnityEngine;
using System.Collections;

public class AnimationCharacters : MonoBehaviour {

    private SimulateScript simulation;
    private NetworkView _nV;
    private Animator animator;
    private Rigidbody rigid;
    private Vector3 _direction;
    public Vector3 direction
    {
        get { return _direction; }
        set { _direction = value; }
    }
    private Vector3 _positionToGo;
    public Vector3 positionToGo
    {
        get { return _positionToGo; }
        set { _positionToGo = value; }
    }
    // Use this for initialization
    void Start()
    {
        _nV = this.GetComponent<NetworkView>();
        simulation = this.GetComponent<SimulateScript>();
        animator = this.GetComponent<Animator>();
        rigid = this.rigidbody;
    }
    public void sendAnimationToAll(Vector3 direction)
    {
        _nV.RPC("setAnimationInOtherPlayers", RPCMode.All, simulation.id, _nV.viewID, direction);
    }
    [RPC]
    public void setAnimationInOtherPlayers(int characterID, NetworkViewID nwvID, Vector3 direction)
    {

        if (simulation.id == characterID && nwvID == _nV.viewID)
        {
            setAnimation(direction);
        }

    }
	// Update is called once per frame
	public void setAnimation (Vector3 direction) {
        positionToGo = rigid.position + direction;
        Vector3 temp = direction - transform.position;
        temp.y = 0f;
        this.direction = temp;
        // droite
        if (direction.x > 0 && direction.z > 0 && direction.x > direction.z)
        {
            animator.SetInteger("Direction", 3);
        }
        //droite
        else if (direction.x > 0 && direction.z < 0 && direction.x > Mathf.Abs(direction.z))
        {
            animator.SetInteger("Direction", 3);
        }
        //gauche
        else if (direction.x < 0 && direction.z < 0 && Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            animator.SetInteger("Direction", 1);
        }
        //gauche
        else if (direction.x < 0 && direction.z > 0 && Mathf.Abs(direction.x) > direction.z)
        {
            animator.SetInteger("Direction", 1);
        }

        //haut
        else if (direction.z > 0 && direction.x < 0 && Mathf.Abs(direction.x) < direction.z)
        {
            animator.SetInteger("Direction", 2);
        }
        //haut
        else if (direction.z > 0 && direction.x > 0 && direction.x < direction.z)
        {
            animator.SetInteger("Direction", 2);
        }
        //bas
        else if (direction.z < 0 && direction.x < 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.z))
        {
            animator.SetInteger("Direction", 0);
        }
        //bas
        else if (direction.z < 0 && direction.x > 0 && direction.x < Mathf.Abs(direction.z))
        {
            animator.SetInteger("Direction", 0);
        }
	}
}
