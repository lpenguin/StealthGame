using UnityEngine;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(SimpleGraph))]
public class MyGraphEditor : NodeGraphEditor {
    public override void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Button ("Button!");
        GUILayout.Button ("Button!");
        GUILayout.EndHorizontal();
        UnityEditor.EditorGUILayout.LabelField("The value is ");
        UnityEditor.EditorGUI.LabelField(new Rect(100, 200, 300, 400), "The value is ");

    }
}