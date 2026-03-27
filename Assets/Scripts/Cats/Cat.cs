using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

public class Cat : MonoBehaviour
{
    private Collider2D collider;
    private Rigidbody2D rb;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void ImpulseTowards(Vector2 direction, float force)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public void SetAsGrabbed(Transform grabber, float verticalGrabOffset)
    {
        transform.position = grabber.position + Vector3.up * verticalGrabOffset;
        transform.parent = grabber;
        collider.enabled = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetAsUngrabbed()
    {
        transform.parent = null;
        collider.enabled = true;
    }
}
