using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class GameController :
    SingletonMonoBehaviour<GameController> {
    
    public int[] scorePerRows;

    public GameData gameData;
    public static SaveData saveData;
    public static int startLevel = 1;

    void Start() {
        saveData = LoadData();
        SoundController.PlayMusic(GameData.GetAudioClip("BGM"), .5f);
        MainMenuUIController.UpdateHUD();
    }

    public static void Play() {
        instance.StartCoroutine(instance.IEPlay());
    }

    IEnumerator IEPlay() {
        LoadingController.LoadScene(1);
        yield return new WaitUntil(() => !LoadingController.isLoading);
        PlayfieldController.Setup(startLevel);
        PlayfieldController.StartSpawn();
    }

    public static void Quit() {
        instance.StartCoroutine(instance.IEQuit());
    }

    IEnumerator IEQuit() {
        LoadingController.LoadScene(0);
        SaveData();
        yield return new WaitUntil(() => !LoadingController.isLoading);
        saveData = LoadData();
        MainMenuUIController.UpdateHUD();
    }

    public static void Score(int rowsCleared) {
        rowsCleared = (int) Mathf.Clamp(
            rowsCleared - 1, 0, instance.scorePerRows.Length - 1
        );

        int scored = instance.scorePerRows[rowsCleared] *
                            PlayfieldController.instance.currentLevel;

        saveData.score += scored;

        GameplayUIController.ShowScored(scored);
        
        if(saveData.score > saveData.highscore)
            saveData.highscore = saveData.score;
    }

    public static void SaveData() {
        PlayerPrefs.SetString("tetris-save", JsonUtility.ToJson(saveData));
    }

    public static SaveData LoadData() {
        SaveData saveData = JsonUtility.FromJson<SaveData>(
            PlayerPrefs.GetString("tetris-save", "")
        );

        return saveData == null ? new SaveData() : saveData;
    }
}
