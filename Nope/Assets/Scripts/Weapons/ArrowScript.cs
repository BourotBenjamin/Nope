using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowScript : MonoBehaviour
{

    protected Vector3 direction;
    protected int damage;
    protected List<Collider> playersInRadius;


    [RPC]
    public void initValues(Vector3 startPos, Vector3 target, int damage)
    {
        this.damage = damage;
        this.playersInRadius = new List<Collider>();
        this.transform.position = startPos;
        this.direction = target - this.transform.position;
        direction.y = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + direction.normalized * Time.deltaTime * 10);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Network.isServer)
        {
            GameObject collisionGameObject = collision.gameObject;
            if (collisionGameObject.tag == "Player")
            {
                collisionGameObject.networkView.RPC("warriorHurt", RPCMode.All, 1);
            }
            Network.Destroy(this.gameObject);
        }
    }

}
