using NavMeshPlus;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatfing : MonoBehaviour
{
    // === Waypoints ===
    public GameObject[] waypoints;
    public GameObject[] waypointsHuir;
    private int currentWaypointIndex = 0;

    // === Detección del jugador ===
    public GameObject player;
    private Transform playerTransform;
    public float Vision = 5f;
    private bool playerVisible = false;
    private bool playerDetected = false;

    // === Componentes ===
    private NavMeshAgent navMeshAgent;
    public Transform myTransform;
    private int layerMask;

    // === Gato ===
    public GameObject cat;
    public Transform posOriCat;
    private CatGrabber catGrabber;
    private AutomaticCatGrabbingTrigger automaticCatGrabbingTrigger;
    public TriggerHabitacion habitacion;

    void Start()
    {
        // Inicializar
        navMeshAgent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        catGrabber = GetComponent<CatGrabber>();
        automaticCatGrabbingTrigger = GetComponent<AutomaticCatGrabbingTrigger>();

        // Posición original del gato
        if (cat != null)
            posOriCat = cat.transform;

        // Configuración
        layerMask = LayerMask.GetMask("Obstacles");
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
        if (playerVisible)
        {
            Debug.Log("Jugador visible");
        }
        // ==========================================
        // 1. DETECCIÓN DEL JUGADOR
        // ==========================================
        if (playerTransform != null && habitacion != null && habitacion.habConPlayer)
        {
            Vector2 directionToPlayer = playerTransform.position - myTransform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer < Vision)
            {
                CheckLineOfSight(directionToPlayer.normalized, distanceToPlayer);
            }
            else
            {
                playerVisible = false;
            }
        }
        else
        {
            playerVisible = false;
        }

        // Actualizar estado de detección
        playerDetected = playerVisible && habitacion != null && habitacion.habConPlayer;

        // ==========================================
        // 2. COMPORTAMIENTO SEGÚN ESTADO
        // ==========================================
        // Caso 1: Detectó al jugador Y no tiene gato → va por el gato
        if (playerDetected && catGrabber != null && !catGrabber.hasCat && cat != null)
        {
            // Activar el collider del trigger si estaba desactivado
            if (automaticCatGrabbingTrigger != null)
            {
                automaticCatGrabbingTrigger.GetComponent<Collider2D>().enabled = true;
            }
            navMeshAgent.SetDestination(cat.transform.position);
        }
        // Caso 2: Detectó al jugador Y tiene gato → huye
        else if (playerDetected && catGrabber != null && catGrabber.hasCat)
        {
            Huir();
            // Si llegó, elegir nuevo waypoint
            if (navMeshAgent.remainingDistance < 0.5f)
            {
                Huir();
            }
        }
        // Caso 3: No detecta al jugador Y tiene gato → vuelve a dejar el gato
        else if (!playerDetected && catGrabber != null && catGrabber.hasCat && posOriCat != null)
        {
            navMeshAgent.SetDestination(posOriCat.position);

            if (Vector2.Distance(myTransform.position, posOriCat.position) < 1f)
            {
                catGrabber.DropCatTowardsRandomDirection();
                // Desactivar collider del trigger cuando suelta el gato
                if (automaticCatGrabbingTrigger != null)
                {
                    automaticCatGrabbingTrigger.GetComponent<Collider2D>().enabled = false;
                }
            }
        }
        // Caso 4: No detecta al jugador Y no tiene gato → patrulla normal
        else if (waypoints != null && waypoints.Length > 0)
        {
            Patrullar();
            // Asegurar que el collider del trigger esté activo para la próxima vez
            if (
                automaticCatGrabbingTrigger != null
                && !automaticCatGrabbingTrigger.GetComponent<Collider2D>().enabled
            )
            {
                automaticCatGrabbingTrigger.GetComponent<Collider2D>().enabled = true;
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

    void Huir()
    {
        if (waypointsHuir == null || waypointsHuir.Length == 0)
            return;

        float mejorPeso = -1f;
        GameObject waypointElegido = null;

        foreach (GameObject wp in waypointsHuir)
        {
            if (wp == null)
                continue;

            // Distancia desde el waypoint hasta el jugador
            float distanciaAlJugador = Vector2.Distance(
                wp.transform.position,
                player.transform.position
            );

            // Factor aleatorio (0 a 1)
            float aleatorio = Random.Range(0f, 1f);

            // Calcular peso: 70% distancia, 30% aleatorio
            float peso = (distanciaAlJugador * 0.7f) + (aleatorio * 0.3f);

            if (peso > mejorPeso)
            {
                mejorPeso = peso;
                waypointElegido = wp;
            }
        }

        if (waypointElegido != null)
        {
            navMeshAgent.SetDestination(waypointElegido.transform.position);
        }
    }

    void CheckLineOfSight(Vector2 normalizedDirection, float distanceToPlayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            myTransform.position,
            normalizedDirection,
            distanceToPlayer,
            layerMask
        );
        playerVisible = (hit.collider == null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Vision);
    }
}
