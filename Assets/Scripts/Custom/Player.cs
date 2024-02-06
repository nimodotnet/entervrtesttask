using Fusion;
using Fusion.XR.Host.Locomotion;
using Fusion.XR.Host.Rig;
using System.Linq;
using UnityEngine;

public class Plaeyr : NetworkBehaviour
{
    [SerializeField] GameObject teacherBody;
    [SerializeField] GameObject studentBody;

    [Networked] bool IsTeacher { get; set; }

    HardwareRig hardwareRig;

    private void Awake()
    {
        hardwareRig = FindObjectOfType<HardwareRig>();
    }

    public override void Spawned()
    {
        base.Spawned();


        if (Runner.ActivePlayers.Count() == 1)
            IsTeacher = true;

        if (IsTeacher)
            teacherBody.SetActive(true);
        else
            studentBody.SetActive(true);

        if (HasInputAuthority)
        {
            var spawnPoint = SpawnManager.Instance.GetSpawnPoint();

            hardwareRig.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);

            if (!IsTeacher)
                return;

            hardwareRig.Headset.GetComponent<VisionInteractor>().enabled = true;
            hardwareRig.RightHand.GetComponent<RayBeamer>().enabled = true;
            hardwareRig.LeftHand.GetComponent<RayBeamer>().enabled = true;

            hardwareRig.GetComponent<RigLocomotion>().enabled = true;
        }
    }

}
