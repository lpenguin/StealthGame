using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
public class GoTo: Node
    {
        public Vector3Parameter target;

        private UnityEngine.AI.NavMeshAgent navAgent;

        /// <summary>Initialization Method of MoveToGameObject.</summary>
        /// <remarks>Check if GameObject object exists and NavMeshAgent, if there is no NavMeshAgent, the default one is added.</remarks>
        protected override void OnStart()
        {
            var gameObject = currentContext.GameObject;
            if (target == null)
            {
                Debug.LogError("The movement target of this game object is null", gameObject);
                return;
            }

            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
			navAgent.SetDestination(target.Value);
            
            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
            #else
                navAgent.Resume();
            #endif
        }

        /// <summary>Method of Update of MoveToGameObject.</summary>
        /// <remarks>Verify the status of the task, if there is no objective fails, if it has traveled the road or is near the goal it is completed
        /// y, the task is running, if it is still moving to the target.</remarks>
        protected override Status OnUpdate()
        {
            if (navAgent.isStopped)
            {
                navAgent.isStopped = false;
            }
            
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                return Status.Success;

            return Status.Running;
        }
        /// <summary>Abort method of MoveToGameObject </summary>
        /// <remarks>When the task is aborted, it stops the navAgentMesh.</remarks>
        protected override void OnAbort()
        {

        #if UNITY_5_6_OR_NEWER
            if(navAgent!=null)
                navAgent.isStopped = true;
        #else
            if (navAgent!=null)
                navAgent.Stop();
        #endif

        }
    }
}