using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

using System;


public class GameManager : Soliton<GameManager>
{
    public enum GameState
    {
        Gameplay
        , MainMenu
        , PauseMenu
        , FadeInOut
    }

    public enum SceneType { 
        MainMenu
        ,   Level
    }

    [Header("Game state")]
    public GameState _gameState;
    public SceneType _sceneType;

    [Header("Screen Fade and Level Object")]
    public MenuManager menuManager;
    public LevelControlConfigurator levelControlConfigurator;
    public ScoreInfoConfigurator scoreInfoConfigurator;
    public FadeScreen fadeScreen;

    [Header("Gravity Modifier")]
    public float gravityModifier;
    public Vector3 gravMod;

    [Header("Settings")]
    public float musicVolume;
    public float soundVolume;
    public float brightness;

    [Header("Audio Control (play music and sfx)")]
    public AudioSource musicAudioSource;
    public AudioSource soundAudioSource;
    public AudioControl musicAudioControl;
    public AudioControl soundAudioControl;

    [Header("Path for saves")]
    public string pathSaveSetting;
    public string pathSaveLevelData;

    [Header("Level data (fill itself from levelController)")]
    [SerializeField]
    private LevelControlConfigurator _levelControl;

    [System.Serializable]
    public struct LevelData {
        public int levelInd;
        public string name;
        public int levelsNeeded;
        public bool isCompleted;
        public float timeCompleted;
        public int highScore;

        public LevelData(int m_levelInd, string m_name, int m_levelNeeded, bool m_isCompleted, float m_timeCompleted, int m_highScore) {
            levelInd = m_levelInd;
            name = m_name;
            isCompleted = m_isCompleted;
            levelsNeeded = m_levelNeeded;
            timeCompleted = m_timeCompleted;
            highScore = m_highScore;

        }
        public LevelData(int m_levelInd, string m_name, int m_levelNeeded, bool m_isCompleted)
        {
            levelInd = m_levelInd;
            name = m_name;
            levelsNeeded = m_levelNeeded;
            isCompleted = m_isCompleted;
            timeCompleted = 0;
            highScore = 0;

        }
        public LevelData(int m_levelInd, string m_name, int m_levelNeeded)
        {
            levelInd = m_levelInd;
            name = m_name;
            levelsNeeded = m_levelNeeded;
            isCompleted = false;
            timeCompleted = 0;
            highScore = 0;

        }
    }

    public List<LevelData> levelDataList = new List<LevelData>();

    [Header("Contorller data")]
    public PlayerControl playerControl;


    /*
     score dict 
     */
    private Dictionary<string, int> scoreCounter = new Dictionary<string, int>();
    
    public int lastLevelInd;  // td

    [Header("Gameplay Data")]
    public GameObject crntCamera;
    public GameObject crntFall;
    public GameObject crntSky;
    

    public float cameraMoveSpeed = 1f;


    public int crntFloor;
    public float floorHeigh = 8.0f;

    Action LoadLevelAction;


    public event Action<float> MusicVolumeChanged;
    public event Action<float> SoundVolumeChanged;
    public event Action GamePaused;
    public event Action GameUnpaused;

    void Start() {

        Time.timeScale = 1;
        crntFloor = 1;


        _gameState = GameState.MainMenu;

        gravMod = new Vector3(0, gravityModifier, 0);
        Physics.gravity = gravMod;

        pathSaveSetting = Application.persistentDataPath + "/settings.json";
        pathSaveLevelData = Application.persistentDataPath + "/levelsinfo.json";

        LoadLevelAction = () => ActionsOnLoadLevel();

        fadeScreen.FadeIn();
        ConstructLevelList();
        LoadLevelData();

        FindPlayer();
        FindGameMenuManager();
        FindMainObjects();

        LoadSettings();
        InitMusic();
        SceneManager.sceneLoaded += FindPlayer;
        SceneManager.sceneLoaded += FindGameMenuManager;
        SceneManager.sceneLoaded += FindMainObjects;
        SceneManager.sceneLoaded += ShowBinding; // HIDE
        SceneManager.sceneLoaded += ActionsOnFadeIn;
    }

