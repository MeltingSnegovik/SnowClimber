using UnityEngine;
using UnityEditor;
using UnityEditorInternal;


/*
For Unity Inspector
Change the view of LevelSceneReference
(there are links to objects with with level information (like link to scene, name, score and ect.)  
 */

/*
 Define that LevelSceneReference will be changed
 */
[CustomEditor(typeof(LevelSceneReference))]


/*
 !child of SceneReferenceEditor
 */
public class LevelSceneReferenceEditor : SceneReferenceEditor
{
    /*
     active instance of our object (for future collecting info)
     */
    private LevelSceneReference levelSceneReference;


    /*
     properties LevelSceneReference from  we are going to show
    name, number(id), completed or not, time and high score
     */
    public SerializedProperty levelNameProperty;
    public SerializedProperty levelNumberProperty;
    public SerializedProperty levelCompletedProperty;
    public SerializedProperty timeComletedProperty;
    public SerializedProperty highScoreProperty;


    /*
 This work on open inspector in unity (yep, it can be closed, than this function doesn't work)
 */
    private void OnEnable() {
        Init();
    }

    public override void OnInspectorGUI()
    {
        /*
        return our object to LevelSceneReference's roots
        */
        levelSceneReference = (LevelSceneReference)target;

        /*update UI from object information*/
        serializedObject.Update();

        /*
        get level path in OS using scene reference
        */
        levelSceneReference.levelPath = AssetDatabase.GetAssetPath(levelSceneReference.levelScene);

        /*
        draw our propirties
        */
        DrawProperties();


        /*
        update information in object after we change it
        */
        serializedObject.ApplyModifiedProperties();

    }
    
    /*initialization
     start with parent init() (there are two fields with ref and path)
     */
    public override void Init()
    {
        base.Init();
        levelNameProperty = serializedObject.FindProperty("levelName");
        levelNumberProperty = serializedObject.FindProperty("levelNumber");
        levelCompletedProperty = serializedObject.FindProperty("levelCompleted");
        timeComletedProperty = serializedObject.FindProperty("timeComleted");
        highScoreProperty = serializedObject.FindProperty("highScore");
    }

    public override void DrawProperties()
    {
        /*
         * start with parent Draw
draw our propirties in separate lines
*/
        base.DrawProperties();
        
        EditorGUILayout.LabelField("Level Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelNameProperty);
        EditorGUILayout.PropertyField(levelNumberProperty);
        EditorGUILayout.PropertyField(levelCompletedProperty);
        EditorGUILayout.PropertyField(timeComletedProperty);
        EditorGUILayout.PropertyField(highScoreProperty);
    }
}
