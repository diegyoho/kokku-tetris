using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using TMPro;

public class LoadingUIController :
    UIControllerBase<LoadingUIController> {
    
    [Header("Screens")]
    [SerializeField]
    CanvasGroup loadingScreen;

    [Header("Loading Screen")]
    [SerializeField]
    Slider loadingBar;
    [SerializeField]
    TextMeshProUGUI progressText;
    
    void Start() {
        UpdateLoadingBar(0);
    }

    public IEnumerator Show() {
        yield return StartCoroutine(
            IEShowScreen(loadingScreen)
        );
    }

    public IEnumerator Hide() {
        yield return StartCoroutine(
            IEHideScreen(loadingScreen)
        );
    }

    public void UpdateLoadingBar(float value) {
        if(loadingBar)
            loadingBar.value = value;
        if(progressText)
            progressText.text = $"{(100 * value).ToString("F0")}%";
    }
}