    /*
     settings 
     */

    [System.Serializable]
    class SettingsData
    {
        public float musicVolume;
        public float soundVolume;
        public float brightness;
        public int lastLevelInd;
    }

    public void SaveSettings()
    {
        SettingsData data = new SettingsData();

        data.musicVolume = musicVolume;
        data.soundVolume = soundVolume;
        data.brightness = brightness;
        data.lastLevelInd = lastLevelInd;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(pathSaveSetting, json);
    }

    public void LoadSettings()
    {
        if (File.Exists(pathSaveSetting))
        {
            string json = File.ReadAllText(pathSaveSetting);
            SettingsData data = JsonUtility.FromJson<SettingsData>(json);

            musicVolume = data.musicVolume;
            soundVolume = data.soundVolume;
            brightness = data.brightness;
            lastLevelInd = data.lastLevelInd;

        }
        else
        {
            musicVolume = 0.5f;
            soundVolume = 0.5f;
            brightness = 0.5f;
        }
        MusicVolumeChanged(musicVolume);
        SoundVolumeChanged(soundVolume);
    }

    public void ChangeSettingsValue(float newMusicVolume, float newSoundVolume, float newBrightness)
    {
        musicVolume = newMusicVolume;
        soundVolume = newSoundVolume;
        brightness = newBrightness;
    }

    public float GiveMusicVolume()
    {
        return musicVolume;
    }

    public float GiveSoundVolume()
    {
        return soundVolume;
    }

    public float GiveBrightness()
    {
        return brightness;
    }

    public AudioSource GiveSoundAudioSource() {
        return soundAudioSource;
    }

    public AudioSource GiveMusicAudioSource()
    {
        return musicAudioSource;
    }

    public void ChangeMusicVolume(float newMusicVolume)
    {
        musicVolume = newMusicVolume;
        musicAudioSource.volume = newMusicVolume;
        MusicVolumeChanged(newMusicVolume);
    }

    public void ChangeSoundVolume(float newSoundVolume) {
        soundVolume = newSoundVolume;
        soundAudioSource.volume = newSoundVolume;
        SoundVolumeChanged(newSoundVolume);
    }

    public void InitMusic()
    {
        musicAudioControl.PlayAudioClip("Music", "MainMenu");
    }

    public int GiveLastLevelInd() {
        return lastLevelInd;
    }

    /*
     settings
     end 
     */


    /*
     level data
     */

    public void ConstructLevelList() {

        for (int i = 0; i < _levelControl.GetLevelCount(); i++) {
            levelDataList.Add(new LevelData(
                i
                ,_levelControl.GetLevelName(i)
                ,0
                , _levelControl.GetLevelIsCompleted(i)
                )) ;
        }

    }
    public void SaveLevelData()
    {

        string saveDataToFile = "{";
        LevelData[] data = new LevelData[levelDataList.Count];

        for (int i = 0; i < levelDataList.Count; i++)
        {

            data[i] = levelDataList[i];
            saveDataToFile += "\n\t" + JsonUtility.ToJson(levelDataList[i]) + ";";

        }
        saveDataToFile += "\n}";

        File.WriteAllText(pathSaveLevelData, saveDataToFile);
    }

    public void LoadLevelData()
    {
        char[] charToTrim = {' ', '\n', '\t', ';'};
        if (File.Exists(pathSaveLevelData))
        {
            string json = File.ReadAllText(pathSaveLevelData);
            json = json.Substring(1, json.Length - 2).Trim(charToTrim);
            string[] stringData = json.Split(';');


//            Debug.Log(json);

            for (int i = 0; i < stringData.Length; i++)
            {
//                Debug.Log(stringData[i].Trim(charToTrim));
                LevelData data = JsonUtility.FromJson<LevelData>(stringData[i].Trim(charToTrim));

                for (int j = 0; j < levelDataList.Count; j++)
                    if (data.levelInd == levelDataList[j].levelInd)
                    {
                        LevelData crntLevel = new LevelData(
                            levelDataList[j].levelInd
                            , levelDataList[j].name
                            , levelDataList[j].levelsNeeded
                            , data.isCompleted
                            , data.timeCompleted
                            , data.highScore
                            );
                        levelDataList[j] = crntLevel;
                        break;
                    }
            }
        }
    }

