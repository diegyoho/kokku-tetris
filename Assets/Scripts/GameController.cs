using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameController :
    SingletonMonoBehaviour<GameController> {
    
    public int[] scorePerRows;
    public static GameData data;

    void Start() {
        data = new GameData();
    }

    public static void Score(int rowsCleared) {
        rowsCleared = (int) Mathf.Clamp(
            rowsCleared - 1, 0, instance.scorePerRows.Length - 1
        );

        data.score += instance.scorePerRows[rowsCleared];
    }
}
