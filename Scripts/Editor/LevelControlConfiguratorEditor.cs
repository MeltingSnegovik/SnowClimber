using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/*
For Unity Inspector
Change the view of Level Control Configurator
(there are links to objects with with level information (like link to scene, name, score and ect.)  
 */

/*
 Define that LevelControlConfigurator will be changed
 */
[CustomEditor(typeof(LevelControlConfigurator))]
public class LevelControlConfiguratorEditor : Editor
{
    /*
    active instance of our object (for future collecting info)
    */
    private LevelControlConfigurator levelControlConfigurator;

    /*
    define we have one list in our new brand UI inspector
    */
    private ReorderableList sceneSetsList;

    /*
    On Enable of inspector just draw our list
    */
    private void OnEnable()
    {
        DrawSceneSetsList();
    }

    /*
     Main function for drawing in UI
     */
    void DrawSceneSetsList() {


        /*
       define our end list as new reaordable list
       serializedObject - our levelControlConfigurator
       and "sceneSets" - list in "levelControlConfigurator", which we want to show
       */
        sceneSetsList = new ReorderableList(
            serializedObject
            , serializedObject.FindProperty("sceneSets")
            , true
            , true
            , true
            , true
            );

        /*
        make the header of our three column space
        */
        sceneSetsList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(CalculateColumn(rect, 1, 3, 15, 0), "Scene is Level");
            EditorGUI.LabelField(CalculateColumn(rect, 2, 3, 15, 0), "Scene's Name");
            EditorGUI.LabelField(CalculateColumn(rect, 3, 3, 15, 0), "Scene Reference");
        };


        /*
        put/show each elements in that table from list
        */
        sceneSetsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            /*we get one element for list*/
            var element = sceneSetsList.serializedProperty.GetArrayElementAtIndex(index);

            /*add spacing*/
            rect.y += 2;

            /*and draw that element: find each part of that element and draw it in Calculated rect*/
            EditorGUI.PropertyField(CalculateColumn(rect, 1, 3, 15, 0), element.FindPropertyRelative("isLevelScene"), GUIContent.none);
            EditorGUI.PropertyField(CalculateColumn(rect, 2, 3, 15, 0), element.FindPropertyRelative("sceneName"), GUIContent.none);
            EditorGUI.PropertyField(CalculateColumn(rect, 3, 3, 15, 0), element.FindPropertyRelative("sceneReference"), GUIContent.none);
        };
    }

    /*
    that's going in inspector view and when we interact with it  
    */
    public override void OnInspectorGUI()
    {
        /*update informaton in object*/
        serializedObject.Update();

        /*just draw a line with this string*/
        EditorGUILayout.LabelField("Scene References", EditorStyles.boldLabel);

        /*create our list*/
        sceneSetsList.DoLayoutList();

        /*update information in object after we change it*/
        serializedObject.ApplyModifiedProperties();
    }

    /*
 * create rect for drawing elements
 */
    Rect CalculateColumn(Rect rect, int columnNumber, int columnCount, float xPadding, float xWidth)
    {
        float xPosition = rect.x;
        float xCalcWidth = xWidth; 
        xPosition = rect.x + rect.width / columnCount * (columnNumber - 1);
        
        if (columnNumber != columnCount)
            xCalcWidth = rect.width / columnCount - xPadding;
        else
            xCalcWidth = rect.width / columnCount;

        return new Rect(xPosition, rect.y, xCalcWidth, EditorGUIUtility.singleLineHeight);
    }

}
