using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]

public class Cat : MonoBehaviour
{
    [SerializeField] CatsData catsData;

    private Collider2D collider;
    private Rigidbody2D rb;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        catsData.cats.Add(this);
    }

    public void ImpulseTowards(Vector2 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public void SetAsGrabbed(Transform grabber, float verticalGrabOffset)
    {
        transform.position = grabber.position + Vector3.up * verticalGrabOffset;
        transform.parent = grabber;
        collider.enabled = false;
    }

    public void SetAsUngrabbed()
    {
        transform.parent = null;
        collider.enabled = true;
    }
}
