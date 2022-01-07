using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class HostListener : MonoBehaviour
{
    private NetworkDriver _driver;
    private NativeList<NetworkConnection> _connections;

    private readonly List<ServerInfo> _availableServers = new();

    private void Start()
    {
        _driver = NetworkDriver.Create();

        NetworkEndPoint endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = 9000;

        if (_driver.Bind(endpoint) != 0)
        {
            Debug.Log("Failed to bind to port 9000");
        }
        else
        {
            _driver.Listen();
        }

        _connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    private void Update()
    {
        _driver.ScheduleUpdate().Complete();

        for (int i = 0; i < _connections.Length; ++i)
        {
            if (!_connections[i].IsCreated)
            {
                _connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        NetworkConnection connection;

        while ((connection = _driver.Accept()) != default)
        {
            _connections.Add(connection);
        }

        for (int i = 0; i < _connections.Length; ++i)
        {
            if (!_connections[i].IsCreated)
            {
                continue;
            }

            NetworkEvent.Type cmd;

            while ((cmd = _driver.PopEventForConnection(_connections[i], out DataStreamReader streamReader)) != NetworkEvent.Type.Empty)
            {
                if (cmd == NetworkEvent.Type.Data)
                {
                    Debug.Log("Received data from a new client");

                    ServerInfo server = new(
                            streamReader.ReadFixedString32().ToString(),
                            streamReader.ReadByte(),
                            streamReader.ReadByte(),
                            streamReader.ReadUShort(),
                            streamReader.ReadFixedString32().ToString(),
                            streamReader.ReadUShort());

                    _availableServers.Add(server);

                    Debug.Log(server);

                    _driver.BeginSend(NetworkPipeline.Null, _connections[i], out DataStreamWriter writer);

                    AppendServerList(ref writer);

                    _driver.EndSend(writer);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from server");
                    _connections[i] = default;
                }
            }
        }
    }

    private void AppendServerList(ref DataStreamWriter writer)
    {
        writer.WriteInt(_availableServers.Count);

        foreach (ServerInfo server in _availableServers)
        {
            writer.WriteFixedString32(server.Name);
            writer.WriteByte(server.CurrentPlayer);
            writer.WriteByte(server.MaxPlayers);
            writer.WriteUShort(server.Ping);
            writer.WriteFixedString32(server.Address);
            writer.WriteUShort(server.Port);
        }
    }

    private void OnDestroy()
    {
        _driver.Dispose();
        _connections.Dispose();
    }
}
