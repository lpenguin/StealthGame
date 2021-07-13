using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace SimpleBT.Editor
{
    [CustomPropertyDrawer(typeof(BBParameter))]
    public class BBParameterEditor: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            // var guiC = 
        
            // Draw label
            // position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            // Calculate rects
        
            const int nameWidth = 90;
            const int typeWidth = 90;
            const int margin = 10;
            
            var indent = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.indentLevel = 0;
            // var nameRect = new Rect(position.x, position.y, nameWidth, position.height);
            var typeRect = new Rect(220 + margin, position.y, typeWidth, position.height);
            var valueRect = new Rect(typeRect.xMax + margin, position.y, position.width - typeRect.xMax + margin, position.height);
        
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            var typeProp = property.FindPropertyRelative("Type");
            var bbType = (BBParameterType) typeProp.enumValueIndex;
            var valueProp = property.FindPropertyRelative("value");
            
            // EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
            EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
            DrawValueProperty(valueRect, bbType, valueProp);
            // Don't make child fields be indented
        
        
            // EditorGUILayout.PropertyField(property.FindPropertyRelative("Name"));
            // EditorGUILayout.PropertyField(property.FindPropertyRelative("Type"));
        
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
        
            EditorGUI.EndProperty();
        }
        
        // public override VisualElement CreatePropertyGUI(SerializedProperty property)
        // {
        //     // Create property container element.
        //     var container = new VisualElement();
        //
        //     // Create property fields.
        //     var nameField = new PropertyField(property.FindPropertyRelative("Name"), "Fancy Name");
        //     var typeField = new PropertyField(property.FindPropertyRelative("Type"));
        //
        //     // Add fields to the container.
        //     container.Add(nameField);
        //     container.Add(typeField);
        //
        //     return container;
        // }

        private void DrawValueProperty(Rect valueRect, BBParameterType bbParameterType, SerializedProperty valueProp)
        {
            SerializedProperty realProp;
            switch (bbParameterType)
            {
                case BBParameterType.Bool:
                    realProp = valueProp.FindPropertyRelative("boolValue");
                    break;
                case BBParameterType.Float:
                    realProp = valueProp.FindPropertyRelative("floatValue");
                    break;
                case BBParameterType.Int:
                    realProp = valueProp.FindPropertyRelative("intValue");
                    break;
                case BBParameterType.String:
                    realProp = valueProp.FindPropertyRelative("stringValue");
                    break;
                case BBParameterType.Vector3:
                    realProp = valueProp.FindPropertyRelative("vector3Value");
                    break;
                case BBParameterType.Quaternion:
                    realProp = valueProp.FindPropertyRelative("quaternionValue");
                    break;
                case BBParameterType.Transform:
                    realProp = valueProp.FindPropertyRelative("transformValue");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bbParameterType), bbParameterType, null);
            }
            EditorGUI.PropertyField(valueRect, realProp, GUIContent.none);
            
        }
    }
}