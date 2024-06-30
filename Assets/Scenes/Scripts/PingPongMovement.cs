using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float distance = 10f;
    public bool moveInX = true;
    public bool moveInY = true;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPosition = startPosition;

        if (moveInX)
        {
            newPosition.x += Mathf.PingPong(Time.time * moveSpeed, distance) - distance / 2;
        }

        if (moveInY)
        {
            newPosition.y += Mathf.PingPong(Time.time * moveSpeed, distance) - distance / 2;
        }

        transform.position = newPosition;
    }
}
