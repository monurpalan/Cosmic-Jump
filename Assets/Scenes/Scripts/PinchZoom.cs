using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public float zoomSpeed = 0.5f;
    public float minZoom = 1f;
    public float maxZoom = 5f;
    public Vector2 zoomCenter; // Zoom merkezi, varsayılan olarak kameranın ortası

    private Camera cam;
    private float initialOrthographicSize;
    private Vector3 initialPosition;

    void Start()
    {
        cam = Camera.main;
        initialOrthographicSize = cam.orthographicSize;
        initialPosition = cam.transform.position;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            Zoom(deltaMagnitudeDiff * zoomSpeed);
        }
    }

    void Zoom(float deltaMagnitudeDiff)
    {
        cam.orthographicSize += deltaMagnitudeDiff;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        // Kamerayı sınırlı alanda tut
        Vector3 newPosition = cam.transform.position;
        newPosition += (cam.ScreenToWorldPoint(zoomCenter) - cam.transform.position) * (deltaMagnitudeDiff / initialOrthographicSize);
        newPosition.x = Mathf.Clamp(newPosition.x, initialPosition.x - (cam.orthographicSize * cam.aspect), initialPosition.x + (cam.orthographicSize * cam.aspect));
        newPosition.y = Mathf.Clamp(newPosition.y, initialPosition.y - cam.orthographicSize, initialPosition.y + cam.orthographicSize);
        cam.transform.position = newPosition;
    }
}
