using UnityEngine;

public abstract class Reaction : MonoBehaviour
{
    [SerializeField]
    protected GameObject effect;

    public abstract void Simulate(GameObject targetShape);
}
