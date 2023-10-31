using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameMenuNavigation : MonoBehaviour
{

    public GameObject pauseMenu, settingsMenu;

    public GameObject pauseFirstButton, settingsFirstButton;

    public void PauseOn() {


        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }

    public void OptionOn() {

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(settingsFirstButton);
    }

    public void OptionOff()
    {
        PauseOn();
    }
}
