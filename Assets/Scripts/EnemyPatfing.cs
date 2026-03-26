using UnityEngine;
using UnityEngine.AI;
using NavMeshPlus;
public class EnemyPatfing : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent navMeshAgent;
    public Transform myTransform;
    void Start()
    {
       navMeshAgent = GetComponent<NavMeshAgent>(); 
       myTransform = transform;
       navMeshAgent.updateRotation = false;
       navMeshAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Esto de aqui hace que cuando llegue a un waypoint, lo cambie al siguiente, para que siga la ruta forever
        if (waypoints != null && waypoints.Length > 0)
        {
            GameObject currentWaypoint = waypoints[currentWaypointIndex];
            if (currentWaypoint != null)
            {
                navMeshAgent.SetDestination(currentWaypoint.transform.position); //este es el que mueve al enemigo
                
                if (Vector2.Distance(myTransform.position, currentWaypoint.transform.position) < 0.5f)
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
            }
        }
        
    }
}
