using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerEnemy : MonoBehaviour
{
    public GameObject player;
    public GameObject sonrojo;
    public GameObject caricia;
    private EnemyPatfing enemyPatfing;
    private CatGrabber catGrabber;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float distanciaParaInteractuar = 2f;

    void Start()
    {
        enemyPatfing = GetComponent<EnemyPatfing>();
        catGrabber = GetComponent<CatGrabber>();
    }
    void Update()
    {
        if (enemyPatfing.enabled) distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
    }
    public void Abrazito()
    {
        if (enemyPatfing != null && enemyPatfing.enabled)
        {            
            if (distanceToPlayer < distanciaParaInteractuar)
            {
                enemyPatfing.enabled = false;
                catGrabber.enabled = false;
                sonrojo.SetActive(true);
            }
        }
    } 
    public void CariciaActive()
    {
        caricia.SetActive(true);
    }
    public void CariciaOff()
    {
        caricia.SetActive(false);
    }

}