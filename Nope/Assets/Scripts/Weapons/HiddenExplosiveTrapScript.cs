using UnityEngine;
using System.Collections;

public class HiddenExplosiveTrapScript : MonoBehaviour {

    [RPC]
    void Show()
    {
        this.renderer.enabled = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        if(Network.isServer && collider.tag == "Player")
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10.0f);
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
