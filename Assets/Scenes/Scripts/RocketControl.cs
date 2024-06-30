using UnityEngine;

public class RocketControl : MonoBehaviour
{
    public float speed = 5f;
    public GameObject playerPrefab;
    private GameObject currentPlayer;
    public float controlDuration = 10f;

    void Start()
    {
        currentPlayer = null;
    }

    void Update()
    {

        float inputX = Input.GetAxis("Horizontal");
        transform.position += new Vector3(inputX * speed * Time.deltaTime, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            currentPlayer = other.gameObject;
            currentPlayer.SetActive(false);
            Invoke("ReturnControlToPlayer", controlDuration);
        }
    }

    void ReturnControlToPlayer()
    {
        if (currentPlayer != null)
        {

            currentPlayer.transform.position = this.transform.position;
            currentPlayer.SetActive(true);


            Destroy(this.gameObject);
        }
    }
}
