using UnityEngine;
using System.Collections;

public class StandActionScript : ActionScript
{
    
    public StandActionScript(Vector3 destination, int duration) : base(destination, duration)
    {
    }

    void FixedUpdate()
    {
        if(this.started && Time.time - this.startTime > duration)
        {
            this.endSimulation();
        }
    }
}
