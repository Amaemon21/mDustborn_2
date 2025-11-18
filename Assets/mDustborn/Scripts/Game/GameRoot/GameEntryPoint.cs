using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameEntryPoint : IInitializable
{
    private Coroutines _coroutines;
    private UIRootView _uiRoot;

    private GameEntryPoint(Coroutines coroutines, UIRootView uiRoot)
    {
        _coroutines = coroutines;
        _uiRoot = uiRoot;
    }
    
    public void Initialize()
    {
        RunGame();
    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == Scenes.GAMEPLAY)
        {
            _coroutines.StartCoroutine(LoadAndStartGameplay());
            return;
        }

        if (sceneName == Scenes.MAINMENU)
        {
            _coroutines.StartCoroutine(LoadAndStartMainMenu());
            return;
        }

        if (sceneName != Scenes.BOOT)
        {
            return;
        }
#endif

        _coroutines.StartCoroutine(LoadAndStartMainMenu());
    }

    private IEnumerator LoadAndStartGameplay()
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAMEPLAY);

        yield return new WaitForSeconds(1);

        
        
        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadAndStartMainMenu()
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.MAINMENU);

        yield return new WaitForSeconds(1);
        
        
        
        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}