using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScoreShow : MonoBehaviour
{

    public string scoreName;
    public int scoreCount;
    public int scorePrice;

    public Sprite scoreSprite;

    /*
    ui
    */

    public TextMeshProUGUI uiScoreName;
    public TextMeshProUGUI uiScoreCount;
    public TextMeshProUGUI uiScorePrice;
    public TextMeshProUGUI uiScoreFinal;
    public Image uiScoreImage;

    public void UpdateUI() {
        uiScoreImage.sprite = scoreSprite;
        uiScoreName.text = scoreName;
        uiScoreCount.text = scoreCount.ToString();
        uiScorePrice.text = scorePrice.ToString();

        int fal = scoreCount * scorePrice;
        uiScoreFinal.text = fal.ToString();
    }
}
