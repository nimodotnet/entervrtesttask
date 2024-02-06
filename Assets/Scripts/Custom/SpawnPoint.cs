using Fusion;
using System;
using UnityEngine;

public class SpawnPoint : MonoBehaviour, IComparable<SpawnPoint>
{
    [Networked]
    public bool IsBusy { get; private set; }

    public int Index => index;

    [SerializeField] int index;

    public int CompareTo(SpawnPoint other)
    {
        if (other is null) throw new ArgumentException();
        return Index - other.Index;
    }
}
