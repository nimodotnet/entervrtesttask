using System.Threading.Tasks;
using UnityEngine;

public class SimpleEffectReaction : Reaction
{
    [SerializeField] int delayInSeconds;
    public override async void Simulate(GameObject targetShape)
    {
        await Task.Delay(delayInSeconds * 1000);

        Instantiate(effect, targetShape.transform.position, Quaternion.identity);
    }
}
