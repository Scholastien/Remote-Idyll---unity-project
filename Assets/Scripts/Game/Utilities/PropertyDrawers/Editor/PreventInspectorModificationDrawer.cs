using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PreventInspectorModification))]
public class PreventInspectorModificationDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Change the returned property height to the double to cater for the additional item code description
        return EditorGUI.GetPropertyHeight(property) * 2;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var obj = property.serializedObject.targetObject;

        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.String)
        {
            EditorGUI.BeginChangeCheck(); // start check for changed values

            // Draw item code
            var newValue = EditorGUI.TextField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.stringValue);

            if (GUI.Button(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Assign New GUID"))
            {
                var type = obj.GetType();
                if (type == typeof(ItemDetails))
                {
                    ItemDetails item = (ItemDetails)obj;
                    item.AssignGUID();
                    EditorUtility.SetDirty(obj);
                }
            }

            // if item code value has changed, set the value to new value
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = property.stringValue;

                EditorUtility.SetDirty(obj);
            }
        }
        EditorGUI.EndProperty();
    }



}