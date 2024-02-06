using TMPro;
using UnityEngine;

public class TransferCounter : MonoBehaviour
{
    [SerializeField] TextMeshPro scoreVisual;

    int score = 0;

    private void Awake()
    {
        scoreVisual = GetComponentInChildren<TextMeshPro>();
    }

    public void AddScore()
    {
        score++;

        scoreVisual.SetText(score.ToString());
    }
}
