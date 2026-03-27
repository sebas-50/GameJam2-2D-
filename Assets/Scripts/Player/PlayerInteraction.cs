using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float pettingTime;
    [SerializeField] private CatGrabber catGrabber;
    [SerializeField] private Collider2D interactionTrigger;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject visualIndicator;
    [SerializeField] private float targetCheckingRate;
    [SerializeField] private Animator playerAnimator;


    [SerializeField] private float minPurrTime;
    [SerializeField] private float maxPurrTime;

    private HashSet<Transform> targets;
    private Transform closestTarget;

    private bool isLookingForTarget = false;

    private Transform cacheTarget;

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
                    case "Cat": // agarrar gato
                        catGrabber.GrabCat(closestTarget.GetComponent<Cat>());
                        interactionTrigger.enabled = false;

                        AudioManager.Instance.PlayPlayerSFX("pickup_cat_01");
                        AudioManager.Instance.PlayCat("meow_04");
                        
                        Purr();

                        StopLookingForTarget();
                        break;

                    case "Enemy": // acariciar enemigo
                        cacheTarget = closestTarget;
                        closestTarget.GetComponent<CatGrabber>().DropCatTowardsDirection(transform.position - closestTarget.position);
                        closestTarget.GetComponent<ControllerEnemy>().RecievePat();
                        targets.Remove(closestTarget);

                        AudioManager.Instance.PlayEnemySFX("loved_02");
                        AudioManager.Instance.PlayPlayerSFX("hugging_01");
                        AudioManager.Instance.PlayCat("angry_cat_02");

                        playerAnimator.Play("Pet");
                        playerController.enabled = false;
                        Invoke(nameof(ResetPatting), pettingTime);
                        break;
                }

            }
            else // soltar gato
            {
                catGrabber.DropCatTowardsRandomDirection();
                interactionTrigger.enabled = true;

                LookForTarget();

                CancelPurr();
            }
        }
    }

    
    private void CancelPurr()
    {
        CancelInvoke(nameof(Purr));
    }

    private void Purr()
    {
        AudioManager.Instance.PlayCat("purr_03");

        Invoke(nameof(Purr), Random.Range(minPurrTime, maxPurrTime));
    }

    private void ResetPatting()
    {
        Debug.Log("Patting reset");
        playerController.enabled = true;
        playerAnimator.Play("Idle");

        cacheTarget.GetComponent<ControllerEnemy>().StopPatting();
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
        if (targets != null && targets.Count == 0) return;

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
