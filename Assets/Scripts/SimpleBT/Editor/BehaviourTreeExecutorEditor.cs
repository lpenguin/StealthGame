using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SimpleBT.Editor
{
    [CustomEditor(typeof(BehaviourTreeExecutor))]
    public class BehaviourTreeExecutorEditor: UnityEditor.Editor
    {
        private SerializedProperty scriptFile;
        private SerializedProperty executeOnUpdate;
        private SerializedProperty blackboard;
        private Dictionary<Node, bool> isFolded = new Dictionary<Node, bool>();
        private Dictionary<string, bool> isFoldedSubtrees = new Dictionary<string, bool>();
        private string addName;
        private int typeIndex = 0;
        
        void OnEnable()
        {
            scriptFile = serializedObject.FindProperty("scriptFile");
            executeOnUpdate = serializedObject.FindProperty("executeOnUpdate");
            blackboard = serializedObject.FindProperty("blackboard").FindPropertyRelative("parameters");
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
            if (executor.tree != null)
            {
                if (Application.isPlaying)
                {
                    if (GUILayout.Button("Step", GUILayout.Width(90)))
                    {
                        executor.Step();
                    }                
                }
            }
            
            EditorGUILayout.PropertyField(blackboard);

            executor.EnsureTreeLoaded();

        //

            //
            //     if (executor.tree.subTrees.Count > 0)
            //     {
            //         EditorGUILayout.Space();
            //         EditorGUILayout.LabelField("Subtrees", EditorStyles.boldLabel);
            //         // ShowSubtrees(executor.tree);
            //     }
            //     EditorGUILayout.Space();
            //     EditorGUILayout.LabelField("Root node", EditorStyles.boldLabel);
            //     if (executor.tree.name != null)
            //     {
            //         EditorGUILayout.LabelField(executor.tree.name);
            //     }
            //     
            //     
            //     // ShowNode(executor.tree.root);
            // }
            // else
            // {
            //     EditorGUILayout.LabelField("Assign tree script");
            // }
            // EditorGUILayout.Space();
            // EditorGUILayout.LabelField("Blackboard", EditorStyles.boldLabel);
            // ShowBlackboard(executor.blackboard);
            serializedObject.ApplyModifiedProperties();
        }

        // private void ShowSubtrees(BehaviourTree tree)
        // {
        //     foreach (var kv in tree.subTrees)
        //     {
        //         var name = kv.Key;
        //         var node = kv.Value;
        //         var color = GetStatusColor(node.Status);
        //         
        //         if (!isFoldedSubtrees.TryGetValue(name, out var foldout))
        //         {
        //             foldout = false;
        //             isFoldedSubtrees[name] = foldout;
        //         }
        //         
        //         var style = GetFoldoutStyle(color);
        //         foldout = EditorGUILayout.Foldout(foldout, name, style);
        //         isFoldedSubtrees[name] = foldout;
        //         if (foldout)
        //         {
        //             EditorGUI.indentLevel += 1;
        //             ShowNode(node);
        //             EditorGUI.indentLevel -= 1;
        //         }
        //     }
        // }
        //
        // void ShowBlackboard(Blackboard bb)
        // {
        //     EditorGUI.BeginChangeCheck();
        //     foreach (var param in bb.Parameters)
        //     {
        //         EditorGUILayout.BeginHorizontal();
        //         
        //         var newName = EditorGUILayout.TextField(param.Name);
        //         if (newName != param.Name)
        //         {
        //             bb.RenameParameter(param.Name, newName);
        //             this.Repaint();
        //             break;
        //         }
        //         
        //         if (param.Type == typeof(Vector3))
        //         {
        //             param.Value = EditorGUILayout.Vector3Field("", (Vector3)param.Value);    
        //         }
        //         else if (param.Type == typeof(string))
        //         {
        //             param.Value = EditorGUILayout.TextField((string) param.Value);
        //         }
        //         else if (param.Type == typeof(bool))
        //         {
        //             param.Value = EditorGUILayout.Toggle((bool)param.Value);    
        //         }else if (param.Type == typeof(float))
        //         {
        //             param.Value = EditorGUILayout.FloatField((float)param.Value);    
        //         }else if (param.Type == typeof(double))
        //         {
        //             param.Value = EditorGUILayout.DoubleField((double)param.Value);    
        //         }else if (param.Type == typeof(Transform))
        //         {
        //             param.Value = EditorGUILayout.ObjectField((UnityEngine.Object)param.Value, typeof(Transform), true);    
        //         }else if (param.Type == typeof(int))
        //         {
        //             param.Value = EditorGUILayout.IntField((int)param.Value);    
        //         }else if (param.Type == typeof(Quaternion))
        //         {
        //             var euler = ((Quaternion) param.Value).eulerAngles;
        //             
        //             var newEuler = EditorGUILayout.Vector3Field("", euler);
        //             if (newEuler != euler)
        //             {
        //                 param.Value = Quaternion.Euler(euler);
        //             }
        //         }
        //         else if (param.Type == typeof(string[]))
        //         {
        //             var strArr = (param.Value as string[]) ?? new string[0];
        //             
        //             EditorGUILayout.LabelField($"{string.Concat(strArr)}");    
        //         }
        //         else
        //         {
        //             EditorGUILayout.LabelField($"{param.Value}");    
        //         }
        //
        //         if (GUILayout.Button("x"))
        //         {
        //             bb.DeleteParameter(param.Name);
        //             
        //             this.Repaint();
        //             break;
        //         }
        //         
        //         EditorGUILayout.EndHorizontal();
        //         
        //     }
        //     EditorGUILayout.BeginHorizontal();
        //     addName = EditorGUILayout.TextField(addName);
        //     var keys = Blackboard.Types.Keys.ToArray();
        //     typeIndex = EditorGUILayout.Popup(typeIndex, keys);
        //     if (GUILayout.Button("Add"))
        //     {
        //         var addType = Blackboard.Types[keys[typeIndex]];
        //         bb.AddParameter(addName, addType);
        //         this.Repaint();
        //     }
        //     EditorGUILayout.EndHorizontal();
        //     if (EditorGUI.EndChangeCheck())
        //     {
        //         Undo.RecordObject(target, "Blackboard update");
        //     }
        //     
        // }
        //     
        // void ShowNode(Node node)
        // {
        //     if (node == null)
        //     {
        //         Debug.LogWarning("Node is null");
        //         return;
        //     }
        //     
        //     if (!isFolded.TryGetValue(node, out var foldout))
        //     {
        //         foldout = !(node is SimpleBT.Nodes.Tree);
        //         isFolded[node] = foldout;
        //     }
        //
        //     // Debug.Log($"{node.Name} ({node.Children.Count}) {foldout}");
        //     // EditorGUILayout.BeginFoldoutHeaderGroup(foldout, node.Name);
        //     // EditorGUILayout.EndFoldoutHeaderGroup();
        //     // EditorGUILayout.LabelField($"{node.Name} ({node.Children.Count})");
        //
        //     var color = GetStatusColor(node.Status);
        //
        //
        //     List<string> parameters = new List<string>();
        //     var namedChildren = new List<(string name, Node node)>();
        //     
        //     
        //     foreach (var param in node.Parameters)
        //     {
        //         string value;
        //         if (param.BbName != null)
        //         {
        //             value = $"[{param.BbName}]";
        //         }
        //         else
        //         {
        //             object objValue = param.Get();
        //             if (objValue is string[] objValueStr)
        //             {
        //                 value = string.Join(", ", objValueStr);
        //             }
        //             else if (param.Type.IsAssignableFrom(typeof(Node)) && objValue != null)
        //             {
        //                 namedChildren.Add((param.Name, objValue as Node));
        //                 continue;
        //             }
        //             else
        //             {
        //                 value = objValue?.ToString() ?? "null";
        //             }
        //         }
        //         
        //         parameters.Add($"{param.Name}: {value}");
        //     }
        //
        //     var parametersStr = string.Join(", ", parameters);
        //     
        //     namedChildren.AddRange(node.Children.Select(c => ((string)null, c)));
        //     if(namedChildren.Count > 0)
        //     {
        //         var style = GetFoldoutStyle(color);
        //         string label = "";
        //         
        //         
        //         label = string.IsNullOrEmpty(node.Comment) ? node.Name : $"{node.Comment} ({node.Name})";
        //
        //         label = string.IsNullOrEmpty(node.Id) ? label : $"{label} #{node.Id}";
        //
        //         label = foldout ? label : $"{label} +{node.Children.Count}";
        //         if (!string.IsNullOrEmpty(parametersStr))
        //         {
        //             label = $"{label} {parametersStr}";
        //         }
        //         
        //         // EditorGUILayout.BeginHorizontal();
        //         foldout = EditorGUILayout.Foldout(foldout, label, style);
        //         // if (!string.IsNullOrEmpty(node.Comment))
        //         // {
        //         //     // label += " // " + node.Comment;
        //         //     var labelStyle = new GUIStyle(EditorStyles.label);
        //         //     labelStyle.normal.textColor = new Color32(0x85, 0xC4, 0x6C, 0xFF);
        //         //     EditorGUILayout.LabelField(" // " + node.Comment, labelStyle);
        //         //     GUILayout.FlexibleSpace();
        //         // }
        //         // EditorGUILayout.EndHorizontal();
        //         
        //         isFolded[node] = foldout;
        //
        //         if (!foldout)
        //         {
        //             return;
        //         }
        //
        //         EditorGUI.indentLevel += 1;
        //         foreach (var child in namedChildren)
        //         {
        //             if (!string.IsNullOrEmpty(child.name))
        //             {
        //                 EditorGUILayout.LabelField(child.name + ":");
        //                 EditorGUI.indentLevel += 1;
        //             }
        //             // Debug.Log($"  Child {child.Name}");
        //             ShowNode(child.node);
        //             
        //             if (!string.IsNullOrEmpty(child.name))
        //             {
        //                 EditorGUI.indentLevel -= 1;
        //             }
        //         }
        //         EditorGUI.indentLevel -= 1;
        //         return;
        //     }
        //
        //     {
        //         var style = new GUIStyle(EditorStyles.label) {normal = {textColor = color}};
        //         EditorGUILayout.LabelField($"{node.Name} {parametersStr}", style);
        //         
        //     }
        //
        //
        // }
        //
        // private static GUIStyle GetFoldoutStyle(Color color)
        // {
        //     var style = new GUIStyle(EditorStyles.foldout)
        //     {
        //         normal = {textColor = color},
        //         active = {textColor = color},
        //         focused = {textColor = color},
        //         hover = {textColor = color},
        //         onNormal = {textColor = color},
        //         onActive = {textColor = color},
        //         onFocused = {textColor = color},
        //         onHover = {textColor = color},
        //     };
        //     return style;
        // }
        //
        // private static Color GetStatusColor(Status nodeStatus)
        // {
        //     Color color;
        //     switch (nodeStatus)
        //     {
        //         case Status.Empty:
        //             color = Application.isPlaying ? Color.gray : EditorStyles.label.normal.textColor;
        //             break;
        //         case Status.Running:
        //             color = new Color32(0x65, 0x95, 0xEB, 0xFF);
        //             break;
        //         case Status.Fail:
        //             color = new Color32(0xED, 0x94, 0xC0, 0xFF);
        //             break;
        //         case Status.Success:
        //             color = new Color32(0x39, 0xCC, 0x8F, 0xFF);
        //             break;
        //         // case Status.Interrupted:
        //             // color = new Color32(0xC9, 0xA2, 0x6D, 0xFF);
        //             // break;
        //         default:
        //             throw new ArgumentOutOfRangeException();
        //     }
        //
        //     return color;
        // }
    }
}