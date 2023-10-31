using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/*
For Unity Inspector
Change the view of Scene Reference
It's a base object with just a link to scene for other object with similar structure (like Level Scene Reference and @Custom level scene reference@) 
Used for MainMenu
Maybe will be used for Credits and logo
 */

/*
 Define that SceneReference will be changed
 */
[CustomEditor(typeof(SceneReference))]
public class SceneReferenceEditor : Editor
{

    /*
    active instance of our object (for future collecting info)
    */
    private SceneReference sceneReference;

    /*
    define the variables from our object
    First one is a link to scene
    second is a real path in OS to file of this scene (and it's fill itself by reference path)
    */
    public SerializedProperty levelSceneProperty;
    public SerializedProperty levelPathProperty;

    /*
    This work on open inspector in unity (yep, it can be closed, than this function doesn't work)
    */
    private void OnEnable()
    {
        Init();
    }


    /*
    that's going in inspector view and when we interact with it  
     */
    public override void OnInspectorGUI()
    {
        /*
         return our object to SceneREference's roots
         */
        sceneReference = (SceneReference)target;
        serializedObject.Update();

        /*
         get level path in OS using scene reference
         */
        sceneReference.levelPath = AssetDatabase.GetAssetPath(sceneReference.levelScene);


        /*
         draw our propirties
         */
        DrawProperties();

        /*
         update information in object after we change it
         */
        serializedObject.ApplyModifiedProperties();

    }
    /*initialization*/
    public virtual void Init()
    {
        levelSceneProperty = serializedObject.FindProperty("levelScene");
        levelPathProperty = serializedObject.FindProperty("levelPath");
    }

    public virtual void DrawProperties() {
        /*
 draw our propirties in separate lines
 */
        EditorGUILayout.LabelField("Scene Reference", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelSceneProperty);

        EditorGUILayout.LabelField("Scene Path Index [Don't touch]", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(levelPathProperty);

    }
}
