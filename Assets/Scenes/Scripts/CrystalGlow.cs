using UnityEngine;

public class CrystalGlow : MonoBehaviour
{
    public float glowDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private float time;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

        float alpha = (Mathf.Sin(time * (2 * Mathf.PI) / glowDuration) + 1) * 0.5f;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        time += Time.deltaTime;


        if (time > glowDuration)
        {
            time = 0;
        }
    }
}
