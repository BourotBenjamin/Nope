using UnityEngine;
using System.Collections;

public class planeRangeScript : MonoBehaviour {

    private float sizeX;
    private float sizeZ;
    private GameObject primitive;
    private Vector3 scale;
    private Mesh mesh;
    // only change if using with a custom created plane that has a different number of segments
    private int m_planeSegments = 10;
 
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
        sizeX = sizex;
        sizeZ = sizez;
        primitive = GameObject.CreatePrimitive(PrimitiveType.Plane);
        primitive.transform.position = pos;
        primitive.transform.localScale = new Vector3(sizeX, 0, sizeZ);
        //MeshRenderer renderer = primitive.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        primitive.renderer.material.shader = Shader.Find("Particles/Alpha Blended");

        Texture2D circleRange = new Texture2D(2, 2);
        circleRange = Resources.Load("Images/circleRange", typeof(Texture2D)) as Texture2D;
        Debug.Log(circleRange);
        primitive.renderer.material.mainTexture = circleRange;

        mesh = ((MeshFilter)primitive.GetComponent(typeof(MeshFilter))).mesh as Mesh;
        UpdatePlane();
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
            Vector3 position = new Vector3(transform.position.x+ (sizeX/2), transform.position.y, transform.position.z+(sizeZ/2));
            Vector3[] vertices = mesh.vertices;
            float xStep = (sizeX / m_planeSegments);
            float zStep = (sizeZ / m_planeSegments);
            int squaresize = m_planeSegments + 1;
            for (int n = 0; n < squaresize; n++)
            {
                for (int i = 0; i < squaresize; i++)
                {
                    //vertices[(n*squaresize)+i].y = Terrain.activeTerrain.SampleHeight(position);
                    position.x -= xStep;
                }
                position.x += (((float)squaresize) *xStep);
                position.z -= zStep;
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }
    }
}
