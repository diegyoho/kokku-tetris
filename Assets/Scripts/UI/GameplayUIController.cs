using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using TMPro;

public class GameplayUIController :
    UIControllerBase<GameplayUIController> {
    
    [Header("Screens")]
    [SerializeField]
    CanvasGroup gameOverScreen;

    public static void ShowGameOver() {
        instance.StartCoroutine(
            instance.IEShowScreen(instance.gameOverScreen)
        );
    }

    public void BackToMainMenu() {
        LoadingController.LoadScene(0);
    }
}
