using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Patrol.Editor
{

    [CustomEditor(typeof(PatrolRoute))]
    public class PatrolRouteEditor: UnityEditor.Editor
    {
        private static Tool LastTool = Tool.None;
        

        void OnEnable()
        {
            // LastTool = Tools.current;
            var t = (target as PatrolRoute);
    
            if(t == null){
                return;
            }

            var info = t.editorInfo;
            info.editingRoute = false;
            info.selectedIndex = -1;
            // Tools.current = Tool.None;
        }
     
        void OnDisable()
        {
            // Tools.current = LastTool;
        }

        public override void OnInspectorGUI()
        {

            var t = (target as PatrolRoute);
    
            if(t == null){
                return;
            }

            var info = t.editorInfo;

            EditorGUI.BeginChangeCheck();

            bool editingRoute = GUILayout.Toggle(info.editingRoute, "Edit positions");
            if(EditorGUI.EndChangeCheck()){
                info.editingRoute = editingRoute;
                if(info.editingRoute && (Tools.current != Tool.Move || Tools.current != Tool.Rotate)){
                    Tools.current = Tool.Move;
                }
                info.selectedIndex = -1;
            }

            base.OnInspectorGUI();
        }

        public void OnSceneGUI()
        {
            var t = (target as PatrolRoute);
    
            if(t == null){
                return;
            }

            var info = t.editorInfo;

            var points = t.rawPoints;
            var transform = t.transform;
            
            Vector3 prevPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;

            for (var index = 0; index < points.Length; index++)
            {
                var patrolPoint = points[index];

                Vector3 worldPosition = transform.TransformPoint(patrolPoint.position);
                Quaternion worldRotation = transform.rotation * patrolPoint.rotation;
                
                float size = HandleUtility.GetHandleSize(worldPosition) * .1f;

                Handles.SphereHandleCap(
                    0,
                    worldPosition,
                    Quaternion.identity,
                    size,
                    EventType.Repaint
                );

                if(patrolPoint.rotate){
                    Handles.ArrowHandleCap(
                        0,
                        worldPosition,
                        worldRotation,
                        size * 5, 
                        EventType.Repaint);                    
                }                
                
                if(!info.editingRoute)
                {

                } 
                else if(info.selectedIndex == index)
                {
                    switch (Tools.current)
                    {
                        case Tool.Move:
                        {
                            EditorGUI.BeginChangeCheck();
                            worldPosition = Handles.PositionHandle(worldPosition, worldRotation);

                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(target, "Move point");
                                patrolPoint.position = transform.InverseTransformPoint(worldPosition);
                            }

                            break;
                        }
                        case Tool.Rotate:
                        {
                            EditorGUI.BeginChangeCheck();
                            worldRotation = Handles.RotationHandle(worldRotation, worldPosition);

                            if (EditorGUI.EndChangeCheck())
                            {
                                Undo.RecordObject(target, "Rotate point");
                                patrolPoint.rotation = Quaternion.Inverse(transform.rotation) * worldRotation;
                            }

                            break;
                        }
                    }
                }
                else 
                {
                    bool selected = Handles.Button(worldPosition, worldRotation, size, size, Handles.SphereHandleCap);
                    if (selected){
                        info.selectedIndex = index;
                    }
                }



                string label = string.IsNullOrEmpty(patrolPoint.treeName)
                    ? $"{index}"
                    : $"{index} ({patrolPoint.treeName})";

                // label = $"<font color=\"green\">{label}</font>";
                GUIStyle labelStyle = new GUIStyle();
                labelStyle.normal.textColor = Color.green;
                
                Handles.Label(worldPosition, label, labelStyle);

                Handles.color = info.editingRoute ? Color.green : Color.white;

                if(index == 0){
                    firstPoint = worldPosition;
                }

                if(index > 0){
                    Handles.DrawLine(prevPoint, worldPosition, 1f);
                }

                if(t.patrolMode == PatrolRoute.PatrolMode.Cycle && index == points.Length - 1){
                    Handles.DrawLine(firstPoint, worldPosition, 1f);
                }

                Handles.color = Color.white;

                prevPoint = worldPosition;
            }
        }
    }
}