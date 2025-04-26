using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] private List<NetworkStartPosition> _startPoints;

    public IReadOnlyList<NetworkStartPosition> StartPosition => _startPoints;
}