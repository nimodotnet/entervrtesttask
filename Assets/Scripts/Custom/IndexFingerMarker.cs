using UnityEngine;

public class IndexFingerMarker : MonoBehaviour
{
    public static IndexFingerMarker Instance;

    private void Awake()
    {
        Instance = this;
    }
}
