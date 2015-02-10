using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HiddenExplosiveTrapScript : MonoBehaviour {

    private List<Collider> collidersInArea;

    [RPC]
    void Show()
    {
        this.renderer.enabled = true;
    }

    void Start()
    {
        if(Network.isServer)
        {
            collidersInArea = new List<Collider>();
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, ((SphereCollider) this.collider).radius);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Player")
                {
                    collidersInArea.Add(hitColliders[i]);
                }
                i++;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (Network.isServer && collider.tag == "Player" && collidersInArea.Contains(collider))
        {
            collidersInArea.Remove(collider);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Network.isServer && collider.tag == "Player" && !collidersInArea.Contains(collider))
        {
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 10.0f);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Player")
                {
                    hitColliders[i].gameObject.GetComponent<SimulateScript>().warriorDies();
                    Network.Destroy(hitColliders[i].gameObject);
                }
                i++;
            }
            Network.Destroy(this.gameObject);
        }
    }
}
