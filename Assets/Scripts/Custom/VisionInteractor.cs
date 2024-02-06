using Fusion;
using UnityEngine;

public class VisionInteractor : MonoBehaviour
{
    [SerializeField] float viewAreaRadius;

    [SerializeField] LayerMask interactionLayer;
    //public override void Spawned()
    //{
    //    base.Spawned();

    //    if (!HasInputAuthority)
    //        enabled = false;
    //}

    private void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, viewAreaRadius, transform.forward, out hit, 10f, interactionLayer))
        {
            IViewInteractable interactable;

            if (hit.collider.TryGetComponent(out interactable))
            {
                interactable.Interact(gameObject);
            }
        }
    }
}
