using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[System.Serializable]
public class AudioClipInfo {
    public string name;
    public AudioClip clip;
}

[CreateAssetMenu(fileName = "GameData", menuName = "Tetris/GameData", order = 1)]
public class GameData : SingletonScriptableObject<GameData> {
    
    [SerializeField]
    List<AudioClipInfo> audioClipInfos = new List<AudioClipInfo>();
    [SerializeField]
    float[] gravityForLevel;

    public static AudioClip GetAudioClip(string name) {
        AudioClipInfo audioClipInfo = instance.audioClipInfos.Find(
            aci => aci.name == name
        );

        if(audioClipInfo != null)
            return audioClipInfo.clip;

        return null;
    }

    public static float GetGravityForLevel(int level) {
        level = (int) Mathf.Clamp(
            level - 1, 0, instance.gravityForLevel.Length - 1
        );

        return instance.gravityForLevel[level];
    }
}
