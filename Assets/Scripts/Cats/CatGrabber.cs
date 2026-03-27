using System;
using UnityEngine;

public class CatGrabber : MonoBehaviour
{
    public event Action onCatGrabbed;
    public event Action onCatUngrabbed;

    [HideInInspector] public bool hasCat;
    [SerializeField] private float catDroppingForce;
    [SerializeField] private float verticalCarryPositionOffset;

    private Cat grabbedCat;

    private void Start()
    {
        GetComponent<Collider2D>();
        GetComponent<Rigidbody2D>();
    }
   
    public void GrabCat(Cat cat)
    {
        hasCat = true;
        grabbedCat = cat;
        cat.SetAsGrabbed(transform, verticalCarryPositionOffset);

        onCatGrabbed?.Invoke();
    }

    private void UngrabCat()
    {
        hasCat = false;
        grabbedCat.SetAsUngrabbed();
        grabbedCat = null;

        onCatUngrabbed?.Invoke();
    }

    public void DropCatTowards(Vector2 direction)
    {
        grabbedCat.ImpulseTowards(direction.normalized, catDroppingForce);
        UngrabCat();
    }

    [ContextMenu("Drop Cat")]
    public void DropCatTowardsRandomDirection()
    {
        DropCatTowards(new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)));
    }
}
