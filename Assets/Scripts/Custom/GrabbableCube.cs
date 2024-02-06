using Fusion;
using Fusion.XR.Host.Grabbing;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableCube : NetworkBehaviour
{
    public UnityEvent Transferred => transferred;

    [SerializeField] uint lastPlayerId;

    [SerializeField] UnityEvent transferred;

    public override void Spawned()
    {
        base.Spawned();

        var networkGrabbable = GetComponent<NetworkGrabbable>();

        networkGrabbable.onDidGrab.AddListener(OnGrabbed);
    }

    void OnGrabbed(NetworkGrabber grabber)
    {
        var playerObject = grabber.GetComponentInParent<NetworkObject>();
        var objectId = playerObject.Id;

        if (lastPlayerId == objectId.Raw)
            return;

        lastPlayerId = objectId.Raw;
        transferred?.Invoke();
    }
}
