using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class MainMenuUIController :
    UIControllerBase<MainMenuUIController> {
    
    [SerializeField]
    Text startLevel;

    public void GoToGameplay() {
        GameController.Play();
    }

    public void UpdateStartLevel(float value) {
        startLevel.text = $"LEVEL: {value}";
        GameController.startLevel = (int) value;
    }
}
