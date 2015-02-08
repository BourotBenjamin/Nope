using UnityEngine;
using System.Collections;

public class TrapActionScript : ActionScript
{

    private bool created;
    private GameObject prefab;

    public TrapActionScript(Vector3 destination, int duration, GameObject prefab)
        : base(destination, duration)
    {
        this.prefab = prefab;
        created = false;
    }
    public TrapActionScript()
        : base()
    {
        created = false;
    }

    public override void FixedUpdate(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if (!created && Network.isServer)
            {
                created = true;
                GameObject obj = (GameObject)Network.Instantiate(prefab, simulation.transform.position, simulation.transform.rotation, 0);
                //obj.networkView.RPC("Show", simulation.owner);
            }
            else if (Time.time - this.startTime > 1.0f)
            {
                this.endSimulation();
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "TrapActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "TrapActionScript";
    }
}
