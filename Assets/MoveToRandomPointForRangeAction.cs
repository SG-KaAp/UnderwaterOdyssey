using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move to Random Point for Range", story: "Moves the NPС to a random point", category: "Action/Navigation", id: "d21fcb21569b45fd6394ba8df4a493c7")]
public partial class MoveToRandomPointForRangeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> agentGameObject;
    [SerializeReference] public BlackboardVariable<float> range;
    [SerializeReference] public BlackboardVariable<Vector3> randomPoint;
    protected override Status OnStart()
    {
        NavMeshAgent agent = agentGameObject.Value.GetComponentInChildren<NavMeshAgent>();
        randomPoint.Value = agentGameObject.Value.transform.position + UnityEngine.Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint.Value, out hit, range, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

