using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
/*
For Unity Inspector
Change the view of Score Info Configurator
this object have a links between name of object and link to other object with data about object score, sprite and exc
 */


/*
 Define that ScoreInfoConfigurator will be changed
 */
[CustomEditor(typeof(ScoreInfoConfigurator))]
public class ScoreConfiguratorEditor : Editor
{
    /*
    active instance of our object (for future collecting info)
    */
    private ScoreInfoConfigurator scoreInfoConfigurator;

    /*
    define we have one list (with a link to object data) in our new brand UI inspector
    */
    private ReorderableList listScoreInfo;

    /*
     other properties when object has no data in list
    */
    private SerializedProperty nullSpriteScoreProperty;
    private SerializedProperty nullScoreProperty;

    /*
    This work on open inspector in unity (yep, it can be closed, than this function doesn't work)
    */
    private void OnEnable()
    {
        /*draw our data list*/
        DrawTable();

        /*and define our null properties*/
        nullSpriteScoreProperty = serializedObject.FindProperty("nullSpriteScore");
        nullScoreProperty = serializedObject.FindProperty("nullScore");
    }


    /*
    that's going in inspector view and when we interact with it  
     */
    public override void OnInspectorGUI()
    {

        /*update informaton in UI from object*/
        serializedObject.Update();

        /*draw a line with that text*/
        EditorGUILayout.LabelField("Score Sets", EditorStyles.boldLabel);

        /*
        create our list 
        */
        listScoreInfo.DoLayoutList();


        EditorGUILayout.PropertyField(nullSpriteScoreProperty);
        EditorGUILayout.PropertyField(nullScoreProperty);

        serializedObject.ApplyModifiedProperties();
    }

    /*
     draw our talbe with links
     */
    void DrawTable()
    {
        /*
        define our end list as new reaordable list
        ScoreConfiguratorEditor - our auidoPlayerConfigurator
        and listScoreSet - list, which we want to show
        */
        listScoreInfo = new ReorderableList(
              serializedObject
            , serializedObject.FindProperty("listScoreSet")
            , true
            , true
            , true
            , true
            );


        /*
        make the header of our two column space
        */
        listScoreInfo.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(CalculateColumn(rect, 1, 15, 0), "Object Name");
            EditorGUI.LabelField(CalculateColumn(rect, 2, 15, 0), "Score Info");
        };

        /*
        put/show each elements in that table from list
        */
        listScoreInfo.drawElementCallback = (Rect rect, int index, bool isActives, bool isFocused) =>
        {
            /*we get one element for list*/
            var element = listScoreInfo.serializedProperty.GetArrayElementAtIndex(index);

            /*add spacing*/
            rect.y += 2;

            /*and draw each part of that element in calculated Ractangle*/
            EditorGUI.PropertyField(CalculateColumn(rect, 1, 0, 0), element.FindPropertyRelative("scoreName"), GUIContent.none);
            EditorGUI.PropertyField(CalculateColumn(rect, 2, 10, 10), element.FindPropertyRelative("scoreInfo"), GUIContent.none);

        };
    }

    /*
     * create rect for drawing elements
     */
    Rect CalculateColumn(Rect rect, int columnNumber, float xPadding, float xWidth)
    {
        float xPosition = rect.x;
        switch (columnNumber)
        {
            case 1:
                xPosition = rect.x + xPadding;
                break;
            case 2:
                xPosition = rect.x + rect.width / 2 + xPadding;
                break;

        }

        return new Rect(xPosition, rect.y, rect.width / 2 - xWidth, EditorGUIUtility.singleLineHeight);
    }
    
}
