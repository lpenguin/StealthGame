using System.IO;
using UnityEngine;

namespace SimpleBT
{
    public class BehaviourTreeExecutor: MonoBehaviour
    {
        public TextAsset scriptFile;
        public BehaviourTree tree;
        public bool executeOnUpdate = true;
        [SerializeField]
        public Blackboard blackboard = new Blackboard();
        public void LoadIfNeeded()
        {
            if (scriptFile == null)
            {
                tree = null;
                return;
            }
            
            if (tree == null || tree.root == null)
            {
                LoadTree();
            }
        }

        public void LoadTree()
        {
            var reader = new StringReader(scriptFile.text);
            var parser = new YamlParser();
            tree = parser.ParseTree(reader);
        }

        public void Awake()
        {
            LoadTree();
        }

        private void Update()
        {
            if (executeOnUpdate)
            {
                Step();
            }
        }

        public void Step()
        {
            var context = new ExecutionContext()
            {
                GameObject = gameObject,
                Blackboard = blackboard
            };
            
            tree.root.Execute(context);
        }
    }
}