using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] float maxFallSpeed = 5f;
    [SerializeField] float bottomCap = -4.5f;

    [SerializeField] Color targetColor = Color.red;
    [SerializeField] float lerpDuration = 3f; // Duration of the color lerping in seconds
    private Color initialColor;

    private Rigidbody2D rb;
    private bool startLifetime;

    private void Start()
    {
        initialColor = GetComponent<SpriteRenderer>().color;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.velocity.y < -maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -maxFallSpeed);
        } 
        if(transform.position.y <= bottomCap)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            if (!startLifetime)
            {
                startLifetime = true;
                StartCoroutine(LerpColorAndDestroy());
            }
        }
    }

    private IEnumerator LerpColorAndDestroy()
    {
        float lerpTimer = 0f;

        while (lerpTimer < lerpDuration)
        {
            // Calculate lerp percentage based on time
            float lerpPercent = lerpTimer / lerpDuration;

            // Lerp the color from initial color to target color
            Color lerpedColor = Color.Lerp(initialColor, targetColor, lerpPercent);

            // Apply the lerped color to the object's renderer
            GetComponent<SpriteRenderer>().color = lerpedColor;

            // Increment the timer by deltaTime
            lerpTimer += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the color is set to the target color when lerping is complete
        GetComponent<SpriteRenderer>().color = targetColor;

        // Destroy the object after lerping is complete
        Destroy(gameObject);
    }

}
