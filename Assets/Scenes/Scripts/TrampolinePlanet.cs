using UnityEngine;

public class TrampolinePlanet : MonoBehaviour
{
    public float bounceForce = 20f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (other.transform.position - transform.position).normalized;
                rb.AddForce(direction * bounceForce, ForceMode2D.Impulse);


                if (audioSource != null)
                {
                    audioSource.Play();
                }
            }
        }
    }
}
