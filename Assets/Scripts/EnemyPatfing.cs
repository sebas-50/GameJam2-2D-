using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;

public class EnemyPatfing : MonoBehaviour
{
    [Header("Waypoints")]
    public GameObject[] waypoints;
    public GameObject[] waypointsHuir;
    private int currentWaypointIndex = 0;

    [Header("Jugador")]
    public GameObject player;
    private Transform playerTransform;
    public float Vision = 5f;
    public float distanciaHuir = 8f;
    private bool playerDetected = false;

    [Header("Componentes")]
    private NavMeshAgent navMeshAgent;
    private Transform myTransform;
    private int layerMask;
    private Vector3 ultimoDestinoHuir;

    [Header("Gato")]
    public GameObject cat;
    private Vector3 posOriCat;
    private CatGrabber catGrabber;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        catGrabber = GetComponent<CatGrabber>();
        
        if (cat != null) posOriCat = cat.transform.position;

        layerMask = LayerMask.GetMask("Obstacles");
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        if (player != null) playerTransform = player.transform;
    }

    void Update()
    {
        if (playerTransform == null) return;
                
        Vector2 directionToPlayer = playerTransform.position - myTransform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < Vision)
        {
            RaycastHit2D hit = Physics2D.Raycast(myTransform.position, directionToPlayer.normalized, distanceToPlayer, layerMask);

            Debug.DrawRay(myTransform.position, directionToPlayer.normalized * distanceToPlayer, Color.red);
            
            if (hit.collider == null)
            {
                playerDetected = true;
            }
            else
            {
                playerDetected = false;
            }
        }
        else
        {
            playerDetected = false;
        }
        
        float distanciaActual = Vector2.Distance(myTransform.position, playerTransform.position);

        if (catGrabber != null && catGrabber.hasCat && (playerDetected || distanciaActual < distanciaHuir))
        {
            HuirDinamico();
        }
        else if (playerDetected && catGrabber != null && !catGrabber.hasCat && cat != null)
        {
            navMeshAgent.SetDestination(cat.transform.position);
        }
        else if (waypoints != null && waypoints.Length > 0 && catGrabber != null && !catGrabber.hasCat)
        {
            Patrullar();
        }
        else if (catGrabber != null && catGrabber.hasCat && !playerDetected)
        {
            navMeshAgent.SetDestination(posOriCat);

            if (Vector2.Distance(myTransform.position, posOriCat) < 1f)
            {
                catGrabber.DropCatTowardsRandomDirection();
            }
        }
    }

    void Patrullar()
    {
        GameObject currentWaypoint = waypoints[currentWaypointIndex];
        if (currentWaypoint != null)
        {
            navMeshAgent.SetDestination(currentWaypoint.transform.position);

            if (Vector2.Distance(myTransform.position, currentWaypoint.transform.position) < 0.5f)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
        }
    }

    void HuirDinamico()
    {
        if (waypointsHuir == null || waypointsHuir.Length == 0) return;

        if (navMeshAgent.hasPath && navMeshAgent.remainingDistance > 2f)
        {
            return;
        }

        float mejorPuntaje = -999f;
        GameObject mejorWaypoint = null;

        foreach (GameObject wp in waypointsHuir)
        {
            if (wp == null) continue;

            Vector3 posWP = wp.transform.position;
            
            float distanciaAlJugador = Vector2.Distance(posWP, player.transform.position);
            float factorLejania = distanciaAlJugador;
            
            Vector2 direccionWP = (posWP - myTransform.position).normalized;
            Vector2 direccionJugador = (player.transform.position - myTransform.position).normalized;
            float angulo = Vector2.Angle(direccionWP, direccionJugador);
            float factorDireccion = angulo / 180f;
            
            float aleatorio = Random.Range(0.7f, 1.3f);

            float puntaje = (factorLejania * factorDireccion) * aleatorio;
            
            if (puntaje > mejorPuntaje)
            {
                mejorPuntaje = puntaje;
                mejorWaypoint = wp;
            }
        }

        if (mejorWaypoint != null)
        {
            navMeshAgent.SetDestination(mejorWaypoint.transform.position);
            ultimoDestinoHuir = mejorWaypoint.transform.position;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Vision);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaHuir);
    }
}