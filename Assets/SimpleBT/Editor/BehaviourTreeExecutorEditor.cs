using System;
using System.Collections.Generic;
using System.Linq;
using SimpleBT;
using UnityEditor;
using UnityEngine;

namespace BehaviourTreeUtils.Editor
{
    [CustomEditor(typeof(BehaviourTreeExecutor))]
    public class BehaviourTreeExecutorEditor: UnityEditor.Editor
    {
        private SerializedProperty scriptFile;
        private SerializedProperty executeOnUpdate;
        private Dictionary<Node, bool> isFolded = new Dictionary<Node, bool>();
        private Dictionary<string, bool> isFoldedSubtrees = new Dictionary<string, bool>();
        private string addName;
        private int typeIndex = 0;
        
        void OnEnable()
        {
            scriptFile = serializedObject.FindProperty("scriptFile");
            executeOnUpdate = serializedObject.FindProperty("executeOnUpdate");
        }
        
        public override void OnInspectorGUI()
        {
            var executor = target as BehaviourTreeExecutor;
            // serializedObject.Update();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(scriptFile);
            if (GUILayout.Button("Reload"))
            {
                executor.LoadTree();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(executeOnUpdate);
            
            
            executor.LoadIfNeeded();
            if (executor.tree != null)
            {
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Step"))
                    {
                        executor.Step();
                    }                
                }

                if (executor.tree.subTrees.Count > 0)
                {
                    EditorGUILayout.LabelField("Subtrees");
                    ShowSubtrees(executor.tree);
                }

                if (executor.tree.name != null)
                {
                    EditorGUILayout.LabelField(executor.tree.name);
                }
                ShowNode(executor.tree.root);
            }
            else
            {
                EditorGUILayout.LabelField("Assign tree script");
            }
            
            ShowBlackboard(executor.blackboard);
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowSubtrees(BehaviourTree tree)
        {
            foreach (var kv in tree.subTrees)
            {
                var name = kv.Key;
                var node = kv.Value;
                var color = GetStatusColor(node.Status);
                
                if (!isFoldedSubtrees.TryGetValue(name, out var foldout))
                {
                    foldout = true;
                    isFoldedSubtrees[name] = true;
                }
                
                var style = GetFoldoutStyle(color);
                foldout = EditorGUILayout.Foldout(foldout, name, style);
                isFoldedSubtrees[name] = foldout;
                if (foldout)
                {
                    EditorGUI.indentLevel += 1;
                    ShowNode(node);
                    EditorGUI.indentLevel -= 1;
                }
            }
        }

        void ShowBlackboard(Blackboard bb)
        {
            EditorGUI.BeginChangeCheck();
            foreach (var param in bb.Parameters)
            {
                EditorGUILayout.BeginHorizontal();
                
                var newName = EditorGUILayout.TextField(param.Name);
                if (newName != param.Name)
                {
                    bb.RenameParameter(param.Name, newName);
                    this.Repaint();
                    break;
                }
                
                if (param.Type == typeof(Vector3))
                {
                    param.Value = EditorGUILayout.Vector3Field("", (Vector3)param.Value);    
                }
                else if (param.Type == typeof(bool))
                {
                    param.Value = EditorGUILayout.Toggle((bool)param.Value);    
                }else if (param.Type == typeof(float))
                {
                    param.Value = EditorGUILayout.FloatField((float)param.Value);    
                }else if (param.Type == typeof(double))
                {
                    param.Value = EditorGUILayout.DoubleField((double)param.Value);    
                }else if (param.Type == typeof(Transform))
                {
                    param.Value = EditorGUILayout.ObjectField((UnityEngine.Object)param.Value, typeof(Transform), true);    
                }else if (param.Type == typeof(int))
                {
                    param.Value = EditorGUILayout.IntField((int)param.Value);    
                }else if (param.Type == typeof(Quaternion))
                {
                    var euler = ((Quaternion) param.Value).eulerAngles;
                    
                    var newEuler = EditorGUILayout.Vector3Field("", euler);
                    if (newEuler != euler)
                    {
                        param.Value = Quaternion.Euler(euler);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField($"{param.Value}");    
                }

                if (GUILayout.Button("x"))
                {
                    bb.DeleteParameter(param.Name);
                    
                    this.Repaint();
                    break;
                }
                
                EditorGUILayout.EndHorizontal();
                
            }
            EditorGUILayout.BeginHorizontal();
            addName = EditorGUILayout.TextField(addName);
            var keys = Blackboard.Types.Keys.ToArray();
            typeIndex = EditorGUILayout.Popup(typeIndex, keys);
            if (GUILayout.Button("Add"))
            {
                var addType = Blackboard.Types[keys[typeIndex]];
                bb.AddParameter(addName, addType);
                this.Repaint();
            }
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Blackboard update");
            }
            
        }
            
        void ShowNode(Node node)
        {
            if (node == null)
            {
                Debug.LogWarning("Node is null");
                return;
            }
            
            if (!isFolded.TryGetValue(node, out var foldout))
            {
                foldout = true;
                isFolded[node] = true;
            }
        
            // Debug.Log($"{node.Name} ({node.Children.Count}) {foldout}");
            // EditorGUILayout.BeginFoldoutHeaderGroup(foldout, node.Name);
            // EditorGUILayout.EndFoldoutHeaderGroup();
            // EditorGUILayout.LabelField($"{node.Name} ({node.Children.Count})");

            var color = GetStatusColor(node.Status);


            List<string> parameters = new List<string>();
            foreach (var param in node.Parameters)
            {
                var value = param.BbName != null ? $"[{param.BbName}]" : param.Get();
                
                parameters.Add($"{param.Name}: {value}");
            }

            var parametersStr = string.Join(", ", parameters);

            if(node.Children.Count > 0)
            {
                var style = GetFoldoutStyle(color);

                string label = foldout ? node.Name : $"{node.Name} ({node.Children.Count})";
                foldout = EditorGUILayout.Foldout(foldout, label, style);
                isFolded[node] = foldout;

                if (!foldout)
                {
                    return;
                }

                EditorGUI.indentLevel += 1;
                foreach (var child in node.Children)
                {
                    // Debug.Log($"  Child {child.Name}");
                    ShowNode(child);
                }
                EditorGUI.indentLevel -= 1;
                return;
            }

            {
                var style = new GUIStyle(EditorStyles.label) {normal = {textColor = color}};
                EditorGUILayout.LabelField($"{node.Name} {parametersStr}", style);
                
            }


        }

        private static GUIStyle GetFoldoutStyle(Color color)
        {
            var style = new GUIStyle(EditorStyles.foldout)
            {
                normal = {textColor = color},
                active = {textColor = color},
                focused = {textColor = color},
                hover = {textColor = color},
                onNormal = {textColor = color},
                onActive = {textColor = color},
                onFocused = {textColor = color},
                onHover = {textColor = color},
            };
            return style;
        }

        private static Color GetStatusColor(Status nodeStatus)
        {
            Color color;
            switch (nodeStatus)
            {
                case Status.Empty:
                    color = Application.isPlaying ? Color.gray : EditorStyles.label.normal.textColor;
                    break;
                case Status.Running:
                    color = new Color32(0x65, 0x95, 0xEB, 0xFF);
                    break;
                case Status.Failed:
                    color = new Color32(0xED, 0x94, 0xC0, 0xFF);
                    break;
                case Status.Success:
                    color = new Color32(0x39, 0xCC, 0x8F, 0xFF);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return color;
        }
    }
}