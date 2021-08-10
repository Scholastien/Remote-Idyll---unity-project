using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneControllerManager : SingletonMonobehaviour<SceneControllerManager>
{
    private bool _isFading = false;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    public SceneName startingSceneName;

    private IEnumerator Start()
    {
        faderCanvasGroup.transform.SetAsLastSibling();

        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        EventHandler.CallAfterSceneUnloadEvent();

        StartCoroutine(Fade(0f));
    }

    // How we publicly access the Manager and ask for a scene load
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (!_isFading)
        {
            StartCoroutine(FadeAndSwitchScene(sceneName, spawnPosition));
        }
    }

    private IEnumerator FadeAndSwitchScene(string sceneName, Vector3 spawnPosition)
    {
        // Call before scene unload fade out event
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();

        // start fading to black and wait for it to finish before continuing
        yield return StartCoroutine(Fade(1f));

        // set player position
        Player.Instance.transform.position = spawnPosition;

        // call before scene unload event
        EventHandler.CallBeforeSceneUnloadEvent();

        // unload the current active scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // start loading the given scene and wait for it to finish (set it to active)
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        // call after scene load
        EventHandler.CallAfterSceneUnloadEvent();

        // start fading back in and wait for it to finish before exiting the function
        yield return StartCoroutine(Fade(0f));

        // call after scene load fade in event
        EventHandler.CallAfterSceneUnloadFadeInEvent();
    }

    // load as an additive scene and set as active
    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        // set the fading flag to true so the fadeandswitch coroutine won't be called again
        _isFading = true;

        // block raycasts into the scene so no more mouse input can be accepted
        faderCanvasGroup.blocksRaycasts = true;

        // calculate how fast the CanvasGroup should fade based on it's current alpha, it's final alpha and how long it had to change between the two.
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - targetAlpha) / fadeDuration; // whether or not (faderCanvasGroup.alpha - targetAlpha) is negative or positive, we want to know the distance between those, so it must be a positive value.

        // while the CanvasGroup hasn't reached the target alpha yet...
        while (!Mathf.Approximately(faderCanvasGroup.alpha, targetAlpha))
        {
            // ... move the alpha towards it's target
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

            yield return null;
        }

        //set flag to false since the fade has finished
        _isFading = false;

        //Stop canvasgroup from blocking raycast so mouse input are no longer ignored
        faderCanvasGroup.blocksRaycasts = false;
    }

    [ButtonGroup]
    [Button("Load First Scene")]
    public void LoadFirstScene()
    {
        SceneControllerManager.Instance.FadeAndLoadScene(startingSceneName.ToString(), new Vector3());
    }

}
