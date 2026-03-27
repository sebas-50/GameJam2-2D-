using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    [SerializeField] private float verticalAmplitude;
    [SerializeField] private float verticalFrecuency;

    [SerializeField] private float horizontalAmplitude;
    [SerializeField] private float horizontalFrecuency;

    [SerializeField] private float angleAmplitude;

    [SerializeField] private bool isMoving = false;

    public void StartMoving()
    {
        isMoving = true;
    }

    public void StopMoving()
    {
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
            transform.localPosition = new Vector3(verticalAmplitude * Mathf.Sin(horizontalFrecuency * Time.time), verticalAmplitude * Mathf.Abs(Mathf.Sin(verticalFrecuency * Time.time)), 0f);
        }

    }
}
