using Fusion.Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Fusion;

public class ConnectionManager : MonoBehaviour, INetworkRunnerCallbacks
{
    public UnityEvent PlayerJoined => playerJoined;
    public UnityEvent PlayerLeft => playerLeft;

    public UnityEvent Conenected => connected;

    [SerializeField] UnityEvent playerJoined;
    [SerializeField] UnityEvent playerLeft;

    [SerializeField] NetworkObject playerPrefab;
    [SerializeField] NetworkRunner runner;

    [SerializeField] UnityEvent connected;
    Dictionary<PlayerRef, NetworkObject> spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();

    INetworkSceneManager sceneManager;

    private void Awake()
    {
        if (runner == null)
            runner = GetComponent<NetworkRunner>();

        if (runner == null)
            runner = gameObject.AddComponent<NetworkRunner>();

        runner.ProvideInput = true;
    }

    private async void Start()
    {
        await Connect();
    }

    public async Task Connect()
    {
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();

        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        var args = new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        };

        await runner.StartGame(args);

        print("Connected!");

        connected?.Invoke();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer && playerPrefab != null)
        {
            print($"Player joined! PayerId: {player.PlayerId}");

            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, position: transform.position, rotation: transform.rotation, inputAuthority: player);
            spawnedPlayers.Add(player, networkPlayerObject);

            playerJoined?.Invoke();
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (spawnedPlayers.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedPlayers.Remove(player);

            playerLeft?.Invoke();
        }
    }

    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}


