using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    [SerializeField] private float stepsFrecuency;

    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float horizontalAmplitude;

    [SerializeField] private float angleAmplitude;

    [SerializeField] private bool isMoving = false;

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
        transform.localPosition = Vector3.zero;
        isMoving = false;
    }

    public void SetFacingDirection(bool isFacingRight)
    {
        if (isFacingRight)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.localPosition = new Vector3(horizontalAmplitude * Mathf.Sin(Mathf.PI/2f + stepsFrecuency * Time.time), verticalAmplitude * Mathf.Abs(Mathf.Sin(stepsFrecuency * Time.time)), 0f);
        }
    }
}
