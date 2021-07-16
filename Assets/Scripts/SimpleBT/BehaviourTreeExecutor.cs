using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace SimpleBT
{
    public class BehaviourTreeExecutor: MonoBehaviour
    {
        public TextAsset scriptFile;
        [SerializeField]
        [HideInInspector]
        public BehaviourTree tree;
        public bool executeOnUpdate = true;
        [SerializeField]
        public Blackboard blackboard = new Blackboard();
        private ExecutionContext _context = new ExecutionContext();
        private EventBus _eventBus = new EventBus();

        [NonSerialized]
        public UnityEvent onStep = new UnityEvent();
        [NonSerialized]
        public NodeUnityEvent onContentsChanged = new NodeUnityEvent(); 
        public EventBus EventBus => _eventBus;
        
        public void EnsureTreeLoaded()
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
            foreach (var parameter in tree.blackboardParameters)
            {
                if (!blackboard.HasParameter(parameter.Name))
                {
                    blackboard.AddParameter(parameter);
                }
            }

            _context = new ExecutionContext()
            {
                GameObject = gameObject,
                Blackboard = blackboard,
                BehaviourTree = tree,
                EventBus = _eventBus,
                OnContentsChanged = onContentsChanged
            };
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

            if ((tree.root.Status & (Status.Success | Status.Fail)) == 0)
            {
                tree.root.Execute(_context);
                onStep.Invoke();
            }
        }
    }
}