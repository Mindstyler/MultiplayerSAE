using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

public class NotifyMainServer : MonoBehaviour
{
    private NetworkDriver _driver;
    private NetworkConnection _connection;
    private bool _done;

    private void Start()
    {
        _driver = NetworkDriver.Create();
        _connection = default;

        NetworkEndPoint endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 9000;
        _connection = _driver.Connect(endpoint);
    }

    private void Update()
    {
        _driver.ScheduleUpdate().Complete();

        if (!_connection.IsCreated)
        {
            if (!_done)
            {
                Debug.Log("Something went wrong during connect.");
            }

            return;
        }

        NetworkEvent.Type cmd;

        while ((cmd = _connection.PopEvent(_driver, out DataStreamReader streamReader)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                Debug.Log("Connected to the main server");

                _driver.BeginSend(_connection, out DataStreamWriter writer);

                AppendServerData(ref writer);

                _driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                int elementCount = streamReader.ReadInt();

                Debug.Log($"Received server list with {elementCount} elements");

                if (elementCount > 0)
                {
                    ServerManager.Instance.ServerList.Clear();
                }

                for (int i = 0; i < elementCount; ++i)
                {
                    ServerInfo server = new(
                        streamReader.ReadFixedString32().ToString(),
                        streamReader.ReadByte(),
                        streamReader.ReadByte(),
                        streamReader.ReadUShort(),
                        streamReader.ReadFixedString32().ToString(),
                        streamReader.ReadUShort());

                    ServerManager.Instance.ServerList.Add(server);
                }

                //_done = true;
                //_connection.Disconnect(_driver);
                //_connection = default;
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from the server");

                _connection = default;
            }
        }
    }

    private void AppendServerData(ref DataStreamWriter writer)
    {
        writer.WriteFixedString32("MyServer");
        writer.WriteByte(0); // current players
        writer.WriteByte(4); // max players
        writer.WriteUShort(43);
        writer.WriteFixedString32(NetworkEndPoint.AnyIpv4.Address);
        writer.WriteUShort(9000); // port
    }

    private void OnDestroy()
    {
        _driver.Dispose();
    }
}
