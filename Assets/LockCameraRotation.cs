using UnityEngine;
using Cinemachine;

public class LockCameraRotation : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private void LateUpdate()
    {
        if (virtualCamera != null)
        {
            // Keep the camera's rotation fixed (e.g., looking straight ahead)
            virtualCamera.transform.rotation = Quaternion.Euler(0, virtualCamera.transform.rotation.eulerAngles.y, 0);
        }
    }
}
