using UnityEngine;
using System.Collections;
public class AlienController : MonoBehaviour
{
    public float dashSpeed = 20f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 2f;

    private float dashCooldownTimer = 0f;
    private bool isDashing = false;
    private bool isInOrbit = false;
    private GameObject currentPlanet = null;
    public float walkSpeed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isInOrbit && currentPlanet != null)
        {

            RotateAroundPlanet();
        }
        if (dashCooldownTimer <= 0 && !isDashing)
        {

            StartCoroutine(DoDash());
            dashCooldownTimer = dashCooldown;
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }
    IEnumerator DoDash()
    {
        isDashing = true;
        float originalSpeed = walkSpeed;
        walkSpeed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        walkSpeed = originalSpeed;
        isDashing = false;
    }
    void RotateAroundPlanet()
    {

        float direction = 1f;
        Vector3 planetCenter = currentPlanet.transform.position;
        transform.RotateAround(planetCenter, Vector3.forward, walkSpeed * direction * Time.deltaTime);


        Vector3 toCenter = (planetCenter - transform.position).normalized;
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, -toCenter);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            isInOrbit = true;
            currentPlanet = other.gameObject;
            rb.gravityScale = 0;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentPlanet)
        {
            isInOrbit = false;
            currentPlanet = null;
            rb.gravityScale = 1;
        }
    }
}
