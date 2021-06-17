using SimpleBT.Attributes;
using UnityEngine;

namespace SimpleBT.Nodes.Robot
{
    [Name("Robot.LookAround")]
    public class LookAround : Node
    {
        private Parameter<float> degrees = new Parameter<float>(90);
        private Parameter<float> speed  = new Parameter<float>(3);

        private Quaternion _initialRotation;
        private Transform _transform;
        private bool _rotatingLeft = true;

        private const float EPSILON =  0.9999999f;

        protected override void OnStart()
        {
            _transform = currentContext.GameObject.transform;
            _initialRotation = _transform.rotation;
        }

        protected override Status OnUpdate()
        {
            var target = Quaternion.AngleAxis((_rotatingLeft ? -1 : 1) * degrees.Value, _transform.up) * _initialRotation;
            _transform.rotation = Quaternion.Lerp(_transform.rotation, target, speed.Value * Time.deltaTime);
        
            var abs = Mathf.Abs(Quaternion.Dot(target, _transform.rotation));
            if(abs >= EPSILON){
                if(_rotatingLeft){
                    _rotatingLeft = false;
                } else {
                    return Status.Success;
                }
            }

            return Status.Running;
        }
    }
}
