using UnityEngine;

public class SubstanceStream : MonoBehaviour
{
    public void CheckForSubstance(RaycastHit hit, string trigger)
    {
        Substance substance = null;

        if (hit.collider.TryGetComponent(out substance))
        {
            substance.Interact(trigger);
        }
    }
}
