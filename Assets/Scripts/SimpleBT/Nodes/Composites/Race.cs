using System;
using System.Collections.Generic;

namespace SimpleBT.Nodes.Composites
{
    public class Race: Node
    {
        protected override Status OnUpdate()
        {
            foreach (var child in Children)
            {
                child.Execute(currentContext);
                switch (child.Status)
                {
                    case Status.Running:
                        continue;
                    case Status.Fail:
                        return Status.Fail;
                    case Status.Success:
                        foreach (var otherChild in Children)
                        {
                            if (otherChild == child)
                            {
                                continue;
                            }
                            otherChild.Reset();
                        }
                        return Status.Success;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Status.Running;
        }
    }
}