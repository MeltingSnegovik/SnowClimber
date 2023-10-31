using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "Some Scene Reference", menuName = "Scriptable Objects/Scene Managment/Scene Reference", order = 1)]
public class SceneReference : ScriptableObject
{
    public string levelPath;
#if UNITY_EDITOR
    public SceneAsset levelScene;
#endif 

    Action LoadLevelAction;

    public void UpdateActions() {
        LoadLevelAction = () => SceneManager.LoadSceneAsync(levelPath, LoadSceneMode.Single);
    }

    public void ReloadLevel(FadeScreen screenFader)
    {
        if (LoadLevelAction == null)
            UpdateActions();

        screenFader.FadeOut(LoadLevelAction);
    }

}
