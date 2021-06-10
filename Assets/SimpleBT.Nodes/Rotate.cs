using SimpleBT.Attributes;
using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    [Name("Transform.Rotate")]
    public class Rotate: Node
    {
        private QuaternionParameter rotation = Quaternion.identity;
        private FloatParameter speed = 100f;
        private FloatParameter stopAngle = Mathf.PI * 0.05f;
        protected override Status OnUpdate()
        {
            var transform = currentContext.GameObject.transform;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation.Value, speed.Value * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, rotation.Value) < stopAngle.Value)
            {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}