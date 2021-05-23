using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(SimpleNode))]
public class SimpleNodeEditor : NodeEditor {
    private SimpleNode simpleNode;

    public override void OnBodyGUI() {
        if (simpleNode == null) simpleNode = target as SimpleNode;

        // Update serialized object's representation
        serializedObject.Update();

        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("a"));
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("b"));
        UnityEditor.EditorGUILayout.LabelField("The value is ");
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("otherGraph"));
        if(simpleNode.otherGraph != null){
            if(GUILayout.Button("Open Graph")){
                Debug.Log("Clicked");
                UnityEditor.AssetDatabase.OpenAsset(simpleNode.otherGraph);
            }
        }

        // Apply property modifications
        serializedObject.ApplyModifiedProperties();
    }
}