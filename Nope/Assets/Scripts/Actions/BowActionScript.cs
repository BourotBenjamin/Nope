using UnityEngine;
using System.Collections;

public class BowActionScript : ActionScript
{

    private bool created;
    private GameObject prefab;

    public BowActionScript(Vector3 destination, int duration, GameObject prefab)
        : base(destination, duration)
    {
        destinationNeeded = true;
        created = false;
        this.prefab = prefab;
    }
    public BowActionScript()
        : base()
    {
        destinationNeeded = true;
        created = false;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if (!created && Network.isServer)
            {
                created = true;
                simulation.transform.LookAt(this.destination);
                GameObject obj = (GameObject)Network.Instantiate(Resources.Load("Prefabs/Arrow", typeof(GameObject)), simulation.transform.position + simulation.transform.forward * 2 + Vector3.up, simulation.transform.rotation, 0);
                obj.networkView.RPC("initValues", RPCMode.All, new object[] { simulation.transform.position + simulation.transform.forward * 2, destination, 10 });
            }
            else if (Time.time - this.startTime > 1.0f)
            {
                this.endSimulation();
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "BowActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "BowActionScript";
    }
}
