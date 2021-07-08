using System;
using System.Collections.Generic;
using System.Linq;
using SimpleBT.Nodes;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace SimpleBT.Editor
{
    public class TreeViewItem<T>: TreeViewItem
    {
        public TreeViewItem(T data)
        {
            Data = data;
        }

        public T Data { get; }
    }
    public class NodeTreeView: TreeView
    {
        private Node _root;
        public NodeTreeView(Node root, TreeViewState state) : base(state)
        {
            _root = root;
            // Reload();
        }

        public NodeTreeView(Node root, TreeViewState state, MultiColumnHeader multiColumnHeader) : base(state, multiColumnHeader)
        {
            _root = root;
            // Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            return BuildNodeForRoot();
            // return BuildNode(_root, -1);
        }

        private TreeViewItem BuildNodeForRoot()
        {
            var treeNode = new TreeViewItem<Node>(null)
            {
                id = 0,
                depth = -1,
                displayName = "Root",
            };
            treeNode.AddChild(BuildNode(_root, 0));
            return treeNode;
        }
        private TreeViewItem<Node> BuildNode(Node node, int depth)
        {

            var treeNode = new TreeViewItem<Node>(node)
            {
                id = node.NumericId,
                depth = depth,
                displayName = GetNodeName(node),
            };

            foreach (var child in node.Children)
            {
                treeNode.AddChild(BuildNode(child, depth + 1));
            }

            int iParam = 0;
            foreach (var param in node.Parameters)
            {
                object objValue = param.Get();
                if (param.Type != typeof(object) && param.Type.IsAssignableFrom(typeof(Node)) && objValue != null)
                {
                    var paramTreeNode = new TreeViewItem<string>(param.Name)
                    {
                        id = treeNode.id * 10000 + iParam,
                        depth = depth + 1,
                        displayName = param.Name + ":",
                    };
                    iParam += 1;
                    var paramNode = objValue as Node;
                    paramTreeNode.AddChild(BuildNode(paramNode, depth + 2));
                    treeNode.AddChild(paramTreeNode);
                    
                }

            }
            return treeNode;
        }
        protected override void RowGUI (RowGUIArgs args)
        {
            var rect = args.rowRect;
            
            float num = GetContentIndent(args.item) + extraSpaceBeforeIconAndLabel;
            rect.xMin += num;
            args.rowRect = rect;
            
            if (args.item is TreeViewItem<Node> nodeItem)
            {
                var color = GetStatusColor(nodeItem.Data.Status);
                var style = new GUIStyle(EditorStyles.label) {normal = {textColor = color}};

                GUI.Label(args.rowRect, GetNodeName(nodeItem.Data), style);
            }
            
            if (args.item is TreeViewItem<string> stringItem)
            {
                GUI.Label(args.rowRect, stringItem.Data + ":");
            }
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
                case Status.Fail:
                    color = new Color32(0xED, 0x94, 0xC0, 0xFF);
                    break;
                case Status.Success:
                    color = new Color32(0x39, 0xCC, 0x8F, 0xFF);
                    break;
                // case Status.Interrupted:
                // color = new Color32(0xC9, 0xA2, 0x6D, 0xFF);
                // break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return color;
        }

        private string GetNodeName(Node node)
        {
            List<string> parameters = new List<string>();
            
            foreach (var param in node.Parameters)
            {
                string value;
                if (param.BbName != null)
                {
                    value = $"[{param.BbName}]";
                }
                else
                {
                    object objValue = param.Get();
                    if (objValue is string[] objValueStr)
                    {
                        value = string.Join(", ", objValueStr);
                    }
                    else if (param.Type.IsAssignableFrom(typeof(Node)) && objValue != null)
                    {
                        continue;
                    }
                    else
                    {
                        value = objValue?.ToString() ?? "null";
                    }
                }
                
                parameters.Add($"{param.Name}: {value}");
            }

            var parametersStr = string.Join(", ", parameters);

            string label;
                
            label = string.IsNullOrEmpty(node.Comment) ? node.Name : $"{node.Comment} ({node.Name})";

            label = string.IsNullOrEmpty(node.Id) ? label : $"{label} #{node.Id}";

            // label = foldout ? label : $"{label} +{node.Children.Count}";
            label = $"{label} (id: {node.NumericId}) {parametersStr}";

            return label;
        }

        // protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
        // {
        //     if (!(root is TreeViewItem<Node> nodeRoot))
        //     {
        //         throw new Exception("Invalid tree view item type");
        //     }
        //
        //     var res = nodeRoot.Data.Children
        //         .Select(c => new TreeViewItem<Node>(c)
        //         {
        //             id = c.NumericId,
        //             displayName = c.Name,
        //             depth = root.depth + 1,
        //             parent = root.parent,
        //         })
        //         .ToArray();
        //     return res;
        // }

        public void SetRoot(Node root)
        {
            bool needReload = _root != root;
            _root = root;
            if (needReload)
            {
                Reload();
                ExpandAll();
            }
        }
    }
}