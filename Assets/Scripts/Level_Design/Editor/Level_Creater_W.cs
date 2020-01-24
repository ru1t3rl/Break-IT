using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level_Creater))]
public class Level_Creater_W : Editor
{
    bool initialized = false, generated = false;
    Level_Creater creator;
    Vector2Int prevSize = Vector2Int.zero;
    Vector2 prevMargin = Vector2.zero;
    GameObject prevBrick;

    GUIStyle LabelStyle = new GUIStyle();
    GUIStyle descStyle = new GUIStyle();
    GUIStyle titleStyle = new GUIStyle();

    GUIStyle lifeBox = new GUIStyle();

    public override void OnInspectorGUI()
    {
        if (!initialized)
        {
            creator = (Level_Creater)target;

            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.fontSize = 35;
            titleStyle.alignment = TextAnchor.UpperCenter;

            //LabelStyle.fontStyle = FontStyle.Bold;
            LabelStyle.fontSize = 12;

            descStyle.fontSize = 10;

            initialized = true;
        }

        GUILayout.Space(18);

        //////// Title ////////
        GUILayout.Label("Level Designer", titleStyle);
        //////////////////////

        GUILayout.Space(28);

        // Brick
        prevBrick = creator.brick;
        creator.brick = EditorGUILayout.ObjectField("Brick Prefab", creator.brick, typeof(GameObject), false) as GameObject;
        if (creator.brick != prevBrick)
            creator.UpdateBricksList();

        GUILayout.Space(20);

        // Brick Margin
        creator.prevMargin = creator.brickMargin;
        creator.brickMargin = EditorGUILayout.Vector2Field("Margin", creator.brickMargin);
        EditorGUILayout.HelpBox("The margin is the space between the bricks. X is for the horizontal spacing and Y for the vertical spacing.", MessageType.None);


        // Brick Spawn
        creator.prevSize = new Vector2Int(creator.size.x, creator.size.y);
        creator.size = EditorGUILayout.Vector2IntField("Field Size", creator.size);

        if (creator.size != creator.prevSize || creator.brickMargin != creator.prevMargin)
        {
            creator.UpdateBricksList();
        }

        GUILayout.Space(20);

        // Enable / Disable bricks
        GUILayout.Label("Brick Lives (0 means Disabled)");

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();
        for (int iCol = -1; iCol < creator.size.y; iCol++)
        {
            if (iCol < 0)
                GUILayout.Label(" ");
            else
                GUILayout.Label((creator.size.y - iCol).ToString());
        }
        EditorGUILayout.EndVertical();

        for (int iCol = 0; iCol < creator.size.x; iCol++)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label((iCol + 1).ToString() + " ");
            for (int iRow = creator.size.y; iRow-- > 0;)
            {
                try
                {
                    try
                    {
                        Brick b = creator.bricksList[iRow * creator.size.x + iCol].GetComponent<Brick>();
                        b.health = EditorGUILayout.IntField(b.health);
                        b.ToggleVisble(b.health > 0);
                    }
                    catch (MissingReferenceException)
                    {
                        creator.UpdateThemBricksList();
                    }
                }
                catch (KeyNotFoundException)
                {
                    creator.UpdateThemBricksList();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.HelpBox("When setup Correctly the bricks will pick a color based on their lives. If this is not the case at the \"Brick\" script to your prefab and set the materials.\n" +
            "(Material 0 will be used for one live and Material 5 for six lifes.)", MessageType.Info);


        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Enable All"))
        {
            creator.ToggleVisible(true);
        }
        else if (GUILayout.Button("Hide All"))
        {
            creator.ToggleVisible(false);
        }
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Update GameObject Brick"))
            creator.UpdateGameObject();
    }

}
