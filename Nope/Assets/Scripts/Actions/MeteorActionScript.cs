﻿using UnityEngine;
using System.Collections;

public class MeteorActionScript : ActionScript
{

    private bool created;
    private GameObject prefab;

    public MeteorActionScript(Vector3 destination, int duration, GameObject prefab)
        : base(destination, duration)
    {
        destinationNeeded = true;
        this.prefab = prefab;
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
        Debug.LogError("Meteor");
        if (this.started)
        {
            if (!created && Network.isServer)
            {
                created = true;
                GameObject obj = (GameObject)Network.Instantiate(Resources.Load("Prefabs/Meteor", typeof(GameObject)), destination + Vector3.up * 2, simulation.transform.rotation, 0);
                Debug.LogError(destination + Vector3.up * 2);
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
