using UnityEngine.Assertions;
using Patrol;
using SimpleBT.Parameters;
using SimpleBT.Attributes;
using UnityEditor.Experimental.GraphView;


namespace SimpleBT.Nodes.Patrol
{
    [Name("Patrol.NextPoint")]
    public class PatrolNextPoint: PatrolCurrentPoint
    {
        protected override Status OnUpdate(){
            index.Value = route.NextIndex(index.Value);

            return base.OnUpdate();
        }

    }
}