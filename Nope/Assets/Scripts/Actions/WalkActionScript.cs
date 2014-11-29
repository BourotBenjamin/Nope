using UnityEngine;
using System.Collections;

public class WalkActionScript : ActionScript 
{

    public WalkActionScript(Vector3 destination, int duration) : base(destination, duration)
    {
    }

    void FixedUpdate()
    {
        if (this.started)
        {
            Vector3 direction = this.transform.position - this.destination;
            if (Time.time - this.startTime > duration || direction.magnitude < 1.0f)
            {
                this.endSimulation();
            }
            else
            {
                this.transform.Translate(direction.normalized);
            }
        }
    }
}
