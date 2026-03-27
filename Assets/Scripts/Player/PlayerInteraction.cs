using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private CatGrabber catGrabber;

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
            switch (closestTarget.tag)
            {
                case "Cat":
                    catGrabber.GrabCat(closestTarget.GetComponent<Cat>());
                break;

                case "Enemy":
                    closestTarget.GetComponent<CatGrabber>().DropCatTowardsDirection(transform.position - closestTarget.position);
                break;

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hola");
        if (collision.CompareTag("Enemy") || collision.CompareTag("Cat"))
        {
            Debug.Log("Entro en seleccion");

            targets.Add(collision.transform);

            if (!isLookingForTarget)
            {
                isLookingForTarget = true;
                InvokeRepeating(nameof(CheckAndSetClosestTarget), 0, targetCheckingRate);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Adios");
        if (collision.CompareTag("Enemy") || collision.CompareTag("Cat"))
        {
            Debug.Log("Salió de seleccion");
            targets.Remove(collision.transform);

            if (targets.Count < 1)
            {
                visualIndicator.transform.parent = null;
                visualIndicator.SetActive(false);

                isLookingForTarget = false;
                CancelInvoke(nameof(CheckAndSetClosestTarget));
            }
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
