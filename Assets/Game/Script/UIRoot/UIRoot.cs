using UnityEngine;

public class UIRoot : MonoBehaviour
{
    [SerializeField] private Transform _uiHolder;

    public void AttachUI(GameObject element)
    {
        element.transform.SetParent(_uiHolder);
    }
    
    public void DetachUI(GameObject element)
    {
        element.transform.SetParent(null);
    }
}