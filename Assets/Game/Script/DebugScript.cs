using NaughtyAttributes;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    [Button("Handle")]
    public void Handle()
    {
        GameObject[] objs = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var obj in objs)
        {
            if (obj.name.Contains("Prop"))
            {
                if(obj.TryGetComponent<MeshCollider>(out var component))
                    DestroyImmediate(component);
            }
        }
    }
}
