using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameController :
    SingletonMonoBehaviour<GameController> {
    
    public int[] scorePerRows;

    public GameData gameData;
    public static SaveData saveData;

    void Start() {
        saveData = new SaveData();
        SoundController.PlayMusic(GameData.GetAudioClip("BGM"), .5f);
    }

    public static void Score(int rowsCleared) {
        rowsCleared = (int) Mathf.Clamp(
            rowsCleared - 1, 0, instance.scorePerRows.Length - 1
        );

        saveData.score += instance.scorePerRows[rowsCleared];
    }
}
