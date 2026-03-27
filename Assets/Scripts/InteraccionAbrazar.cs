using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class InteraccionAbrazar : MonoBehaviour
{
    PlayerInput playerInput;
    public bool handWihtCat = false;
    public bool abrazo = false;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (handWihtCat)
            {
                if(playerInput.actions["Interact"].IsPressed())
                {
                    abrazo = true;
                }
                else
                {
                    abrazo = false;
                }
            }
        }
    }
        
    // Update is called once per frame
    void Update()
    {
        
        
        
    }
}
