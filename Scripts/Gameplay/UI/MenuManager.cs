using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Slider sliderMusicVolume;
    public Slider sliderSoundVolume;
    public Slider sliderBrightness;

    public TextMeshProUGUI timeText;

    public GameObject pauseScreen;
    public GameObject completeScreen;
    public GameObject gameOverScreen;
    public GameObject settingsScreen;

    public void Start()
    {
        InitSettings();

        sliderMusicVolume.onValueChanged.AddListener(ChangeMusicVolume);
        sliderSoundVolume.onValueChanged.AddListener(ChangeSoundVolume);
    }

    public void Update()
    {
        UpdateTimeUI();
    }

    public void InitSettings()
    {
        sliderMusicVolume.value = GameManager.Instance.GiveMusicVolume() * 100f;
        sliderSoundVolume.value = GameManager.Instance.GiveSoundVolume() * 100f;
        sliderBrightness.value = GameManager.Instance.GiveBrightness() * 100f;
    }

    public void ChangeMusicVolume(float value)
    {
        GameManager.Instance.ChangeMusicVolume(value/100f);
    }


    public void ChangeSoundVolume(float value)
    {
        GameManager.Instance.ChangeSoundVolume(value / 100f);
    }


    public void UpdateTimeUI() {
        timeText.text = TimeManager.GetTime().ToString("0.00");
    }

    public void GameOverUIOn(bool status) {
        if (status)
        {
            gameOverScreen.SetActive(true);
        }
        else
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void CompleteUIOn(bool status)
    {
        if (status)
        {
            completeScreen.SetActive(true);
        }
        else
        {
            completeScreen.SetActive(true);
        }
    }

    public void PauseChangedUI(bool isPaused) {
        pauseScreen.SetActive(isPaused);
    }

    /*
     Buttons
     */


    public void ClickNextLevelButton()
    {
        GameManager.Instance.NextLevelLoad();
    }

    public void ClickRestartLevelButton()
    {
        GameManager.Instance.RestartLevel();
    }

    public void ClickSettingsButton()
    {
        settingsScreen.SetActive(true);
    }

    public void ClickMainMenuButton()
    {
        GameManager.Instance.MainMenuLoad();
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


    public void ClickContinueButton() {
        GameManager.Instance.ChangePaused();
    }

    /*
     Buttons
     end
     */

    private void OnDestroy()
    {
        sliderMusicVolume.onValueChanged.RemoveListener(ChangeSoundVolume);
        sliderSoundVolume.onValueChanged.RemoveListener(ChangeSoundVolume);
    }
}
