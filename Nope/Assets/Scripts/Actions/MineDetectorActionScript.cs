using UnityEngine;
using System.Collections;

public class MineDetectorActionScript : ActionScript
{

    private bool created;
    private Object prefab;

    public MineDetectorActionScript(Vector3 destination, int duration)
        : base(destination, duration)
    {
        destinationNeeded = false;
        created = false;
    }
    public MineDetectorActionScript()
        : base()
    {
        destinationNeeded = false;
        created = false;
    }

    public override void doAction(Transform transform, Rigidbody rigidbody)
    {
        if (this.started)
        {
            if (!created && (Network.isServer || Network.player == simulation.owner))
            {
                created = true;
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5.0f);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    if (hitColliders[i].tag == "Trap")
                    {
                        hitColliders[i].gameObject.GetComponent<HiddenExplosiveTrapScript>().showMine();
                    }
                    i++;
                }
            }
            else if (Time.time - this.startTime > 1.0f)
            {
                this.endSimulation();
            }
        }
    }


    public override object[] getArrayOfParams()
    {
        object[] array = { "MineDetectorActionScript", destination, duration };
        return array;
    }

    public override string getName()
    {
        return "MineDetectorActionScript";
    }
}
