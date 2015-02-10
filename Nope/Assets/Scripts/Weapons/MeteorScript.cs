using UnityEngine;
using System.Collections;

public class MeteorScript : MonoBehaviour {

	void OnCollisionEnter(Collision collision)
    {
        if (Network.isServer)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.0f);
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
            Network.Destroy(gameObject);
        }
    }
}
