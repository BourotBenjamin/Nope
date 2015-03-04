using UnityEngine;
using System.Collections;

public class HealActionScript : ActionScript
{

    private bool created;
    private Object prefab;

    public HealActionScript(Vector3 destination, int duration)
        : base(destination, duration)
    {
        destinationNeeded = false;
        created = false;
    }
    public HealActionScript()
        : base()
    {
        destinationNeeded = false;
        created = false;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if (!created)
            {
                created = true;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    if (hitColliders[i].tag == "Player")
                    {
                        hitColliders[i].networkView.RPC("warriorHealed", RPCMode.All, simulation.owner);
                    }
                    i++;
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
        object[] array = { "HealScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "HealScript";
    }
}
