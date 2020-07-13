using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class LoadingUIController :
    UIControllerBase<LoadingUIController> {
    
    [Header("Screens")]
    [SerializeField]
    CanvasGroup loadingScreen = null;

    [Header("Loading Screen")]
    [SerializeField]
    Slider loadingBar = null;
    [SerializeField]
    Text progressText = null;
    
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