    public void ChangeLevelData(int crntLevelInd, float crntTime, int crntScore)
    {
        if (levelDataList[crntLevelInd].isCompleted == false || crntTime < levelDataList[crntLevelInd].timeCompleted || crntScore >= levelDataList[crntLevelInd].highScore)
        {
            LevelData crntLevel = new LevelData(
                            levelDataList[crntLevelInd].levelInd
                            , levelDataList[crntLevelInd].name
                            , levelDataList[crntLevelInd].levelsNeeded
                            , true
                            , crntTime
                            , crntScore
                            );
            levelDataList[crntLevelInd] = crntLevel;
        }
        SaveLevelData();
    }

    public string NameOfLevel(int crntLevelInd)
    {
        return levelDataList[crntLevelInd].name;
    }

    public bool IsLevelCompleted(int crntLevelInd)
    {
        return levelDataList[crntLevelInd].isCompleted;
    }

    public float TimeOfLevel(int crntLevelInd)
    {
        return levelDataList[crntLevelInd].timeCompleted;
    }

    public int ScoreOfLevel(int crntLevelInd)
    {
        return levelDataList[crntLevelInd].highScore;
    }

    public int LevelsNeeded(int crntLevelInd)
    {
        return levelDataList[crntLevelInd].levelsNeeded;
    }

    public int levelsCount()
    {
        return levelDataList.Count;
    }

    /*
    level data 
    end 
    */

    /*
     scene managment
     */


    void ActionsOnFadeIn(Scene scene, LoadSceneMode mode) {
        ResetScore();
        crntFloor = 1;
        TimeManager.Pause(true);
        TimeManager.ResetTime();
        fadeScreen.FadeIn(LoadLevelAction);
        switch (_sceneType) {
            case SceneType.MainMenu: 
                musicAudioControl.PlayAudioClip("Music", "MainMenu");
                break;
            case SceneType.Level:
                musicAudioControl.PlayAudioClip("Music", "Gameplay");
                break;
        }
    }

    public void ActionsOnFadeIn() {
        ResetScore();
        crntFloor = 1;
        TimeManager.Pause(true);
        TimeManager.ResetTime();
        fadeScreen.FadeIn(LoadLevelAction);
        switch (_sceneType)
        {
            case SceneType.MainMenu:
                musicAudioControl.PlayAudioClip("Music", "MainMenu");
                break;
            case SceneType.Level:
                musicAudioControl.PlayAudioClip("Music", "Gameplay");
                break;
        }
    }


    public void ActionsOnLoadLevel()
    {
        ChangeGameState(GameState.Gameplay);
        TimeManager.Pause(false);
    }

    public void LoadLevel(int levelInd)
    {
        SaveSettings();
        ChangeGameState(GameState.FadeInOut);
        ChangeSceneType(SceneType.Level);
        lastLevelInd = levelInd;
        levelControlConfigurator.LoadLevelByIndex(levelInd, fadeScreen);
    }

    public void RestartLevel()
    {
        ChangeGameState(GameState.FadeInOut);
        ChangeSceneType(SceneType.Level);
        levelControlConfigurator.LoadScenePath(SceneManager.GetActiveScene().path, fadeScreen);
    }

