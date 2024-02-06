using Fusion.Sockets;
using Fusion.XR.Host.Grabbing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fusion.XR.Host.Rig
{
    public enum RigPart
    {
        None,
        Headset,
        LeftController,
        RightController,
        Undefined
    }

    public struct RigInput : INetworkInput
    {
        public Vector3 playAreaPosition;
        public Quaternion playAreaRotation;

        public Vector3 rightHandPosition;
        public Quaternion rightHandRotation;

        public Vector3 leftHandPosition;
        public Quaternion leftHandRotation;

        public Vector3 headsetPosition;
        public Quaternion headsetRotation;

        public HandCommand leftHandCommand;
        public HandCommand rightHandCommand;

        public GrabInfo leftGrabInfo;
        public GrabInfo rightGrabInfo;
    }

    public class HardwareRig : MonoBehaviour, INetworkRunnerCallbacks
    {

        public HardwareHeadset Headset => headset;
        public HardwareHand RightHand => rightHand;
        public HardwareHand LeftHand => leftHand;
        public NetworkRunner Runner => runner;

        public float InterpolationDelay => interpolationDelay;

        [SerializeField] HardwareHeadset headset;
        [SerializeField] HardwareHand rightHand;
        [SerializeField] HardwareHand leftHand;
        [SerializeField] NetworkRunner runner;

        XRControllerInputDevice rightHandInputDevice;
        XRControllerInputDevice leftHandInputDevice;
        XRHeadsetInputDevice headsetInputDevice;

        [SerializeField] float interpolationDelay;

        [SerializeField] bool useInputInterpolation;

        protected virtual void Awake()
        {
            if (rightHand)
                rightHandInputDevice = rightHand.GetComponentInChildren<XRControllerInputDevice>();

            if (leftHand)
                leftHandInputDevice = leftHand.GetComponentInChildren<XRControllerInputDevice>();

            if (headset)
                headsetInputDevice = headset.GetComponentInChildren<XRHeadsetInputDevice>();

            if (leftHandInputDevice == null || rightHandInputDevice == null || headsetInputDevice == null)
                useInputInterpolation = false;
        }

        protected void Start()
        {
            if (runner)
                runner.AddCallbacks(this);
        }

        private void OnDestroy()
        {
            if (runner)
                runner.RemoveCallbacks(this);
        }


        public virtual void Rotate(float angle)
        {
            transform.RotateAround(headset.transform.position, transform.up, angle);
        }

        public virtual void Teleport(Vector3 position)
        {
            Vector3 headsetOffet = headset.transform.position - transform.position;
            headsetOffet.y = 0;
            transform.position = position - headsetOffet;
        }

        public virtual IEnumerator FadedTeleport(Vector3 position)
        {
            if (headset.fader)
                yield return headset.fader.FadeIn();

            Teleport(position);

            if (headset.fader)
                yield return headset.fader.WaitBlinkDuration();

            if (headset.fader)
                yield return headset.fader.FadeOut();
        }

        public virtual IEnumerator FadedRotate(float angle)
        {
            if (headset.fader)
                yield return headset.fader.FadeIn();

            Rotate(angle);

            if (headset.fader)
                yield return headset.fader.WaitBlinkDuration();

            if (headset.fader)
                yield return headset.fader.FadeOut();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            RigInput rigInput = new RigInput();

            rigInput.playAreaPosition = transform.position;
            rigInput.playAreaRotation = transform.rotation;

            if (useInputInterpolation)
            {
                var rightHandInterpolationPose = rightHandInputDevice.InterpolatedPose(InterpolationDelay);
                var leftHandInterpolationPose = leftHandInputDevice.InterpolatedPose(InterpolationDelay);

                var headsetInterpolationPose = headsetInputDevice.InterpolatedPose(InterpolationDelay);

                rigInput.rightHandPosition = rightHandInterpolationPose.position;
                rigInput.rightHandRotation = rightHandInterpolationPose.rotation;
                rigInput.leftHandPosition = leftHandInterpolationPose.position;
                rigInput.leftHandRotation = leftHandInterpolationPose.rotation;
                rigInput.headsetPosition = headsetInterpolationPose.position;
                rigInput.headsetRotation = headsetInterpolationPose.rotation;
            }
            else
            {
                rigInput.rightHandPosition = rightHand.transform.position;
                rigInput.rightHandRotation = rightHand.transform.rotation;
                rigInput.leftHandPosition = leftHand.transform.position;
                rigInput.leftHandRotation = leftHand.transform.rotation;
                rigInput.headsetPosition = headset.transform.position;
                rigInput.headsetRotation = headset.transform.rotation;
            }

            rigInput.rightHandCommand = rightHand.handCommand;
            rigInput.leftHandCommand = leftHand.handCommand;

            rigInput.rightGrabInfo = rightHand.grabber.GrabInfo;
            rigInput.leftGrabInfo = leftHand.grabber.GrabInfo;

            input.Set(rigInput);
        }
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
        public void OnConnectedToServer(NetworkRunner runner) { }
        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
    }
}
