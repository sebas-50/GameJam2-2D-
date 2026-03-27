using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightAntorcha : MonoBehaviour
{
    public float Distancia = 5f;
    public GameObject player;
    private Light2D light2D;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.transform.position, transform.position);
        
        light2D.enabled = distance < Distancia;
    }
}