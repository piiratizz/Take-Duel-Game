using UnityEngine;
using Zenject;

public class LoadingScreenService
{
    private GameObject _loadingScreen;
    
    public void Initialize()
    {
        var prefab = Resources.Load("LoadingScreen");
        _loadingScreen = Object.Instantiate(prefab) as GameObject;
        Object.DontDestroyOnLoad(_loadingScreen);
    }
    
    public void ShowLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }
    
    public void HideLoadingScreen()
    {
        _loadingScreen.SetActive(false);
    }
}