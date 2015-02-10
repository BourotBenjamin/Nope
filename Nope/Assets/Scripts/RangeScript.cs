using UnityEngine;
using System.Collections;

public class RangeScript : MonoBehaviour {

    private float sizeX;
    private float sizeZ;
    private GameObject primitive;
    private Vector3 scale;
    private Vector3 position;
    private Mesh mesh;
    private aimScript aim;
    // only change if using with a custom created plane that has a different number of segments
    private int m_planeSegments = 10;
    Texture2D circleRange;
    Texture2D attackRange;
    // Use this for initialization
    void Start () {
        //UpdatePlane();
    }
   
    // Update is called once per frame
    void FixedUpdate () {
      //  UpdatePlane();
       
    }
 
    /// <summary>
    /// Update the plane so that its the same shape as the terrain under it
    /// Call after the position of the plane has changed
    /// </summary>
    public void addRange(float sizex, float sizez, Vector3 pos)
    {
        sizeX = sizex/5;
        sizeZ = sizez/5;
        primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.transform.position = pos;
        primitive.transform.localScale = new Vector3(sizeX, 0, sizeZ);
        //MeshRenderer renderer = primitive.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        primitive.renderer.material.shader = Shader.Find("Particles/Alpha Blended");
        primitive.collider.enabled = false;
        circleRange = new Texture2D(2, 2);
        circleRange = Resources.Load("Materials/circleRange", typeof(Texture2D)) as Texture2D;
        primitive.renderer.material.mainTexture = circleRange;
    }

    public GameObject addRangeAttack(float sizex, float sizez, Vector3 pos)
    {
        sizeX = sizex/5;
        sizeZ = sizez/5;
        primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.transform.position = pos;
        primitive.transform.localScale = new Vector3(sizeX, 0, sizeZ);
        primitive.renderer.material.shader = Shader.Find("Particles/Alpha Blended");
        primitive.collider.enabled = false;
        primitive.transform.position = new Vector3(pos.x, 0.2f, pos.z-5);
        attackRange = new Texture2D(2, 2);
        attackRange = Resources.Load("Materials/healthEnemyFull", typeof(Texture2D)) as Texture2D;
        primitive.renderer.material.mainTexture = attackRange;
        primitive.transform.parent = transform;
        
        return primitive;
    }
    public float getCircleRay()
    {
        return primitive.transform.localScale.z*10;
    }

    public void addPointDest(Vector3 posDest)
    {
        primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.transform.position = posDest;
        primitive.transform.localScale = new Vector3(0.05f, 0, 0.05f);
        primitive.renderer.material.shader = Shader.Find("Particles/Alpha Blended");
        circleRange = new Texture2D(2, 2);
        circleRange = Resources.Load("Materials/circleRange", typeof(Texture2D)) as Texture2D;
        primitive.renderer.material.mainTexture = circleRange;
        primitive.collider.enabled = false;
    }

    public void addPointPull(Vector3 posDest)
    {
        primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.transform.position = posDest;
        primitive.transform.localScale = new Vector3(0.05f, 0, 1.0f);
        primitive.renderer.material.shader = Shader.Find("Particles/Alpha Blended");
        attackRange = new Texture2D(2, 2);
        attackRange = Resources.Load("Materials/healthEnemyFull", typeof(Texture2D)) as Texture2D;
        primitive.renderer.material.mainTexture = attackRange;
        primitive.collider.enabled = false;

    }

    public void deleteRange()
    {
        Destroy(primitive);
        mesh = null;
    }
    public void UpdatePlane()
    {   
        
        if (mesh != null)
        {
            position = new Vector3(transform.position.x+ (sizeX/2), transform.position.y, transform.position.z+(sizeZ/2));
            Vector3[] vertices = mesh.vertices;
           
            float xStep = (sizeX / m_planeSegments);
            float zStep = (sizeZ / m_planeSegments);
            int squaresize = m_planeSegments + 1;
            for (int n = 0; n < squaresize; n++)
            {
                for (int i = 0; i < squaresize; i++)
                {
                    vertices[(n*squaresize)+i].y = 10;
                    mesh.vertices[i].y = vertices[(n * squaresize) + i].y;
                    position.x -= xStep;
                }
                position.x += (((float)squaresize) *xStep);
                position.z -= zStep;
            }
            //mesh.vertices = vertices;
            foreach(var i in mesh.vertices)
            {
                mesh.Clear();
                mesh.RecalculateBounds();
                //mesh.RecalculateNormals();
            }
            
        }
    }
}
