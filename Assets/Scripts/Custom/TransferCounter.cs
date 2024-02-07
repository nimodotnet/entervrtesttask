using TMPro;
using UnityEngine;

public class TransferCounter : MonoBehaviour
{
    [SerializeField] TextMeshPro scoreVisual;

    public int Score { get; set; }

    private void Awake()
    {
        scoreVisual = GetComponentInChildren<TextMeshPro>();
    }

    public void AddScore()
    {
        Score++;

        scoreVisual.SetText(Score.ToString());
    }

    public void ResetScore() => Score = 0;
}
