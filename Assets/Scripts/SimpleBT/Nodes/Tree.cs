using System;
using SimpleBT.Parameters;

namespace SimpleBT.Nodes
{
    public class Tree: Node
    {
        private StringParameter name;

        private string _currentName;
        protected override void OnStart()
        {
            if (name.Value == null)
            {
                throw new Exception($"Subtree name can not be null");
            }

            if (_currentName != name.Value)
            {
                _currentName = name.Value;
                ClearAndLoadSubtree();
            }
        }

        private void ClearAndLoadSubtree()
        {
            var subTrees = currentContext.BehaviourTree.subTrees;
            if (!subTrees.TryGetValue(name.Value, out var subTreeNode))
            {
                throw new Exception($"Cannot find subtree \"{name.Value}\"");
            }

            ClearChildren();
            AddChild(subTreeNode.Clone());
            currentContext.OnContentsChanged.Invoke(this);
        }

        protected override Status OnUpdate()
        {
            var subTreeNode = Children[0];
            // if ((subTreeNode.Status & (Status.Empty | Status.Running)) == 0)
            // {
            //     Reset();
            // }
            //
            subTreeNode.Execute(currentContext);
            return subTreeNode.Status;
        }
    }
}