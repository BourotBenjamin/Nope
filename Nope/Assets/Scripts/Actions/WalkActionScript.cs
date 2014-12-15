using UnityEngine;
using System.Collections;

public class WalkActionScript : ActionScript 
{

    public WalkActionScript(Vector3 destination, int duration) : base(destination, duration)
    {
    }

    public override void FixedUpdate(Transform transform)
    {
        if (this.started)
        {
            Debug.LogError("Hello");
            Vector3 direction = transform.position - this.destination;
            if (Time.time - this.startTime > duration || direction.magnitude < 1.0f)
            {
                this.endSimulation();
            }
            else
            {
                transform.Translate(direction.normalized);
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "WalkActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "WalkActionScript";
    }
}
