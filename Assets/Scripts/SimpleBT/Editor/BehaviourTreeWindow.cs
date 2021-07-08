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
        
        void OnEnable ()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (nodeTreeViewState == null)
                nodeTreeViewState = new TreeViewState ();

            _nodeTreeView = new NodeTreeView(null, nodeTreeViewState);
        }
        
        void OnGUI ()
        {
            var executor = Selection.activeGameObject?.GetComponent<BehaviourTreeExecutor>();
            if (executor == null)
            {
                EditorGUILayout.LabelField("Select GameObject with the BehaviorTreeExecutor first");
                return;
            }
            executor.LoadIfNeeded();
            if (executor.tree == null)
            {
                EditorGUILayout.LabelField("No tree is assigned to the BehaviorTreeExecutor");
                return;
            }
            _nodeTreeView.SetRoot(executor.tree.root);
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