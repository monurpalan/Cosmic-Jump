using UnityEngine;

public class OrbitMotion : MonoBehaviour
{
    public Transform orbitCenter;
    public float orbitRadius = 5f;
    public float orbitSpeed = 1f;
    private float orbitAngle;
    void FixedUpdate()
    {

        float orbitCircumference = 2 * Mathf.PI * orbitRadius;

        float anglePerSecond = (orbitSpeed / orbitCircumference) * 2 * Mathf.PI;


        orbitAngle += anglePerSecond * Time.deltaTime;

        orbitAngle %= 2 * Mathf.PI;


        float x = Mathf.Cos(orbitAngle) * orbitRadius;
        float y = Mathf.Sin(orbitAngle) * orbitRadius;
        transform.position = orbitCenter.position + new Vector3(x, y, 0);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}