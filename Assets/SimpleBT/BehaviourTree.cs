using System;

namespace SimpleBT
{
    [Serializable]
    public class BehaviourTree
    {
        public BehaviourTree(Node root)
        {
            this.root = root;
        }

        public Node root { get; set; }
        
    }
}