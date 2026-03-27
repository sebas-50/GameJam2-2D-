using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private CatGrabber catGrabber;
    [SerializeField] private Collider2D interactionTrigger;

    [SerializeField] private GameObject visualIndicator;
    [SerializeField] private float targetCheckingRate;

    private HashSet<Transform> targets;
    private Transform closestTarget;

    private bool isLookingForTarget = false;

    private void Start()
    {
        targets = new HashSet<Transform>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!catGrabber.hasCat)
            {
                switch (closestTarget.tag)
                {
                    case "Cat":
                        catGrabber.GrabCat(closestTarget.GetComponent<Cat>());

                        interactionTrigger.enabled = false;
                        StopLookingForTarget();
                        break;

                    case "Enemy":
                        closestTarget.GetComponent<CatGrabber>().DropCatTowardsDirection(transform.position - closestTarget.position);
                        targets.Remove(closestTarget);
                        break;
                }
            }
            else
            {
                catGrabber.DropCatTowardsRandomDirection();
                interactionTrigger.enabled = true;

                LookForTarget();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat") || (collision.CompareTag("Enemy") && collision.GetComponent<CatGrabber>().hasCat))
        {
            targets.Add(collision.transform);

            LookForTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Cat") || (collision.CompareTag("Enemy") && collision.GetComponent<CatGrabber>().hasCat))
        {
            targets.Remove(collision.transform);

            StopLookingForTarget();
        }
    }

    private void LookForTarget()
    {
        if (!isLookingForTarget)
        {
            isLookingForTarget = true;
            InvokeRepeating(nameof(CheckAndSetClosestTarget), 0, targetCheckingRate);
        }
    }

    private void StopLookingForTarget()
    {
        if (targets.Count < 1 && isLookingForTarget)
        {
            visualIndicator.transform.SetParent(null);
            visualIndicator.SetActive(false);

            isLookingForTarget = false;
            CancelInvoke(nameof(CheckAndSetClosestTarget));
        }
    }

    private void CheckAndSetClosestTarget()
    {
        if (targets.Count == 0) return;

        float minDistance = float.MaxValue;

        foreach (Transform target in targets)
        {
            float currentTargetDistance = (transform.position - target.position).magnitude;

            if (currentTargetDistance < minDistance)
            {
                minDistance = currentTargetDistance;
                closestTarget = target;
            }
        }

        visualIndicator.SetActive(true);
        visualIndicator.transform.parent = closestTarget;
        visualIndicator.transform.localPosition = Vector3.zero;
    }
}
