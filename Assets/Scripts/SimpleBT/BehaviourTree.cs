using System;
using System.Collections.Generic;

namespace SimpleBT
{
    [Serializable]
    public class BehaviourTree
    {
        public BehaviourTree()
        {
        }

        public string name { get; set; }
        public Node root { get; set; }
        public Dictionary<string, Node> subTrees { get; set; } = new Dictionary<string, Node>();

        public List<BBParameter> blackboardParameters = new List<BBParameter>();

    }
}