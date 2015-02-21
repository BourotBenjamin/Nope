using UnityEngine;
using System.Collections;

public class TrapActionScript : ActionScript
{

    private bool created;
    private GameObject prefab;

    public TrapActionScript(Vector3 destination, int duration)
        : base(destination, duration)
    {
        destinationNeeded = false;
        created = false;
    }
    public TrapActionScript()
        : base()
    {
        destinationNeeded = false;
        created = false;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if (!created && Network.isServer)
            {
                created = true;
                Network.Instantiate(Resources.Load("Prefabs/Mine", typeof(GameObject)), simulation.transform.position, simulation.transform.rotation, 0);
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
