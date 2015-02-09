using UnityEngine;
using System.Collections;

public class WalkActionScript : ActionScript 
{

    public WalkActionScript(Vector3 destination, int duration) : base(destination, duration)
    {
        destinationNeeded = true;
    }

    public WalkActionScript() : base()
    {
        destinationNeeded = true;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            Vector3 direction = this.destination - transform.position;
            direction.y = 0f;

            if (/*(duration != -1 && Time.time - this.startTime > duration) ||*/ direction.magnitude < 0.1f)
            {
                this.endSimulation();
            }
            else
            {
                rigidbody.MovePosition(rigidbody.position + direction.normalized * Time.deltaTime * 10);
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
