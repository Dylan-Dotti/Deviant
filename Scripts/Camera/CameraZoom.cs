using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField]
    private FloatRange cameraDistRange;
    [SerializeField]
    private float scrollSpeed = 1;

    private Camera playerCamera;
    private float originalDist;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        originalDist = playerCamera.orthographicSize;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Home))
        {
            playerCamera.orthographicSize = originalDist;
        }
        float mouseWheelDelta = -Input.mouseScrollDelta.y * 
            Time.deltaTime * scrollSpeed;
        playerCamera.orthographicSize = Mathf.Clamp(
            playerCamera.orthographicSize + mouseWheelDelta,
            cameraDistRange.Min, cameraDistRange.Max);
    }
}
