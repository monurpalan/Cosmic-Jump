using UnityEngine;

public class BlackHoleEffect : MonoBehaviour
{
    public float pullStrength = 10f;
    public float shrinkSpeed = 0.1f;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            Vector2 direction = (transform.position - other.transform.position).normalized;
            other.attachedRigidbody.AddForce(direction * pullStrength);


            other.transform.localScale = Vector3.Lerp(other.transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);


            if (other.transform.localScale.x < 0.1)
            {
                PlayerController playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DieAndRespawn();
                }
            }
        }
    }
}