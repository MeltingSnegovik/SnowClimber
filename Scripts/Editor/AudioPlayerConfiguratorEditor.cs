using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

/*
For Unity Inspector
Change the view of Audio player configurator
(there are links between object A and object B with list of objects A actions   like: bird – bird audio setting)
 */


/*
 Define that AudioPlayerConfigurator will be changed
 */
[CustomEditor(typeof(AudioPlayerConfigurator))]
public class AudioPlayerConfiguratorEditor : Editor
{

    /*
     active instance of our object (for future collecting info)
     */
    private AudioPlayerConfigurator audioPlayerConfigurator;


    /*
     define we have one list in our new brand UI inspector
     */
    private ReorderableList audioSettingsList;

    /*
     On Enable of inspector just draw our list
     */
    private void OnEnable()
    {
        DrawAudioSettingsList();
    }

    /*
     Main function for drawing in UI
     */
    void DrawAudioSettingsList() {

        /*
         define our end list as new reaordable list
        serializedObject - our auidoPlayerConfigurator
        and audioSettingsSet - list, which we want to show
         */
        audioSettingsList = new ReorderableList(
            serializedObject
            , serializedObject.FindProperty("audioSettingsSet")
            , true
            , true
            , true
            , true
            );

    /*
     make the header of our two column space
     */
        audioSettingsList.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(CalculateColumn(rect, 1, 15, 0), "Carrier Name");
            EditorGUI.LabelField(CalculateColumn(rect, 2, 15, 0), "Audio Player Settings");
        };

    /*
     put/show each elements in that table from list
     */

        audioSettingsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            /*we get one element for list*/
            var element = audioSettingsList.serializedProperty.GetArrayElementAtIndex(index);

            /*add spacing*/
            rect.y += 2;

            /*and draw that element*/
            EditorGUI.PropertyField(CalculateColumn(rect, 1, 0, 0), element.FindPropertyRelative("carrier"), GUIContent.none);
            EditorGUI.PropertyField(CalculateColumn(rect, 2, 10, 10), element.FindPropertyRelative("audioSetting"), GUIContent.none);
        };

    }


    /*
    that's going in inspector view and when we interact with it  
     */
    public override void OnInspectorGUI() {
        /*update informaton from object*/
        serializedObject.Update();

        /*
         just draw a line with this sr=tring
         */
        EditorGUILayout.LabelField("Carriers with audio settings", EditorStyles.boldLabel);
        
        /*
        create our list 
         */
        audioSettingsList.DoLayoutList();

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
