using UnityEngine;

public abstract class WindowBase : MonoBehaviour
{
    [SerializeField] private GameObject _content;

    public virtual void Open()
    {
        _content.SetActive(true);    
    }

    public virtual void Close()
    {
        _content.SetActive(false); 
    }
}