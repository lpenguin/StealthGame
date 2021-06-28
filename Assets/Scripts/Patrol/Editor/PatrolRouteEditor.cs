using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Patrol.Editor
{
    [CustomEditor(typeof(PatrolRoute))]
    public class PatrolRouteEditor: UnityEditor.Editor
    {
        public void OnSceneGUI()
            {
                Vector3 Vector2ToVector3(Vector2 v){
                    return new Vector3(v.x, 0, v.y);
                }
                
                Vector2 Vector3ToVector2(Vector3 v){
                    return new Vector2(v.x, v.z);
                }
                
                var t = (target as ConvexHullCollider2D);
        
                var hull = t.ConvexHull;
                var points = hull.Points.ToArray();
                
                EditorGUI.BeginChangeCheck();
        
                var trPos = t.transform.position;
                var transformation = Matrix4x4.Translate(trPos) * Matrix4x4.Rotate(t.transform.rotation) ;
                var inverseTransformation = transformation.inverse;
                
                // Vector3 pos = Handles.PositionHandle(Vector2ToVector3(points[0]) + trPos, Quaternion.identity);
                Vector3 snap = new Vector3(1, 0, 1);
                bool wasUpdate = false;
                // var rot = Camera.current.transform.rotation;
                for (var index = 0; index < points.Length; index++)
                {
                    
                    var point = transformation.MultiplyPoint3x4(Vector2ToVector3(points[index]));
                    var size = HandleUtility.GetHandleSize(point);
                    Vector3 pos = Handles.FreeMoveHandle( point, 
                        Quaternion.identity, 
                        size / 10,
                        snap, 
                        Handles.SphereHandleCap);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        // Debug.Log("Move point");
                        Undo.RecordObject(target, "Move point");
                        points[index] = Vector3ToVector2(inverseTransformation.MultiplyPoint3x4(pos));
                        wasUpdate = true;
                        // break;
                    }
                }
        
                if (wasUpdate)
                {
                    hull.SetPoints(points);
                    t.ConvexHullChanged.Invoke();
                }
        
            }
    }
}