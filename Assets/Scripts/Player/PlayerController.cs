using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private Rigidbody2D rb;

    [SerializeField] private float speed;

    private float movY;
    private float movX;

    private void Start()
    {
       playerInput = GetComponent<PlayerInput>(); 
       rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        movY = playerInput.actions["Move"].ReadValue<Vector2>().y;
        movX = playerInput.actions["Move"].ReadValue<Vector2>().x;
        Vector2 movement = new Vector2(movX, movY) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
