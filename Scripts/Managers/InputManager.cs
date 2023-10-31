using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class InputManager : Soliton<InputManager>
{
    public static SnowClimber inputActions;

    public static event Action rebindComplete;
    public static event Action rebindCanceled;
    public static event Action<InputAction, int> rebindStarted;

    private void Start()
    {
        if (inputActions == null)
        {
            inputActions = new SnowClimber();
        }

    }

    public static void StartRebind(string actionName, int bindingIndex, bool excludeMouse) {
        InputAction action = inputActions.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex) {
            Debug.Log("Couldn't find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite)
        {
            int firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                DoRebind(action, bindingIndex, true, excludeMouse);

        }
        else 
            DoRebind(action, bindingIndex, true, excludeMouse);
    }

    private static void DoRebind(InputAction actionToRebind, int bindingIndex, bool allCompositeParts, bool excludeMouse)
    {
        if (actionToRebind == null || bindingIndex < 0)
            return;

        actionToRebind.Disable();

//        Debug.Log(actionToRebind + " " + bindingIndex + " " + allCompositeParts + " " + excludeMouse);

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                int nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, allCompositeParts, excludeMouse);
            }

            rebindComplete?.Invoke();

        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");
        rebind.WithControlsExcluding("<Mouse>/delta");
        rebind.WithControlsExcluding("<Gamepad>/Start");
        rebind.WithControlsExcluding("<Keyboard>/p");
        rebind.WithControlsExcluding("<Keyboard>/escape");
        rebind.OnMatchWaitForAnother(0.1f);


        if (excludeMouse)
            rebind.WithControlsExcluding("Mouse");

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
    }

    public static string GetBindingName(string actionName, int bindingIndex) {

        if (inputActions == null)
        {
            inputActions = new SnowClimber();

        }

        InputAction action = inputActions.asset.FindAction(actionName);
        return action.bindings[bindingIndex].effectivePath;

    }

    public static void SaveBindingOverride(string actionName) {
        InputAction action = inputActions.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++) {
            PlayerPrefs.SetString("Last" + action.actionMap + action.name + i, action.bindings[i].overridePath);
//          Debug.Log("PlayerPrefab Save: Last" + action.actionMap + action.name + PlayerPrefs.GetString("Last" + action.actionMap + action.name + i));
        }

    }

    public static void LoadDefaultBindingOverride(string actionName)
    {
        if (inputActions == null)
        {
            inputActions = new SnowClimber();
        }


        InputAction action = inputActions.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Default" + action.actionMap + action.name + i)))
            {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString("Default" + action.actionMap + action.name + i));
//               Debug.Log("PlayerPrefab: Default" + action.actionMap + action.name + PlayerPrefs.GetString("Last" + action.actionMap + action.name + i));

            }
        }
    }

    public static void LoadBindingOverride(string actionName) {
        if (inputActions == null)
        {
            inputActions = new SnowClimber();
        }

            InputAction action = inputActions.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++) {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Last" + action.actionMap + action.name + i)))
            {
                action.ApplyBindingOverride(i, PlayerPrefs.GetString("Last" + action.actionMap + action.name + i));
//                Debug.Log("PlayerPrefab: Last ||||||||" + action.actionMap + " |||||||||||" + action.name +"||||||||||||"+ PlayerPrefs.GetString("Last" + action.actionMap + action.name + i) );
            }
            else
                LoadDefaultBindingOverride(actionName);
        }
    }

    public static void ResetBinding(string actionName, int bindingIndex) {
        InputAction action = inputActions.asset.FindAction(actionName);

        if (action == null || action.bindings.Count <= bindingIndex) {
            Debug.Log("Could not find action or binding");
            return;
        }

        if (action.bindings[bindingIndex].isComposite) {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
                action.RemoveBindingOverride(i);

        }
        else
            action.RemoveBindingOverride(bindingIndex);
    }

    public static void GetCurrentBinding(string actionName, int bindingIndex) {
        InputAction action = inputActions.asset.FindAction(actionName);
        
        string pathBind = InputManager.GetBindingName(actionName, bindingIndex);

//        Debug.Log(action.bindings[bindingIndex].effectivePath + " ||||||||| " + InputControlPath.ToHumanReadableString(pathBind, InputControlPath.HumanReadableStringOptions.OmitDevice));
    }
}  
