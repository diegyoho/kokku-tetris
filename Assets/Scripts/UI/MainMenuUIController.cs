using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using TMPro;

public class MainMenuUIController :
    UIControllerBase<MainMenuUIController> {

    public void GoToGameplay() {
        LoadingController.LoadScene(1);
    }
}
