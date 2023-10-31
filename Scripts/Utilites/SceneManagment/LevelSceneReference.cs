using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level X Scene Reference", menuName = "Scriptable Objects/Scene Managment/Level Scene Reference", order = 1)]
public class LevelSceneReference : SceneReference
{
    public string levelName;
    public int levelNumber;
    public bool levelCompleted;
    public float timeComleted;
    public int highScore;

    public string GetLevelName() {
        return levelName;
    }

    public int GetLevelNumber()
    {
        return levelNumber;
    }

    public bool GetLevelCompleted() {
        return levelCompleted;
    }

    public float GetTimeCompleted() {
        return timeComleted;
    }

    public int GetHishScore() {
        return highScore;
    }


    public void UpdateLevelInfo(bool newLevelCompleted, float newTimeCompleted, int newHighScore)
    {
        
    }

}
