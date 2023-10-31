using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Audio Player Configurator", menuName = "Scriptable Objects/Audio Configurator", order = 1)]
public class AudioPlayerConfigurator : ScriptableObject
{
    [System.Serializable]
    public struct AudioSettingsSet {
        public string carrier;
        public AudioPlayerSettings audioSetting;
    }

    public List<AudioSettingsSet> audioSettingsSet = new List<AudioSettingsSet>();

    public AudioClip GetAudioClip(string carrier, string action) {
        AudioClip audioClip = null;
        for (int i = 0; i < audioSettingsSet.Count; i++) {
            if (audioSettingsSet[i].carrier == carrier)
                audioClip = audioSettingsSet[i].audioSetting.GetAudioClip(action);
        }

        if (audioClip == null)
            Debug.Log("There is no action for " + carrier);

        return audioClip;
    }
}
