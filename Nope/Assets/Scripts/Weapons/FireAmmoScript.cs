using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireAmmoScript : MonoBehaviour 
{

    protected Vector3 position;
    protected Vector3 direction;
    protected int damage;
    protected List<Collider> playersInRadius; 

    public FireAmmoScript(Vector3 position, Vector3 direction, int damage)
    {
        this.position = position;
        this.direction = direction;
        this.damage = damage;
        this.playersInRadius = new List<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.position += this.direction * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGameObject = collision.gameObject;
        if(collisionGameObject.tag == "player")
        {
            //TODO Outch
        }
        foreach(Collider collider in playersInRadius)
        {
            GameObject colliderGameObject = collider.gameObject;
            if(collisionGameObject != colliderGameObject)
            {
                //TODO Outch
            }
        }
        Destroy(this);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "player")
        {
            playersInRadius.Add(collider);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "player")
        {
            playersInRadius.Remove(collider);
        }
    }

}
