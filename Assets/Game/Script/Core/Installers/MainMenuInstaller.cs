using UnityEngine;
using Zenject;

public class MainMenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}