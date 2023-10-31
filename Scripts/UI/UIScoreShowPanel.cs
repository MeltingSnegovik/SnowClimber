using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIScoreShowPanel : MonoBehaviour
{

    public RectTransform scorePanelRect;

    public UIScoreShow uiScoreShowPrefab;

    public ScoreInfoConfigurator scoreInfoConfigurator;

    public Dictionary<string, int> scoreCounter = new Dictionary<string, int>();
    public List<UIScoreShow> uiScoreShow = new List<UIScoreShow>();

    private void OnEnable()
    {
        UpdateScoreDictionary();
        UpdateShowPanel();
    }

    public void UpdateScoreDictionary()
    {
        scoreCounter = GameManager.Instance.GetScoreDictionary();
    }

    public void UpdateShowPanel()
    {
        int finalScore = 0;
        foreach (KeyValuePair<string, int> kvp in scoreCounter)
        {

            UIScoreShow crntUIScoreShow = Instantiate(uiScoreShowPrefab, scorePanelRect);

            crntUIScoreShow.scoreName = kvp.Key;
            crntUIScoreShow.scoreCount = kvp.Value;
            crntUIScoreShow.scorePrice = scoreInfoConfigurator.GetScorePrice(kvp.Key);
            crntUIScoreShow.scoreSprite = scoreInfoConfigurator.GetScoreIcon(kvp.Key);
            crntUIScoreShow.UpdateUI();
            uiScoreShow.Add(crntUIScoreShow);
            
            finalScore += crntUIScoreShow.scoreCount * crntUIScoreShow.scorePrice;


        }

        /*
        add "final score" 
         */
        UIScoreShow finalUIScoreShow = Instantiate(uiScoreShowPrefab, scorePanelRect);
        finalUIScoreShow.scoreName = "Final";
        finalUIScoreShow.scoreCount = 1;
        finalUIScoreShow.scorePrice = finalScore;
        finalUIScoreShow.UpdateUI();

    }
}

