using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRebindingControlPanel : MonoBehaviour
{   
    public UIRebindingControl[] uiRebindingControl;

    public void UpdateRebindActions() {
        for (int i = 0; i < uiRebindingControl.Length; i++) {
            uiRebindingControl[i].UpdateControl();
        }
    }

    public void SaveCurrentRebindActions()
    {
        for (int i = 0; i < uiRebindingControl.Length; i++)
        {
            uiRebindingControl[i].SaveCurrentRebind();
        }
    }

    public void LoadLastRebindActions()
    {
        for (int i = 0; i < uiRebindingControl.Length; i++)
        {
            uiRebindingControl[i].LoadLastRebind();
        }
    }

    public void LoadDefaultRebind() {
        for (int i = 0; i < uiRebindingControl.Length; i++)
        {
            uiRebindingControl[i].LoadDefaultRebind();
        }
    }

}
