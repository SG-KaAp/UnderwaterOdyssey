using UnityEngine;
using UnityEngine.AI;

public class EnemyNPC : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    public void MoveToRandomPointForRange(float range)
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}