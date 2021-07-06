using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleBT.Nodes.Navigation
{
    [Name("Nav.GoTo")]
    public class GoTo: Node
    {
        private Vector3Parameter target;
        private FloatParameter stopDistance = float.NaN;
        private BoolParameter updateOnlyIfChanged = true;

        private NavMeshAgent _navAgent;
        private Vector3 _prevPosition;
        
        protected override void OnStart()
        {
            _navAgent = currentContext.GameObject.GetComponent<NavMeshAgent>();
            
            _prevPosition = Vector3.negativeInfinity;
            
            if (float.IsNaN(stopDistance))
            {
                stopDistance = _navAgent.stoppingDistance;
            }

            _prevPosition = target;
            
            _navAgent.isStopped = false;
            
            if (!_navAgent.SetDestination(target))
            {
                Debug.LogWarning($"{Time.frameCount}: Cannot request SetDestination");
            }
        }

        protected override Status OnUpdate()
        {
            if (!updateOnlyIfChanged || _prevPosition != target)
            {
                if (!_navAgent.SetDestination(target))
                {
                    Debug.LogWarning($"{Time.frameCount}: Cannot request SetDestination");
                    return Status.Fail;
                }
            }

            _prevPosition = target;
            
            if (!_navAgent.pathPending)
            {
                if (_navAgent.remainingDistance <= stopDistance)
                {
                    return Status.Success;    
                }

                if (!_navAgent.hasPath)
                {
                    Debug.LogWarning($"{Time.frameCount}: Cannot find path");
                    return Status.Fail;
                }
                
            }

            return Status.Running;
        }
        
        protected override void OnAbort()
        {
            if (_navAgent != null)
            {
                _navAgent.isStopped = true;
            }
        }
        
        protected override void OnEnd()
        {
            if (_navAgent != null)
            {
                _navAgent.isStopped = true;
            }
        }
    }
}