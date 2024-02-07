using Fusion;
using TMPro;

public class TransferCounter : NetworkBehaviour
{
    [Networked] public int Score { get; set; }

    ChangeDetector changeDetector;
    TextMeshPro scoreVisual;

    private void Awake()
    {
        scoreVisual = GetComponentInChildren<TextMeshPro>();
    }

    public override void Spawned()
    {
        base.Spawned();

        changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public void AddScore()
    {
        Score++;
    }

    public override void Render()
    {
        base.Render();

        SyncScore();
    }


    void SyncScore()
    {
        foreach (var change in changeDetector.DetectChanges(this))
        {
            if (change == nameof(Score))
            {
                scoreVisual.SetText(Score.ToString());
                break;
            }
        }
    }
}
