using System;
using System.Collections.Generic;

namespace SimpleBT
{
    [Serializable]
    public class BehaviourTree
    {
        public string name { get; set; }
        public Node root { get; set; }
        public Dictionary<string, Node> subTrees { get; set; } = new Dictionary<string, Node>();
        public Dictionary<string, Node> nodeById {get; set;} = new Dictionary<string, Node>();
        
        public List<BBParameter> blackboardParameters = new List<BBParameter>();

        private static int _lastNumericId = 0;
        public static int GenerateNumericId()
        {
            _lastNumericId += 1;
            return _lastNumericId;
        }
    }
}