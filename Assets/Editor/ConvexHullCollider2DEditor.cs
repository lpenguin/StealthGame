using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ConvexHullCollider2D))]
public class ConvexHullCollider2DEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSceneGUI()
    {
        // var t = (target as LookAtPoint);

        // EditorGUI.BeginChangeCheck();
        // Vector3 pos = Handles.PositionHandle(t.lookAtPoint, Quaternion.identity);
        // if (EditorGUI.EndChangeCheck())
        // {
        //     Undo.RecordObject(target, "Move point");
        //     t.lookAtPoint = pos;
        //     t.Update();
        // }
    }
}
