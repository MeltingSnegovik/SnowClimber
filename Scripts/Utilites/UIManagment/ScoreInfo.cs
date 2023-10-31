using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Score Info", menuName = "Scriptable Objects/Score Info", order = 1)]
public class ScoreInfo : ScriptableObject
{
    public Color objectDiscplayColor;

    public int objectScore;

    public Sprite objectSprite;
}
