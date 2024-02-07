using Fusion;
using Fusion.XR.Host.Grabbing;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableCube : NetworkBehaviour
{
    public UnityEvent Transferred => transferred;

    uint LastPlayerId { get; set; }

    [SerializeField] UnityEvent transferred;

    public override void Spawned()
    {
        base.Spawned();

        var networkGrabbable = GetComponent<NetworkGrabbable>();

        networkGrabbable.onDidGrab.AddListener(OnGrabbed);
    }

    void OnGrabbed(NetworkGrabber grabber)
    {
        if (Runner.ActivePlayers.Count() == 1)
            return;

        var playerObject = grabber.transform.root.GetComponent<NetworkObject>();
        var objectId = playerObject.Id;

        if (LastPlayerId == objectId.Raw)
            return;

        LastPlayerId = objectId.Raw;
        transferred?.Invoke();
    }

    public void ResetLastId() => LastPlayerId = 0;
}
