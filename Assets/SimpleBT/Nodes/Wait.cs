using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    public class Wait: Node
    {
        private FloatParameter duration = 0;

        private float _elapsed;
        private float _lastTime ;
        protected override void OnStart()
        {
            _elapsed = 0;
            _lastTime = Time.time;
        }

        protected override Status OnUpdate()
        {
            _elapsed += (Time.time - _lastTime);
            _lastTime = Time.time;
            
            if (_elapsed >= duration.Value)
            {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}