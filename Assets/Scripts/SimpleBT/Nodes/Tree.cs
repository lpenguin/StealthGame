using System;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class Tree: Node
    {
        private StringParameter name;
        private Node subTreeNode;

        protected override void OnStart()
        {
            if (name.Value == null)
            {
                throw new Exception($"Subtree name can not be null");
            }
            
            var subTrees = currentContext.BehaviourTree.subTrees;
            if (!subTrees.TryGetValue(name.Value, out subTreeNode))
            {
                throw new Exception($"Cannot find subtree \"{name.Value}\"");
            }
        }

        protected override void OnAbort()
        {
            Node.OnAbort(subTreeNode);
        }

        protected override Status OnUpdate()
        {
            if ((subTreeNode.Status & (Status.Empty | Status.Running)) == 0)
            {
                Reset();
            }
            
            subTreeNode.Execute(currentContext);
            return subTreeNode.Status;
        }

        protected override void OnEnd()
        {
            Node.OnEnd(subTreeNode);
        }


        public override void Reset()
        {
            subTreeNode?.Reset();
            base.Reset();
        }

        // public override void Interrupt()
        // {
        //     base.Interrupt();
        //     subTreeNode.Interrupt();
        // }
    }
}