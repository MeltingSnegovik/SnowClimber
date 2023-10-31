using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelIcon : MonoBehaviour
{
    public RectTransform parentRectTransform;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textTime;
    public TextMeshProUGUI textScore;

    public GameObject imIsCompleted;
    public GameObject imIsEnabled;
    public GameObject imIsBlocked;

    public string _levelName;
    public int _levelInd;

    public MainMenuManager m_MainMenu;

    public void UpdateLevelIcon(bool isBlocked, bool isCompleted, float time, int score) { 
        if(!isBlocked)
            imIsBlocked.SetActive(false);
        if (isCompleted) {
            imIsCompleted.SetActive(true);
            textTime.text = time.ToString();
            textName.text = _levelName;
            textScore.text = score.ToString();
        }
    }

    public void ChoosenLevel() {
        m_MainMenu.ChoosenLevel(_levelInd);
    }
}
