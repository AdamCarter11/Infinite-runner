using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float bounceOffForce = 3f;
    [SerializeField] float shootForce = 10f;
    private Rigidbody2D rb;
    private int bounces = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ShootTowardsMouse();
    }

    void ShootTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDirection = (mousePos - transform.position).normalized;
        rb.AddForce(shootDirection * shootForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Rigidbody2D blockRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (blockRigidbody != null)
            {
                // Calculate the incoming velocity and normal vector
                Vector2 incomingVelocity = rb.velocity.normalized;
                Vector2 normal = (collision.contacts[0].point - (Vector2)transform.position).normalized;

                // Calculate the reflection direction based on incoming velocity and normal
                Vector2 reflectedDirection = Vector2.Reflect(incomingVelocity, normal);

                // Apply the reflected direction with the bounce force
                rb.velocity = reflectedDirection * bounceOffForce;

                // Apply force to the block
                Vector2 pushDirection = (collision.contacts[0].point - (Vector2)transform.position).normalized;
                blockRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                // Destroy the projectile
                DestroyProj();
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Rigidbody2D blockRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (blockRigidbody != null)
            {
                // Calculate average contact normal
                ContactPoint2D[] contacts = new ContactPoint2D[10];
                int contactCount = collision.GetContacts(contacts);
                Vector2 averageNormal = Vector2.zero;

                // Calculate the average contact normal
                for (int i = 0; i < contactCount; i++)
                {
                    averageNormal += contacts[i].normal;
                }
                averageNormal /= contactCount;

                // Calculate reflection direction
                Vector2 incomingVelocity = rb.velocity.normalized;
                Vector2 reflectedDirection = Vector2.Reflect(incomingVelocity, averageNormal.normalized);

                // Apply the reflected direction with the bounce force
                rb.velocity = reflectedDirection * bounceOffForce;

                // Apply force to the block (push direction remains the same)
                Vector2 pushDirection = (averageNormal).normalized;
                blockRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);

                DestroyProj();
            }
        }
        // proj bounce off
        if (!collision.gameObject.CompareTag("Player"))
        {
            Vector2 speed = rb.velocity;
            Vector2 dir = Vector2.Reflect(speed.normalized, collision.transform.position - transform.position);
            rb.velocity = dir * Mathf.Max(speed.magnitude, 0f);
        }
        
    }
    private void DestroyProj()
    {
        bounces--;
        if (bounces < 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetBounces(int newBounces)
    {
        bounces = newBounces;
    }


}
