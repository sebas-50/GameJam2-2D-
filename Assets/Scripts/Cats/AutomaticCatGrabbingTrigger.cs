using UnityEngine;

[RequireComponent(typeof(CatGrabber), typeof(Collider2D))]

public class AutomaticCatGrabbingTrigger : MonoBehaviour
{
    [SerializeField] private Collider2D triggerCollider;

    [SerializeField] private float reEnableGrabbingDelay;

    private CatGrabber catGrabber;
    private bool automaticGrabbingEnabled;

    private void Start()
    {
        catGrabber = GetComponent<CatGrabber>();
        catGrabber.onCatUngrabbed += EnableGrabbingAfterDelay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat") && !catGrabber.hasCat)
        {
            catGrabber.GrabCat(collision.GetComponent<Cat>());

            triggerCollider.enabled = false;
        }
    }

    private void EnableGrabbingAfterDelay()
    {
        Invoke(nameof(EnableGrabbing), reEnableGrabbingDelay);
    }

    private void EnableGrabbing()
    {
        triggerCollider.enabled = true;
    }
}
