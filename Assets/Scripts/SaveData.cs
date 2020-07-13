using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    
    [System.NonSerialized]
    public int score;
    public int highscore;
    public bool isMuted;
}
