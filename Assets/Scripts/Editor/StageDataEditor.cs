using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    int num_columns = 0;
    int num_rows = 0;
    NodeType type2Fill = NodeType.None;

    public override void OnInspectorGUI(){
        serializedObject.Update();
        
        var memo = serializedObject.FindProperty("memo");
        EditorGUILayout.PropertyField(memo);
        var diff = serializedObject.FindProperty("difficulty");
        EditorGUILayout.PropertyField(diff);
        var tags = serializedObject.FindProperty("newElementTags");
        EditorGUILayout.PropertyField(tags);
        var unit = serializedObject.FindProperty("_DistanceUnit");
        EditorGUILayout.PropertyField(unit);

        EditorGUILayout.Space(30);

        using(new EditorGUILayout.HorizontalScope()){

            EditorGUILayout.LabelField("Row", GUILayout.Width(50));
            num_rows = EditorGUILayout.IntField(num_rows, GUILayout.Width(30));

            EditorGUILayout.LabelField("Column", GUILayout.Width(50));
            num_columns = EditorGUILayout.IntField(num_columns, GUILayout.Width(30));

            type2Fill = (NodeType) EditorGUILayout.EnumPopup(type2Fill, GUILayout.Width(80));
        }

        if(GUILayout.Button("グリッド生成")){
            (target as StageData).CreateGrid(num_rows, num_columns, type2Fill);
        }

        EditorGUILayout.Space();

        var defaultColor = GUI.color;
        using(new EditorGUILayout.HorizontalScope()){

            var rows = serializedObject.FindProperty("_Rows");

            if(GUILayout.Button("+", GUILayout.Width(20), GUILayout.ExpandHeight(true))){
                for(int i = 0; i < rows.arraySize; i++){
                    var row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("_Nodes");
                    row.InsertArrayElementAtIndex(0);
                }
            }
            if(GUILayout.Button("-", GUILayout.Width(20), GUILayout.ExpandHeight(true))){
                for(int i = 0; i < rows.arraySize; i++){
                    var row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("_Nodes");
                    row.DeleteArrayElementAtIndex(0);
                }
            }

            using(new EditorGUILayout.VerticalScope()){

                if(GUILayout.Button("+")){
                    rows.InsertArrayElementAtIndex(0);
                }
                if(GUILayout.Button("-")){
                    rows.DeleteArrayElementAtIndex(0);
                }

                EditorGUILayout.Space(20);


                for(int i = 0; i < rows.arraySize; i++){
                    var row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("_Nodes");

                    using(new EditorGUILayout.HorizontalScope()){

                        for(int j = 0; j < row.arraySize; j++){

                            var node = row.GetArrayElementAtIndex(j);
                            NodeType type = (NodeType) node.enumValueIndex;
                            GUI.color = TypeDataHolder.Instance[type].EditorColor;
                            node.enumValueIndex = (int) (NodeType) EditorGUILayout.EnumPopup(type, GUILayout.MinWidth(30), GUILayout.Height(30));
                            //EditorGUILayout.EnumFlagsField((NodeType) row.GetArrayElementAtIndex(j).enumValueIndex);
                        }
                        GUI.color = defaultColor;
                    }
                }

                EditorGUILayout.Space();

                if(GUILayout.Button("-")){
                    rows.DeleteArrayElementAtIndex(rows.arraySize - 1);
                }
                if(GUILayout.Button("+")){
                    rows.InsertArrayElementAtIndex(rows.arraySize);
                }
            }

            if(GUILayout.Button("-", GUILayout.Width(20), GUILayout.ExpandHeight(true))){
                for(int i = 0; i < rows.arraySize; i++){
                    var row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("_Nodes");
                    row.DeleteArrayElementAtIndex(row.arraySize - 1);
                }
            }
            if(GUILayout.Button("+", GUILayout.Width(20), GUILayout.ExpandHeight(true))){
                for(int i = 0; i < rows.arraySize; i++){
                    var row = rows.GetArrayElementAtIndex(i).FindPropertyRelative("_Nodes");
                    row.InsertArrayElementAtIndex(row.arraySize);
                }
            }

        }
        GUI.color = defaultColor;

        EditorGUILayout.Space(30);

        if(GUILayout.Button("Testに入れて実行")){
            (target as StageData).GoToTestSequence();
            EditorApplication.ExecuteMenuItem("Edit/Play");
        }

        serializedObject.ApplyModifiedProperties();
    }
}
