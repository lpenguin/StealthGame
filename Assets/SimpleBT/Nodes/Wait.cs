using SimpleBT.Parameters;
using UnityEngine;

namespace SimpleBT.Nodes
{
    public class Wait: Node
    {
        private FloatParameter duration = 0;

        private float _elapsed;

        protected override void OnStart()
        {
            _elapsed = duration.Value;
        }

        protected override Status OnUpdate()
        {
            _elapsed -= Time.deltaTime;
            if (_elapsed < 0)
            {
                return Status.Success;
            }

            return Status.Running;
        }
    }
}