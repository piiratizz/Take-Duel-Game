using UnityEngine;

public class PlayerModelChanger : MonoBehaviour
{
    [SerializeField] private GameObject _globalModel;
    [SerializeField] private GameObject _localModel;

    public void SetGlobalModel()
    {
        _localModel.SetActive(false);
        _globalModel.SetActive(true);
    }
    
    public void SetLocalModel()
    {
        _localModel.SetActive(true);
        _globalModel.SetActive(false);
    }
}
