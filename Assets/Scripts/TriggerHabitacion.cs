using UnityEngine;

public class TriggerHabitacion : MonoBehaviour
{
    public bool habConPlayer = false;
    
    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Algo está en el trigger: " + collision.gameObject.name);
    if (collision.CompareTag("Player"))
    {
        habConPlayer = true;
        Debug.Log("JUGADOR DENTRO");
    }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            habConPlayer = false;
        }
    } 
}