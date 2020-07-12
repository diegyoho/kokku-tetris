using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

[RequireComponent(typeof(LoadingUIController))]
public class LoadingController :
    SingletonMonoBehaviour<LoadingController> {

    LoadingUIController uiController;
    
    public static bool isLoading = false;

    public override void Awake() {
        if(LoadingController.instance)
            Destroy(gameObject);

        base.Awake();
        DontDestroyOnLoad(gameObject);

        uiController = GetComponent<LoadingUIController>();
    }

    public static void LoadScene(int sceneIndex) {
        instance.StartCoroutine(
            instance.IELoadScene(sceneIndex)
        );
    }

    public void LoadSceneNoStatic(int sceneIndex) {
        LoadScene(sceneIndex);
    }

    IEnumerator IELoadScene(int sceneIndex) {
        
        isLoading = true;
        
        uiController.UpdateLoadingBar(0);
        yield return StartCoroutine(uiController.Show());
        

        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(sceneIndex);

        while(!loadingScene.isDone) {
            float progress = Mathf.Clamp01(loadingScene.progress / .9f);

            uiController.UpdateLoadingBar(progress);

            yield return null;
        }

        if(GetComponent<Canvas>())
            GetComponent<Canvas>().worldCamera = Camera.main;
        
        isLoading = false;

        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(uiController.Hide());

    }
}
