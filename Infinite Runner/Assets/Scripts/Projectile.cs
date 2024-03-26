using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float pushForce = 10f;
    [SerializeField] float shootForce = 10f;
    private Rigidbody2D rb;

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
                Vector2 pushDirection = (collision.contacts[0].point - (Vector2)transform.position).normalized;
                blockRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                Destroy(this.gameObject);
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
                Vector2 pushDirection = (collision.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
                blockRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
                Destroy(gameObject); // Destroy the projectile
            }
        }
    }



}
