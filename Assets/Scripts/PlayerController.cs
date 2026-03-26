using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float movY;
    private float movX;
    public float Speed;
    PlayerInput playerInput;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       playerInput = GetComponent<PlayerInput>(); 
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movY = playerInput.actions["Move"].ReadValue<Vector2>().y;
        movX = playerInput.actions["Move"].ReadValue<Vector2>().x;
        Vector2 movement = new Vector2(movX, movY) * Speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
}
