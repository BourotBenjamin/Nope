using UnityEngine;
using System.Collections;

public class WeaponActionScript : ActionScript
{

    private bool created;
    private GameObject prefab;

    public WeaponActionScript(Vector3 destination, int duration, GameObject prefab )
        : base(destination, duration)
    {
        this.prefab = prefab;
        created = false;
        destinationNeeded = true;
    }
    public WeaponActionScript()
        : base()
    {
        created = false;
        destinationNeeded = true;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if(!created && Network.isServer)   
            {
                created = true;
                simulation.transform.LookAt(this.destination);
                GameObject obj = (GameObject)Network.Instantiate(Resources.Load("Prefabs/KillingBall", typeof(GameObject)), simulation.transform.position + simulation.transform.forward * 2, simulation.transform.rotation, 0);
                obj.networkView.RPC("initValues", RPCMode.All, new object[] { simulation.transform.position + simulation.transform.forward*2, destination, 10 });
            }
            else if (Time.time - this.startTime > 1.0f)
            {
                this.endSimulation();
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "WeaponActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "WeaponActionScript";
    }
}
