using NavMeshPlus;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatfing : MonoBehaviour
{

    public GameObject[] waypoints;
    public GameObject[] waypointsHuir;
    private int currentWaypointIndex = 0;

    public GameObject player;
    private Transform playerTransform;
    public float Vision = 5f;
    private bool playerVisible = false;
    private bool playerDetected = false;


    private NavMeshAgent navMeshAgent;
    public Transform myTransform;
    private int layerMask;


    public GameObject cat;
    public Vector3 posOriCat;
    private CatGrabber catGrabber;
    private AutomaticCatGrabbingTrigger automaticCatGrabbingTrigger;
    public TriggerHabitacion habitacion;

    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        myTransform = transform;
        catGrabber = GetComponent<CatGrabber>();
        automaticCatGrabbingTrigger = GetComponent<AutomaticCatGrabbingTrigger>();

        //if (cat != null) posOriCat = cat.transform;

        layerMask = LayerMask.GetMask("Obstacles");
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        if (player != null)
            playerTransform = player.transform;
    }

    void Update()
    {
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

        if (!playerDetected)
        {
            playerDetected = playerVisible && habitacion != null && habitacion.habConPlayer;
        }
        else if (habitacion.habConPlayer)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
        
        // Caso 1: Detectó al jugador Y no tiene gato → va por el gato
        if (playerDetected && catGrabber != null && !catGrabber.hasCat && cat != null)
        {
            if (automaticCatGrabbingTrigger != null)
            {
                automaticCatGrabbingTrigger.GetComponent<Collider2D>().enabled = true;
            }
            Debug.Log("Vamo a por el gato papu");
            navMeshAgent.SetDestination(cat.transform.position);
        }
        // Caso 2: Detectó al jugador Y tiene gato → huye
        else if (playerDetected && catGrabber != null && catGrabber.hasCat)
        {
            Huir();
            Debug.Log("Vamo a huir 1");
            if (navMeshAgent.remainingDistance < 0.5f)
            {
                Huir();
                Debug.Log("Vamo a huir 2");
            }
        }
        // Caso 3: No detecta al jugador Y tiene gato → vuelve a dejar el gato
        else if (!playerDetected && catGrabber != null && catGrabber.hasCat && posOriCat != null)
        {
            navMeshAgent.SetDestination(posOriCat);
            Debug.Log("Vamo a devolver el gato papu");

            if (Vector2.Distance(myTransform.position, posOriCat) < 1f)
            {
                catGrabber.DropCatTowardsRandomDirection();
                Debug.Log("Gato devuelto papu");
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
            float distanciaAlJugador = Vector2.Distance(
                wp.transform.position,
                player.transform.position
            );


            float aleatorio = Random.Range(0f, 1f);


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
