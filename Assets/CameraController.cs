
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private InputSystem inputSystem;

    public float zoomSetting1 = 15f;
    public float sensitivity = 0.5f;
    public float lerpSpeed = 5f; // Adjust for how fast it reaches the target
    public float zoomSensitivity = 5f;

    private bool zoomedIn = false;
    private Vector3 freeRoamCameraReturnPos;
    private Vector3 targetPosition;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        inputSystem.ZoomInEvent += HandleZoomIn;
        inputSystem.ZoomOutEvent += HandleZoomOut;
        inputSystem.AdjustZoomEvent += HandleAdjustZoom;
        inputSystem.CameraTargetEvent += HandleCameraTarget;
    }


    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        if (zoomedIn)
        {
            PositionCamera(targetPosition);
        }
    }

    private void HandleZoomIn(Vector2 mousePos)
    {
        zoomedIn = true;
        freeRoamCameraReturnPos = Camera.main.transform.position;
        Ray ray = Camera.main.ScreenPointToRay(mousePos); 
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = new Vector3(hit.point.x, hit.point.y, Camera.main.transform.position.z);
            Camera.main.transform.position = targetPosition;
        }
        else
        {
            targetPosition = Camera.main.transform.position;
        }


        Camera.main.fieldOfView = zoomSetting1;
    }

    private void HandleZoomOut()
    {
        zoomedIn = false;

        Camera.main.fieldOfView = GlobalSettings.Instance.freeRoamFOV;
        Camera.main.transform.position = freeRoamCameraReturnPos;
    }

    private void HandleAdjustZoom(float z)
    {
        if (z > 0)
        {
            Camera.main.fieldOfView += zoomSensitivity;

        }
        else if (z < 0)
        {
            Camera.main.fieldOfView -= zoomSensitivity;
        }
    }

    private void HandleCameraTarget(Vector2 mouseDelta)
    {
        Vector3 cameraDisplacement = new Vector3(mouseDelta.x, mouseDelta.y, 0) * sensitivity;
        targetPosition += cameraDisplacement;
    }

    private void PositionCamera(Vector3 target)
    {
        Camera.main.transform.position = SmoothLerp(Camera.main.transform.position, target, Time.deltaTime * lerpSpeed);
    }

    private Vector3 SmoothLerp(Vector3 start, Vector3 end, float t)
    {
        t = Mathf.Clamp01(t);
        t = t * t * (3f - 2f * t);
        return Vector3.Lerp(start, end, t);
    }
}


