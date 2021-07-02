using System;
using System.Collections.Generic;

namespace SimpleBT.Nodes
{
    public class Parallel: Node
    {
        private List<Node> _childrenToRun = new List<Node>();

        protected override void OnStart()
        {
            _childrenToRun.Clear();
            _childrenToRun.AddRange(Children);
        }

        protected override Status OnUpdate()
        {
            foreach (var child in _childrenToRun.ToArray())
            {
                child.Execute(currentContext);
                switch (child.Status)
                {
                    case Status.Running:
                        continue;
                    case Status.Fail:
                        return Status.Fail;
                    case Status.Success:
                        _childrenToRun.Remove(child);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (_childrenToRun.Count > 0)
            {
                return Status.Running;
            }

            return Status.Success;
        }
    }
}