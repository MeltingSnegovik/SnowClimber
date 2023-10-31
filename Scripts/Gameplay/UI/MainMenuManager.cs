using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    /*
    Settings part
     */
    public GameObject settingsScreen;
    public GameObject levelChooseScreen;
    public GameObject controlSetting;

    public UIRebindingControlPanel rebindingControlPanel;

    public Slider sliderMusicVolume;
    public Slider sliderSoundVolume;
    public Slider sliderBrightness;

    public RectTransform parentRectTransform;
    public LevelIcon levelIconPrefab;

//    public List<LevelInfo> levels = new List<LevelInfo>();
    List<LevelIcon> icons = new List<LevelIcon>();

    public int choosenLevelInd = 0;

    public int lastLevelInd =0;

    public String textForNewGame;
    public TextMeshProUGUI textButtonPlay;
    /*
    
     */

    public void Start() {
        InitSettings();
        CreateGrid();
        sliderMusicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        sliderSoundVolume.onValueChanged.AddListener(ChangeSoundVolume);
    }


    public void Update() {       
    }


    public void ChangeMusicVolume(float value) {
        GameManager.Instance.ChangeMusicVolume(value/100f);
    }


    public void ChangeSoundVolume(float value)
    {
        GameManager.Instance.ChangeSoundVolume(value / 100f);
    }


    public void InitSettings() {
        
        sliderMusicVolume.value = GameManager.Instance.GiveMusicVolume()*100f;
        sliderSoundVolume.value = GameManager.Instance.GiveSoundVolume()*100f;
        sliderBrightness.value = GameManager.Instance.GiveBrightness()*100f;
        lastLevelInd = GameManager.Instance.GiveLastLevelInd();
        if (lastLevelInd == 0)
            textButtonPlay.text = textForNewGame;
        rebindingControlPanel.UpdateRebindActions();
    }


    public void CreateGrid()
    {

        if (GameManager.Instance.levelsCount() <= 0)
            throw new UnityException("There is no level");

        Rect menuSpaceRect = parentRectTransform.rect;
        float menuSpaceWidth = menuSpaceRect.width;
        float menuSpaceHeight = menuSpaceRect.height;

        Vector2 freeSpacePerGridPerSide = new Vector2(0, 10);
//        int iconAreaSizeY = 10;

        for (int i = 0; i < GameManager.Instance.levelsCount(); i++)
        {
            LevelIcon crntLevelIcon = Instantiate(levelIconPrefab, parentRectTransform);
            int _levelInd = GameManager.Instance.levelDataList[i].levelInd;

            crntLevelIcon.m_MainMenu = this;
            crntLevelIcon._levelInd = _levelInd;
            crntLevelIcon._levelName = GameManager.Instance.NameOfLevel(_levelInd);

            crntLevelIcon.UpdateLevelIcon(
                false
                , GameManager.Instance.IsLevelCompleted(_levelInd)
                , GameManager.Instance.TimeOfLevel(_levelInd)
                , GameManager.Instance.ScoreOfLevel(_levelInd)
                );

            icons.Add(crntLevelIcon);
        }
    }

    public void ChoosenLevel(int m_choosenLevel)
    {
        choosenLevelInd = m_choosenLevel;
    }

    /*
    Buttons behaviour
    */

    public void ClickStartButton()
    {
        GameManager.Instance.LoadLevel(0);
    }

    public void ClickSettingsButton()
    {
        settingsScreen.SetActive(true);
    }
    public void ClickChooseLevel()
    {
        levelChooseScreen.SetActive(true);
    }

    public void ClickExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ClickAcceptSettingsButton()
    {
        GameManager.Instance.SaveSettings();
        settingsScreen.SetActive(false);
    }

    public void ClickCancelSettingsButton()
    {
        GameManager.Instance.LoadSettings();
        settingsScreen.SetActive(false);
    }

    public void ClickChoosenLevelStart() {
        GameManager.Instance.LoadLevel(choosenLevelInd);
    }

    public void ClickChoosenLevelBack() {
        levelChooseScreen.SetActive(false);
    }

    public void ClickControlSettings() {
        controlSetting.SetActive(true);
    }


    public void ClickControlSettingsAccept()
    {
        controlSetting.SetActive(false);

        rebindingControlPanel.SaveCurrentRebindActions();
        rebindingControlPanel.UpdateRebindActions();
    }

    public void ClickControlSettingsBack() {
        controlSetting.SetActive(false);

        rebindingControlPanel.LoadLastRebindActions();
        rebindingControlPanel.UpdateRebindActions();
    }

    public void ClickControlSettingsDefault() {
//      controlSetting.SetActive(false);

        rebindingControlPanel.LoadDefaultRebind();
        rebindingControlPanel.UpdateRebindActions();
    }
    //   public void 

    private void OnDestroy()
    {
        sliderMusicVolume.onValueChanged.RemoveListener(ChangeSoundVolume);
        sliderSoundVolume.onValueChanged.RemoveListener(ChangeSoundVolume);
    }
}
