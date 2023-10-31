using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/*
For Unity Inspector
Change the view of Audio Player Settings  
(there are links "action " – "sound", like "flying"  - "sound of noisy flying.mp3" )
 */


/*
 Define that AudioPlayerSettings will be changed
 */
[CustomEditor(typeof(AudioPlayerSettings))]
public class AudioPlayerSettingsEditor : Editor
{
    /*
    active instance of our object (for future collecting info)
    */
    private AudioPlayerSettings audioPlayerSettings;
    /*
   define we have one list in our new brand UI inspector
   */
    private ReorderableList audioList;

    /*
     On Enable of inspector just draw our list
    */
    private void OnEnable()
    {
        DrawAudioList();
    }
    
    /*
    Main function for drawing in UI
    */
    void DrawAudioList() {

        /*
         define our end list as new reaordable list
        serializedObject - our auidoPlayerConfigurator
        and audioSetsList - list, which we want to show
         */
        audioList = new ReorderableList(
            serializedObject
            , serializedObject.FindProperty("audioSetsList")
            , true
            , true
            , true
            , true
            );

        /*
        make the header of our two column space
        */
        audioList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(CalculateColumn(rect, 1, 15, 0), "Action Name");
            EditorGUI.LabelField(CalculateColumn(rect, 2, 15, 0), "Audio Clip");

        };

        /*
        put/show each elements in that table from list
        */
        audioList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            /*we get one element for list*/
            var element = audioList.serializedProperty.GetArrayElementAtIndex(index);

            /*add spacing*/
            rect.y += 2;

            /*and draw that element*/
            EditorGUI.PropertyField(CalculateColumn(rect, 1, 0, 0), element.FindPropertyRelative("action"), GUIContent.none);
            EditorGUI.PropertyField(CalculateColumn(rect, 2, 10, 10), element.FindPropertyRelative("audioClip"), GUIContent.none);

        };

    }


    /*
that's going in inspector view and when we interact with it  
 */
    public override void OnInspectorGUI()
    {
        /*update informaton in object*/
        serializedObject.Update();

        /*
        just draw a line with this sr=tring
        */
        EditorGUILayout.LabelField("Audio clips for actions", EditorStyles.boldLabel);

        /*
        create our list 
        */
        audioList.DoLayoutList();

        /*
        update information in object after we change it
        */
        serializedObject.ApplyModifiedProperties();
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
