using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireAmmoScript : MonoBehaviour 
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
            if (collisionGameObject.tag != "Ground")
            {
                if (collisionGameObject.tag == "Player")
                {
                    collisionGameObject.networkView.RPC("warriorHurt", RPCMode.All, 2);
                }
                foreach (Collider collider in playersInRadius)
                {
                    GameObject colliderGameObject = collider.gameObject;
                    if (collisionGameObject != colliderGameObject)
                    {
                        collisionGameObject.networkView.RPC("warriorHurt", RPCMode.All, 2);
                    }
                }
                transform.position = new Vector3(90, 90, 90);
                Network.Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Network.isServer)
        { 
            if (collider.gameObject.tag == "Player")
            {
                playersInRadius.Add(collider);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (Network.isServer)
        { 
            if (collider.gameObject.tag == "Player")
            {
                playersInRadius.Remove(collider);
            }
        }
    }

}
