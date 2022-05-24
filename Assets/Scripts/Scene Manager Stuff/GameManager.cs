using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//source: https://www.youtube.com/watch?v=iXWFTgFNRdM
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject loadingScreen;
    public ProgressBar progressBar;

    SceneIndexes currentScene;
    readonly List<AsyncOperation> scenesLoading = new List<AsyncOperation>();
    float totalSceneProgress;

    void Awake()
    {
        instance = this;

        currentScene = SceneIndexes.MENU_SCREEN;
        SceneManager.LoadSceneAsync((int)SceneIndexes.MENU_SCREEN, LoadSceneMode.Additive);
    }

    public void LoadScene(SceneIndexes sceneToLoad)
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)sceneToLoad, LoadSceneMode.Additive));
        currentScene = sceneToLoad;

        StartCoroutine(GetSceneLoadProgress());
    }

    public void ReloadCurrentScene()
    {
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)currentScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)currentScene, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    public IEnumerator GetSceneLoadProgress()
    {
        foreach (AsyncOperation scene in scenesLoading)
        {
            while (!scene.isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation ops in scenesLoading)
                {
                    totalSceneProgress += ops.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                progressBar.ChangeCurrent(Mathf.RoundToInt(totalSceneProgress));

                yield return null;
            }
        }

        loadingScreen.SetActive(false);
    }
}
