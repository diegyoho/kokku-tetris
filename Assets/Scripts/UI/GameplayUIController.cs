using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class GameplayUIController :
    UIControllerBase<GameplayUIController> {
    
    [Header("Screens")]
    [SerializeField]
    CanvasGroup gameOverScreen = null;

    [Header("HUD")]
    [SerializeField]
    Text score = null;
    [SerializeField]
    Text level = null;
    [SerializeField]
    Text scored;

    public static void UpdateScore() {
        instance.score.text = GameController.saveData
                                .score.ToString("D6");
    }

    public static void UpdateLevel() {
        instance.level.text =
            $"LEVEL {PlayfieldController.instance.currentLevel}";
        instance.level.GetComponent<Animator>().SetTrigger("pulse");
    }

    public static void ShowScored(int scored) {
        instance.scored.text = $"+{scored}";
        instance.scored.GetComponent<Animator>().SetTrigger("show");
    }

    public static void ShowGameOver() {
        instance.StartCoroutine(
            instance.IEShowScreen(instance.gameOverScreen)
        );
    }

    public void BackToMainMenu() {
        GameController.Quit();
    }
}
