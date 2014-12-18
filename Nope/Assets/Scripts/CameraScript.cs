using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/CameraStyle")]
public class CameraScript : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOffset;
    public float distance = 5.0f;
    public float maxDistance = 20;
    public float minDistance = .6f;
    public float xSpeed = 200.0f;
    public float ySpeed = 200.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public int zoomRate = 40;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;
    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;
    private float isOrbiting = 0;
    void Start() { Init(); }
    void OnEnable() { Init(); }
    public void Init()
    {
        //If there is no target, create a temporary target at 'distance' from the cameras current viewpoint
        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }
        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        //be sure to grab the current rotations as starting points.
        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        xDeg = Vector3.Angle(Vector3.right, transform.right);
        yDeg = Vector3.Angle(Vector3.up, transform.up);
    }
    /*
	 * Camera logic on LateUpdate to only update after all character movement logic has been handled.
	 */
    void LateUpdate()
    {
        // ZOOM!
        if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
        {
            desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(desiredDistance);
        }
        //ORBIT
        if (Input.GetMouseButton(0) /*&& Input.GetKey(KeyCode.LeftAlt)*/)
        {
            isOrbiting = 1;
        }
        else
        {
            isOrbiting = 0;
        }

        xDeg += Input.GetAxis("Mouse X") * (xSpeed * isOrbiting) * 0.02f;
        yDeg -= Input.GetAxis("Mouse Y") * (ySpeed * isOrbiting) * 0.02f;

        //Clamp the vertical axis for the orbit
        yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
        // set camera rotation
        desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
        currentRotation = transform.rotation;

        rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
        transform.rotation = rotation;



        // Remove Pan
  // otherwise if middle mouse is selected, we pan by way of transforming the target in screenspace
        if (Input.GetMouseButton(2))
        {
            //grab the rotation of the camera so we can move in a psuedo local XY space
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed);
            target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed, Space.World);
        }
  



        /********Update Orbit******/


        // affect the desired Zoom distance if we roll the scrollwheel
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        //clamp the zoom min/max
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        // For smoothing of the zoom, lerp distance
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        // calculate position based on the new currentDistance
        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;
    }
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}









/*public class CameraScript : MonoBehaviour
{
    private float maxPickingDistance = 2000;// increase if needed, depending on your scene size

    private Transform pickedObject = null;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            Debug.Log("Touching at: " + touch.position);

            //Gets the ray at position where the screen is touched
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                Debug.Log("Touch phase began at: " + touch.position);

                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit, maxPickingDistance))
                {
                    pickedObject = hit.transform;
                }
                else
                {
                    pickedObject = null;
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Debug.Log("Touch phase Moved");

                if (pickedObject != null)
                {
                    Vector2 screenDelta = touch.deltaPosition;

                    float halfScreenWidth = 0.5f * Screen.width;
                    float halfScreenHeight = 0.5f * Screen.height;

                    float dx = screenDelta.x / halfScreenWidth;
                    float dy = screenDelta.y / halfScreenHeight;

                    Vector3 objectToCamera =
                        pickedObject.transform.position - Camera.main.transform.position;
                    float distance = objectToCamera.magnitude;

                    float fovRad = Camera.main.fieldOfView * Mathf.Deg2Rad;
                    float motionScale = distance * Mathf.Tan(fovRad / 2);

                    Vector3 translationInCameraRef =
                        new Vector3(motionScale * dx, motionScale * dy, 0);

                    Vector3 translationInWorldRef =
                        Camera.main.transform.TransformDirection(translationInCameraRef);

                    pickedObject.position += translationInWorldRef;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("Touch phase Ended");

                pickedObject = null;
            }
        }
    }
}*/
