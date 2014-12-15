using UnityEngine;
using System.Collections;

public class StandActionScript : ActionScript
{
    
    public StandActionScript(Vector3 destination, int duration) : base(destination, duration)
    {
    }

    public override void FixedUpdate(Transform transform)
    {
        if(this.started && Time.time - this.startTime > duration)
        {
            this.endSimulation();
        }
    }

    public override object[] getArrayOfParams()
    {
        object[] array = { "StandActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "StandActionScript";
    }
}
