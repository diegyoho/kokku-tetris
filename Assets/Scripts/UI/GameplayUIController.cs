using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GameplayUIController :
    UIControllerBase<GameplayUIController> {
    
    [Header("Screens")]
    [SerializeField]
    CanvasGroup gameOverScreen;

    [Header("HUD")]
    [SerializeField]
    Text score;

    public static void UpdateScore() {
        instance.score.text = $"Score: {GameController.data.score}";
    }

    public static void ShowGameOver() {
        instance.StartCoroutine(
            instance.IEShowScreen(instance.gameOverScreen)
        );
    }

    public void BackToMainMenu() {
        LoadingController.LoadScene(0);
    }
}
