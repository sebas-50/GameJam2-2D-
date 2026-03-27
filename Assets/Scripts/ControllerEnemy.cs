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

    public void RecievePat()
    {
        enemyPatfing.enabled = false;
        catGrabber.enabled = false;
        sonrojo.SetActive(true);
        CariciaActive();
    } 

    private void CariciaActive()
    {
        caricia.SetActive(true);
    }

    public void StopPatting()
    {
        caricia.SetActive(false);
    }

}