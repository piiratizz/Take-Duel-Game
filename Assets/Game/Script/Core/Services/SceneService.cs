using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneService
{
    public UniTask LoadBootAsync()
    {
        return LoadSceneAsync(Scenes.Boot);
    }
    
    public UniTask LoadMainMenuAsync()
    {
        return LoadSceneAsync(Scenes.MainMenu);
    }

    public UniTask LoadGameplayAsync()
    {
        return LoadSceneAsync(Scenes.Gameplay);
    }
    
    public async UniTask LoadSceneAsync(string sceneName)
    {
        #if UNITY_EDITOR
        if (__changedEditorMode)
        {
            return;
        }
        #endif
        
        await SceneManager.LoadSceneAsync(Scenes.Boot);
        await SceneManager.LoadSceneAsync(sceneName);
    }

#if UNITY_EDITOR
    private static bool __changedEditorMode;
    [InitializeOnLoadMethod]
    private static void Initialize()
    {
        EditorApplication.playModeStateChanged += (state) =>
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                __changedEditorMode = true;
            }
        };
    }
#endif
}