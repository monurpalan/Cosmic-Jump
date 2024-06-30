using UnityEngine;

public class WormController : MonoBehaviour
{
    public GameObject planet;
    public float emergeInterval = 5f;
    private float timer;
    public float emergeDistance = 2f;
    public float emergeSpeed = 5f;
    private Vector3 emergeDirection;
    private Vector3 startPosition;
    private bool isEmerged = false;

    void Start()
    {
        timer = emergeInterval;
        startPosition = transform.position;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && !isEmerged)
        {
            StartEmerge();
            timer = emergeInterval;
        }
        else if (isEmerged)
        {

            ManageEmergeAndReturn();
        }
    }

    void StartEmerge()
    {

        emergeDirection = (transform.position - planet.transform.position).normalized;
        isEmerged = true;
    }

    void ManageEmergeAndReturn()
    {

        if ((transform.position - startPosition).magnitude < emergeDistance)
        {

            transform.position += emergeDirection * emergeSpeed * Time.deltaTime;
        }
        else
        {

            ReturnToStart();
        }
    }

    void ReturnToStart()
    {

        transform.position -= emergeDirection * emergeSpeed * Time.deltaTime;


        if ((transform.position - startPosition).magnitude < 0.1f)
        {
            transform.position = startPosition;
            isEmerged = false;
        }
    }
}