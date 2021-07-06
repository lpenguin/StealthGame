using System;
using Random = UnityEngine.Random;

namespace SimpleBT.Nodes.Composites
{
    public class RandomSelector: Node
    {
        private int? _currentTask;
        protected override void OnStart()
        {
            _currentTask = null;
        }

        protected override Status OnUpdate()
        {
            if (_currentTask == null)
            {
                _currentTask = Random.Range(0, Children.Count);
            }

            var child = Children[_currentTask.Value];
            child.Execute(currentContext);
            switch (child.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Fail:
                case Status.Success:
                    return child.Status;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}