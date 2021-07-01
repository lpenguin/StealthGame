using UnityEngine.Assertions;
using Patrol;
using SimpleBT.Parameters;
using SimpleBT.Attributes;


namespace SimpleBT.Nodes.Patrol
{
    [Name("Patrol.CurrentPoint")]
    public class PatrolCurrentPoint: Node
    {
        protected TransformParameter patrolRoute;
        protected IntParameter index;
        protected Vector3Parameter position;
        protected BoolParameter rotate;
        protected QuaternionParameter rotation;
        protected StringParameter treeName;

        protected PatrolRoute route;
        
        protected override void OnStart()
        {
            route = patrolRoute.Value?.GetComponent<PatrolRoute>();
            Assert.IsNotNull(route, "route != null");
        }

        protected override Status OnUpdate(){
            var patrolTransform = patrolRoute.Value.transform;

            var patrolPoint = route.Points[index.Value];
            position.Value = patrolTransform.TransformPoint(patrolPoint.position);
            rotate.Value = patrolPoint.rotate;
            if (rotate.Value)
            {
                rotation.Value = patrolTransform.rotation * patrolPoint.rotation;
            }
            treeName.Value = patrolPoint.treeName;
            return Status.Success;
        }

    }
}