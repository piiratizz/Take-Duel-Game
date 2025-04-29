using Cysharp.Threading.Tasks;
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
        await SceneManager.LoadSceneAsync(Scenes.Boot);
        await SceneManager.LoadSceneAsync(sceneName);
    }
}