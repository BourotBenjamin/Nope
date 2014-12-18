using UnityEngine;
using System.Collections;

public class WalkActionScript : ActionScript 
{

    public WalkActionScript(Vector3 destination, int duration) : base(destination, duration)
    {
    }

    public override void FixedUpdate(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            Debug.LogError("Hello");
            Vector3 direction = this.destination - transform.position;
            direction.y = 0f;
            Debug.LogError(direction);
            if ((duration != -1 && Time.time - this.startTime > duration) || direction.magnitude < 1.0f)
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
