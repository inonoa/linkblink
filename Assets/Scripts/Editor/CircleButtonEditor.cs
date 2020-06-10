using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CanEditMultipleObjects, CustomEditor(typeof(CircleButton),true)]
public class CircleButtonEditor : ButtonEditor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        this.serializedObject.Update();
        EditorGUILayout.PropertyField(this.serializedObject.FindProperty("radius"), true);
        this.serializedObject.ApplyModifiedProperties();
    }
}