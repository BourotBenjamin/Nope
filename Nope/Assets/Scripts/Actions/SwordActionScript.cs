using UnityEngine;
using System.Collections;

public class SwordActionScript : ActionScript
{

    private bool created;

    public SwordActionScript(Vector3 destination, int duration)
        : base(destination, duration)
    {
        created = false;
    }
    public SwordActionScript()
        : base()
    {
        created = false;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if (!created && Network.isServer)
            {
                created = true;
                Collider selfCollider = simulation.collider;
                float minDist = int.MaxValue;
                float tempDist = 0f;
                Collider nearest = null;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f);
                foreach (Collider collider in hitColliders)
                {
                    if (collider.tag == "Player" && collider != selfCollider)
                    {
                        tempDist = (selfCollider.transform.position - collider.transform.position).magnitude;
                        if(tempDist < minDist)
                        {
                            minDist = tempDist;
                            nearest = collider;
                        }
                    }
                }
                if(nearest != null)
                {
                    Network.Destroy(nearest.gameObject);
                    nearest.gameObject.GetComponent<SimulateScript>().warriorDies();
                }
            }
            else if (Time.time - this.startTime > 1.0f)
            {
                this.endSimulation();
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "SwordActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "SwordActionScript";
    }
}
