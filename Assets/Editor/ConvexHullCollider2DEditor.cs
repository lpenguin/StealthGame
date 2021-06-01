using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ConvexHullCollider2D))]
public class ConvexHullCollider2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        var t = target as ConvexHullCollider2D;
        if (t == null)
        {
            return;
        }
        
        var hull = t.ConvexHull;
        
        int numPoints = hull.Points.Count;
        
        EditorGUI.BeginChangeCheck();
        int newNumPoints = EditorGUILayout.IntSlider("Num Vertices", numPoints, 3, 24);
        if (newNumPoints != numPoints)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Change num points");
                Vector2[] points = new Vector2[newNumPoints];
                for (int i = 0; i < newNumPoints; i++)
                {
                    float angle = Mathf.PI * 2 * i / newNumPoints;
                    points[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                }
                t.ConvexHull.SetPoints(points);
                t.ConvexHullChanged.Invoke();
            }
        }
    }
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
