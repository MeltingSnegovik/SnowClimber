using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Score Info Configurator", menuName = "Scriptable Objects/Score Info Configurator", order = 1)]
public class ScoreInfoConfigurator : ScriptableObject
{
    [System.Serializable]
    public struct ScoreSet
    {
        public string scoreName;
        public ScoreInfo scoreInfo;
    }

    public Sprite nullSpriteScore;
    public int nullScore;


    public List<ScoreSet> listScoreSet = new List<ScoreSet>();

    public Sprite GetScoreIcon(string o_scoreName)
    {
        Sprite displayIcon = nullSpriteScore;

        for (int i = 0; i < listScoreSet.Count; i++ ) {
            if (listScoreSet[i].scoreName == o_scoreName) {
                displayIcon = listScoreSet[i].scoreInfo.objectSprite;
            }
        }

        return displayIcon;
    }

    public int GetScorePrice(string o_scoreName) {
        int price = nullScore;

        for (int i = 0; i < listScoreSet.Count; i++) { 
            if(listScoreSet[i].scoreName == o_scoreName) {
                price = listScoreSet[i].scoreInfo.objectScore;
            }
        }

        return price;
    }




}
