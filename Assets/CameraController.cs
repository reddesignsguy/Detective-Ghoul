
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private InputSystem inputSystem;
    public float zoomSetting1 = 15f;
    public float sensitivity = 0.5f;
    public float lerpSpeed = 5f; // Adjust for how fast it reaches the target
    public float zoomSensitivity = 5f;

    private bool zoomedIn = false;
    private Vector3 cameraReturnPosition;
    private Vector3 targetPosition;
    private float maxZoomRoamDistance;

    private void Awake()
    {
        maxZoomRoamDistance = GlobalSettings.Instance.maxZoomRoamDistance;
    }
   
    private void OnEnable()
    {
        GameContext.Instance.ZoomStartEvent += HandleZoomStart;
        GameContext.Instance.ZoomEndEvent += HandleZoomOut;
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

    private void HandleZoomStart(Vector3 newTargetPosition)
    {
        zoomedIn = true;
        targetPosition = newTargetPosition;
        cameraReturnPosition = Camera.main.transform.position;
        Camera.main.transform.position = targetPosition;
        Camera.main.fieldOfView = zoomSetting1;
    }

    private void HandleZoomOut()
    {
        zoomedIn = false;
        Camera.main.fieldOfView = GlobalSettings.Instance.freeRoamFOV;
        Camera.main.transform.position = cameraReturnPosition;
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
        Vector3 newPosition = targetPosition + cameraDisplacement;

        bool withinDistance = Vector3.Distance(cameraReturnPosition, newPosition) <= maxZoomRoamDistance;
        if (withinDistance)
        {
            targetPosition = newPosition;
        }
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