    public void MainMenuLoad()
    {
        ChangeGameState(GameState.FadeInOut);
        ChangeSceneType(SceneType.MainMenu);
        levelControlConfigurator.LoadSceneName("Main Menu", fadeScreen);
        fadeScreen.FadeOut();
        ChangeGameState(GameState.MainMenu);
    }

 
    public void NextLevelLoad()
    {
        if (lastLevelInd < levelDataList.Count)
        {
            lastLevelInd++;
            LoadLevel(lastLevelInd);
        }
        else {
            MainMenuLoad();
        }
    }

    /*
     scene managment
     end
     */


    /*
     gameplay managment
     */
    void FindPlayer(Scene scene, LoadSceneMode mode)
    {
        GameObject crnt_player = GameObject.Find("Player");
        if (crnt_player != null)
        {
            playerControl = crnt_player.GetComponent<PlayerControl>();
        }
        else
        {
            Debug.Log("There is not player");
        }
    }
    void FindPlayer()
    {
        GameObject crnt_player = GameObject.Find("Player");
        if (crnt_player != null)
        {
            playerControl = crnt_player.GetComponent<PlayerControl>();
        }
        else
        {
            Debug.Log("There is not player");
        }
    }

    void FindGameMenuManager(Scene scene, LoadSceneMode mode)
    {
        GameObject obj_MenuManager = GameObject.Find("MenuManager");
        if (obj_MenuManager != null)
        {
            menuManager = obj_MenuManager.GetComponent<MenuManager>();
        }
        else
        {
            Debug.Log("There is not menu manager");
        }
    }

    void FindGameMenuManager()
    {
        GameObject obj_MenuManager = GameObject.Find("MenuManager");
        if (obj_MenuManager != null)
        {
            menuManager = obj_MenuManager.GetComponent<MenuManager>();
        }
        else
        {
            Debug.Log("There is not menu manager");
        }
    }

    void FindMainObjects(Scene scene, LoadSceneMode mode) {

        crntCamera = GameObject.Find("Main Camera");
        crntFall = GameObject.Find("Fall");
        crntSky = GameObject.Find("Sky");
        fadeScreen = GameObject.Find("FadeScreen").GetComponent<FadeScreen>();
    }

    void FindMainObjects()
    {
        crntCamera = GameObject.Find("Main Camera");
        crntFall = GameObject.Find("Fall");
        crntSky = GameObject.Find("Sky");
        fadeScreen = GameObject.Find("FadeScreen").GetComponent<FadeScreen>();
    }

    void ShowBinding() {
        InputManager.GetCurrentBinding("Attack", 0);
        InputManager.GetCurrentBinding("Jump", 0);
    }

    void ShowBinding(Scene scene, LoadSceneMode mode) {
        InputManager.GetCurrentBinding("Attack", 0);
        InputManager.GetCurrentBinding("Jump", 0);
    }

    public void GameOver()
    {
        if (IsGameplay())
        {
            ChangeGameState(GameState.PauseMenu);
            menuManager.GameOverUIOn(true);
            GamePaused();
            soundAudioControl.PlayAudioOneTime("GameManager", "GameOver");
            musicAudioControl.StopPlayAudio();
        }
    }

    public void ChangePaused()
    {
        if (_gameState == GameState.Gameplay)
        {
            ChangeGameState(GameState.PauseMenu);
            GamePaused();
            musicAudioControl.PausePlayAudio();
        }
        else if (_gameState == GameState.PauseMenu)
        {
            ChangeGameState(GameState.Gameplay);
            GameUnpaused();
            musicAudioControl.UnpausePlayAudio();
        }
        menuManager.PauseChangedUI(IsPaused());
    }

    public void CompleteLevel()
    {
        ChangeGameState(GameState.PauseMenu);
        menuManager.CompleteUIOn(true);
        GamePaused();
        soundAudioControl.PlayAudioOneTime("GameManager", "Congratulation");
        ChangeLevelData(lastLevelInd, TimeManager.GetTime(), FinalScore());
    }

    public PlayerControl GetPlayerControl()
    {
        return playerControl;
    }


