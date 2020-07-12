using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class MainMenuUIController :
    UIControllerBase<MainMenuUIController> {

    public void GoToGameplay() {
        LoadingController.LoadScene(1);
    }
}
