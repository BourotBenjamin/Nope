﻿using UnityEngine;
using System.Collections;

public class MeteorActionScript : ActionScript
{

    private bool created;

    public MeteorActionScript(Vector3 destination, int duration, GameObject prefab)
        : base(destination, duration)
    {
        destinationNeeded = true;
        created = false;
    }
    public MeteorActionScript()
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
                Network.Instantiate(Resources.Load("Prefabs/Meteor", typeof(GameObject)), destination + Vector3.up * 2, simulation.transform.rotation, 0);
            }
            else if (Time.time - this.startTime > 1.0f)
            {
                this.endSimulation();
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "MeteorActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "MeteorActionScript";
    }
}
