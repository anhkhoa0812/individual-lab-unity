using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Editor : PropertyAttribute 
{
    public class TypeMismatchFixAttribute : PropertyAttribute { }

    [CustomPropertyDrawer(typeof(TypeMismatchFixAttribute))]
    public class TypeMismatchFixDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.ObjectField(position, property.objectReferenceValue, typeof(Object), true);

            EditorGUI.EndProperty();
        }
    }
}
