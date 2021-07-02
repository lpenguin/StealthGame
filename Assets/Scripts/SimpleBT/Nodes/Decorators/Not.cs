using System;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes.Decorators
{
    public class Not: Node
    {
        protected override Status OnUpdate()
        {
            Assert.AreEqual(Children.Count, 1, "Not can only contain 1 child");
            var child = Children[0];
            child.Execute(currentContext);
            switch (child.Status)
            {
                case Status.Running:
                    return Status.Running;
                case Status.Fail:
                    return Status.Success;
                case Status.Success:
                    return Status.Fail;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}