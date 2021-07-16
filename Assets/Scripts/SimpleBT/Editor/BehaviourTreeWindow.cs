using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SimpleBT.Editor
{
    public class BehaviourTreeWindow: EditorWindow
    {
        [SerializeField] 
        TreeViewState nodeTreeViewState;

        private NodeTreeView _nodeTreeView;
        private BehaviourTreeExecutor _currentExecutor;
        void OnEnable ()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (nodeTreeViewState == null)
                nodeTreeViewState = new TreeViewState ();

            _nodeTreeView = new NodeTreeView(null, nodeTreeViewState);
            EnsureExecutor();
        }

        private void OnHierarchyChange()
        {
            EnsureExecutor();
            Repaint();
        }

        private void OnSelectionChange()
        {
            EnsureExecutor();
            Repaint();
        }

        private void OnNodeContentsChanged(Node node)
        {
            _nodeTreeView.Reload();
        }

        private void EnsureExecutor()
        {
            var executor = Selection.activeGameObject?.GetComponent<BehaviourTreeExecutor>();
            if (executor != _currentExecutor)
            {
                if (_currentExecutor != null)
                {
                    _currentExecutor.onStep.RemoveListener(OnExecutorStep);
                    _currentExecutor.onContentsChanged.RemoveListener(OnNodeContentsChanged);
                }
                
                _currentExecutor = executor;
                
                if (_currentExecutor != null)
                {
                    _currentExecutor.onStep.AddListener(OnExecutorStep);
                    _currentExecutor.onContentsChanged.AddListener(OnNodeContentsChanged);
                }
            }
        }

        private void OnExecutorStep()
        {
            Repaint();
        }
        
        void OnGUI ()
        {
            if (_currentExecutor == null)
            {
                EditorGUILayout.LabelField("Select GameObject with the BehaviorTreeExecutor first");
                return;
            }
            
            _currentExecutor.EnsureTreeLoaded();
            if (_currentExecutor.tree == null)
            {
                EditorGUILayout.LabelField("No tree is assigned to the BehaviorTreeExecutor");
                return;
            }
            _nodeTreeView.SetRoot(_currentExecutor.tree.root);
            _nodeTreeView.OnGUI(new Rect(0, 0, position.width, position.height));
        }
        
        // Add menu named "My Window" to the Window menu
        [MenuItem ("SimpleBT/Behavior Tree Window")]
        static void ShowWindow ()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<BehaviourTreeWindow> ();
            window.titleContent = new GUIContent ("Behavior Tree Window");
            window.Show ();
        }
    }
}