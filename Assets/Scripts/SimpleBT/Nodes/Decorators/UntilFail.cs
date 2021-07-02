using System;
using UnityEngine.Assertions;

namespace SimpleBT.Nodes
{
    public class UntilFail: Node
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
                    return Status.Fail;
                case Status.Success:
                    child.Reset();
                    return Status.Running;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}