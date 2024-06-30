using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomStep = 3f;
    public float minCamSize = 5f;
    public float maxCamSize = 33f;


    public void ZoomIn()
    {
        mainCamera.orthographicSize -= zoomStep;
        mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, minCamSize);
    }


    public void ZoomOut()
    {
        mainCamera.orthographicSize += zoomStep;
        mainCamera.orthographicSize = Mathf.Min(mainCamera.orthographicSize, maxCamSize);
    }
}