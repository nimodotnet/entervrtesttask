using Fusion;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] SpawnPoint[] spawnPoints;

    int currentPointIndex;

    [ContextMenu("Find SpawnPoints")]
    public void FindAllSpawnPoints()
    {
        spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
    }

    private void Awake()
    {
        Instance = this;

        Array.Sort(spawnPoints);
    }

    public SpawnPoint GetSpawnPoint()
    {
        return spawnPoints[Runner.ActivePlayers.Count() - 1];
    }
}