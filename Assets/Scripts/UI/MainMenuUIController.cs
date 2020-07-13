using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class MainMenuUIController :
    UIControllerBase<MainMenuUIController> {
    
    [SerializeField]
    Text startLevel;
    [SerializeField]
    Slider startLevelSlider;
    
    [SerializeField]
    Text highscore;
    
    [SerializeField]
    Text soundButton;

    public void GoToGameplay() {
        GameController.Play();
    }

    public static void UpdateHighscore() {
        instance.highscore.text = $"HIGHSCORE: {GameController.saveData.highscore}";
    }

    public void UpdateStartLevel(float value) {
        startLevel.text = $"LEVEL: {value}";
        GameController.startLevel = (int) value;
        startLevelSlider.value = value;
    }

    public void ToggleSound() {
        GameController.saveData.isMuted = !GameController.saveData.isMuted;
        GameController.SaveData();
        UpdateSound();
    }

    public static void UpdateSound() {
        instance.soundButton.text = "SOUND: " + (
            !GameController.saveData.isMuted ? "ON" : "OFF"
        );

        SoundController.musicMuted = GameController.saveData.isMuted;
        SoundController.soundMuted = GameController.saveData.isMuted;
    }

    public static void UpdateHUD() {
        UpdateHighscore();
        UpdateSound();
        instance.UpdateStartLevel(GameController.startLevel);
    }
}