    public void AddScore(string scoreText)
    {
        int val = 1;
        if (scoreCounter.TryGetValue(scoreText, out val))
        {
            scoreCounter[scoreText] = val + 1;
        }
        else {
            scoreCounter.Add(scoreText, 1);
        }
    }


    public Dictionary<string, int> GetScoreDictionary() {
        return scoreCounter;
    }

    public void ResetScore()
    {
        scoreCounter.Clear();
    }

    public int FinalScore() {
        int finalscore = 0;
        foreach (KeyValuePair<string, int> kvp in scoreCounter) {
            finalscore+= kvp.Value * scoreInfoConfigurator.GetScorePrice(kvp.Key);
        }
        return finalscore;
    }



    /*
     wrong door
    next floor
     */

    public int GetCurrentFloor() {
        return crntFloor;
    }

    public float GetFloorHeigh() {
        return floorHeigh;
    }


    public void CheckFloor() {
        crntCamera.GetComponent<SmoothUpMovement>().StartSmoothUpMovement(crntFloor, floorHeigh, cameraMoveSpeed);
        crntFall.GetComponent<SmoothUpMovement>().StartSmoothUpMovement(crntFloor, floorHeigh, cameraMoveSpeed);
        crntSky.GetComponent<SmoothUpMovement>().StartSmoothUpMovement(crntFloor, floorHeigh, cameraMoveSpeed);
    }

    public void NextFloor() {
        if (IsGameplay())
        {
            crntFloor++;
            CheckFloor();

//            StartCoroutine(SmothCameraMovement(crntCamera));
//            StartCoroutine(SmothCameraMovement(crntFall));
//            StartCoroutine(SmothCameraMovement(crntSky));
        }
    }


    IEnumerator SmothCameraMovement(GameObject obj) {
        float crntHigh = obj.transform.position.y;
        float targetHigh = obj.transform.position.y + floorHeigh;
        Vector3 crntCameraPos = obj.transform.position;
        while (!Mathf.Approximately(crntHigh, targetHigh))
        {
            if(!IsGameplay())
                yield return null;
            float locSpeed = Mathf.MoveTowards(crntHigh, targetHigh, cameraMoveSpeed * Time.deltaTime);
            obj.transform.Translate(Vector3.up * (locSpeed- crntHigh), Space.World);
            crntHigh = obj.transform.position.y;

            yield return null;
        }

    }

    /*
    end floor
    */

    /*
    gameplay managment
    end
    */


    public void ChangeGameState(GameState newGameState) {
        if (_gameState == newGameState)
            return;
        switch (newGameState)
        {
            case GameState.Gameplay:
                
                _gameState = newGameState;
                Time.timeScale = 1;
                playerControl.EnableGameplayControls();
                TimeManager.Pause(false);
                break;
            case GameState.PauseMenu:
                _gameState = newGameState;
                Time.timeScale = 0;
                playerControl.EnablePauseMenuControls();
                TimeManager.Pause(true);
                break;
            case GameState.MainMenu:
                _gameState = newGameState;
                Time.timeScale = 1;
                playerControl.EnablePauseMenuControls();
                break;
            case GameState.FadeInOut:
                _gameState = newGameState;
                Time.timeScale = 0.75f;
                break;

        }

    }

    public void ChangeSceneType(SceneType newSceneType)
    {
        if (_sceneType == newSceneType)
            return;
        switch (newSceneType)
        {
            case SceneType.MainMenu:
                _sceneType = newSceneType;
                break;
            case SceneType.Level:
                _sceneType = newSceneType;
                break;

        }

    }

    public static bool IsPaused() {
        if (Instance._gameState == GameState.PauseMenu)
            return true;
        return false;
    }
    public static bool IsGameplay()
    {
        if (Instance._gameState == GameState.Gameplay)
            return true;
        return false;
    }


    public static bool IsFadeInOut() 
    {
        if (Instance._gameState == GameState.FadeInOut)
            return true;
        return false;
    }

    public GameObject GetCurrentCamera() {
        return crntCamera;
    }

}



