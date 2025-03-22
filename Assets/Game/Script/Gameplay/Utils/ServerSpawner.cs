using Mirror;
using UnityEngine;

public static class ServerSpawner
{
    public static void InstantiateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var instance = Object.Instantiate(prefab, position, rotation);
        NetworkServer.Spawn(instance);
    }
}