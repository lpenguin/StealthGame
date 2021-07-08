using SimpleBT.Attributes;
using UnityEngine.AI;

namespace SimpleBT.Nodes.Navigation
{
    [Name("Nav.Stop")]
    public class Stop: Node
    {
        private NavMeshAgent _agent;

        protected override void OnStart()
        {
            _agent = currentContext.GameObject.GetComponent<NavMeshAgent>();
        }

        protected override Status OnUpdate()
        {
            _agent.isStopped = true;
            return Status.Success;
        }
    }
}