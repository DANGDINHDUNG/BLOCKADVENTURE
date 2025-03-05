using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShapeData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class ShapeDataDrawer : Editor
{
    private ShapeData shapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ClearBoardButton();
        EditorGUILayout.Space();

        DrawColumnsInputField();
        EditorGUILayout.Space();

        if (shapeDataInstance.board != null && shapeDataInstance.columns > 0 && shapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();


        if (GUI.changed)
        {
            EditorUtility.SetDirty(shapeDataInstance);
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            shapeDataInstance.Clear();
        }
    }

    private void DrawColumnsInputField()
    {
        var columnsTemp = shapeDataInstance.columns;
        var rowsTemp = shapeDataInstance.rows;

        shapeDataInstance.columns = EditorGUILayout.IntField("Columns", shapeDataInstance.columns);
        shapeDataInstance.rows = EditorGUILayout.IntField("Rows", shapeDataInstance.rows);

        if ((shapeDataInstance.columns != columnsTemp) || (shapeDataInstance.rows != rowsTemp) && 
            shapeDataInstance.columns > 0 && shapeDataInstance.rows > 0)
        {
            shapeDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle
        {
            fixedWidth = 25 * shapeDataInstance.columns,
            alignment = TextAnchor.MiddleCenter
        };

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid);
        dataFieldStyle.normal.background = Texture2D.grayTexture;
        dataFieldStyle.onNormal.background = Texture2D.whiteTexture;

        for (var row = 0; row < shapeDataInstance.rows; row++)
        {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for (var column = 0; column < shapeDataInstance.columns; column++)
            {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(shapeDataInstance.board[row].column[column], dataFieldStyle);
                shapeDataInstance.board[row].column[column] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}

