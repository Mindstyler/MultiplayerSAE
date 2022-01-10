using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Networking.Transport;
using UnityEngine;

internal class ServerManager : MonoBehaviour
{
    internal static ServerManager Instance { get; private set; }
    internal ObservableCollection<ServerInfo> ServerList { get; } = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    StartCoroutine(DebugAddMoreServersContinuously());
    //}

    internal void HostNewServer(string serverName, ushort portNumber, byte maxPlayers)
    {
        ServerInfo hostedServer = new(serverName, 1, maxPlayers, NetworkEndPoint.AnyIpv4.Address, portNumber);
    }

    internal void ConnectToServer(int sourceIndex)
    {
        Debug.Log($"tried to connect to server {ServerList[sourceIndex].Name}");
    }

    private IEnumerator DebugAddMoreServersContinuously()
    {
        int i = 0;

        while (true)
        {
            ServerList.Add(new ServerInfo($"Generated Server {++i}", (byte)Random.Range(0, 11), 10, NetworkEndPoint.AnyIpv4.Address, 9000));

            yield return new WaitForSeconds(0.8f);
        }
    }
}
