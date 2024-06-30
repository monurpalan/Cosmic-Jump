using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravity = -10f;
    public float orbitRadius = 5f;

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playerController.IsInOrbit)
            {
                Vector2 direction = (transform.position - other.transform.position).normalized;

                float distance = Vector2.Distance(transform.position, other.transform.position);
                float forceMagnitude = gravity * (1 - (distance / orbitRadius));

                other.attachedRigidbody.AddForce(direction * forceMagnitude, ForceMode2D.Force);
            }
        }

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Alien"))
        {
            other.transform.SetParent(transform);
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}
