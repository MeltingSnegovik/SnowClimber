using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UIRebindingControl : MonoBehaviour
{
    public PlayerInput m_playerInput;

    [Header("Rebind Settings")]
    public InputActionReference inputActionReference;
    public string actionName;


    [Range(0, 10)]
    [SerializeField]
    public int selectedBinding;


    [Header("Device Display Settings")]
    public DeviceDisplayConfigurator deviceDisplaySettings;

    [Header("UI Display - Action Text")]
    public TextMeshProUGUI actionNameDisplayText;

    [Header("UI Display - Biniding Text for Icon")]
    public TextMeshProUGUI bindingNameDisplayText;
    public Image bindingIconDisplayImage;

    [Header("UI Display - Buttons")]
    public GameObject rebindButtonObject;

    [Header("UI Display - Listening Text")]
    public GameObject listeningForInputObject;


    [Header("Try another one")]
    [SerializeField]
    private InputBinding inputBinding;
    private int bindingIndex;

    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions;

    public void UpdateControl() {
        GetPlayerInput();
        SetupInputAction();
        UpdateActionDisplayUI();
        UpdateBindingDisplayUI();
    }

    void GetPlayerInput() {
    }

    void SetupInputAction() {

        if (inputActionReference != null)
        {
            GetBindingInfo();
            InputManager.LoadBindingOverride(actionName);
        }

    }

    public void ClickRebindButton() {
        StartRebindProcess();
    }

    public void SaveCurrentRebind() {
        InputManager.SaveBindingOverride(actionName);
    }

    public void LoadLastRebind() {
        InputManager.LoadBindingOverride(actionName);
    }

    public void LoadDefaultRebind() {
        InputManager.LoadDefaultBindingOverride(actionName);
    }


    void StartRebindProcess() {

        ToggleGameObjectState(rebindButtonObject, false);
        ToggleGameObjectState(listeningForInputObject, true);

        InputManager.StartRebind(actionName, bindingIndex, true);

        InputManager.rebindComplete += RebindCompleted;
        InputManager.rebindCanceled += RebindCompleted;
    }

    void RebindCompleted()
    {

        ToggleGameObjectState(rebindButtonObject, true);
        ToggleGameObjectState(listeningForInputObject, false);

        UpdateActionDisplayUI();
        UpdateBindingDisplayUI();
    }

    void UpdateActionDisplayUI()
    {
        actionNameDisplayText.SetText(actionName);
    }

    void UpdateBindingDisplayUI()
    {
        
        int controlBindingIndex = inputActionReference.action.GetBindingIndexForControl(inputActionReference.action.controls[0]);
        string pathBind;

        if (Application.isPlaying)
        {
            pathBind = InputManager.GetBindingName(actionName, bindingIndex);
        }
        else {
            pathBind = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        }


        string currentBindingInput = InputControlPath.ToHumanReadableString(
            pathBind, 
            InputControlPath.HumanReadableStringOptions.OmitDevice
            );

        Sprite currentDisplayIcon = null; /* deviceDisplaySettings.GetDeviceBindingIcon(m_playerInput, currentBindingInput); */

        if (currentDisplayIcon)
        {
            ToggleGameObjectState(bindingNameDisplayText.gameObject, false);
            ToggleGameObjectState(bindingIconDisplayImage.gameObject, true);
            bindingIconDisplayImage.sprite = currentDisplayIcon;
        }
        else if (currentDisplayIcon == null)
        {
            ToggleGameObjectState(bindingNameDisplayText.gameObject, true);
            ToggleGameObjectState(bindingIconDisplayImage.gameObject, false);
            bindingNameDisplayText.SetText(currentBindingInput);
        }
       
    }


    void ToggleGameObjectState(GameObject targetGameObject, bool newState)
    {
        targetGameObject.SetActive(newState);
    }


    private void GetBindingInfo()
    {
        if (inputActionReference.action != null)
            actionName = inputActionReference.action.name;

        if (inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

}
