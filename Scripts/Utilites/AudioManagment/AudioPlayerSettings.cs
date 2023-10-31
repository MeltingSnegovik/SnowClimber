using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Audio Player Settings", menuName = "Scriptable Objects/Audio Settings", order = 1)]
public class AudioPlayerSettings : ScriptableObject
{
    [System.Serializable]
    public struct AudioSet
    {
        public string action;
        public AudioClip audioClip;
    }

    public List<AudioSet> audioSetsList = new List<AudioSet>();

    public AudioClip GetAudioClip(string action) {
        AudioClip audioClip = null;

        for (int i = 0; i < audioSetsList.Count; i++) {
            if (audioSetsList[i].action == action)
                audioClip = audioSetsList[i].audioClip;
        }

        if (audioClip == null)
            Debug.Log("There is no Audio clip for this action: " + action);
        return audioClip;
    }
}
