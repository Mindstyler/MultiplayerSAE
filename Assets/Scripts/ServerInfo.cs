using System;

[UnityTransportSerialize]
public partial class ServerInfo
{
    [NetworkSerialize("string32")]
    internal string Name { get; }
    [NetworkSerialize("byte")]
    internal byte CurrentPlayer { get; }
    [NetworkSerialize("byte")]
    internal byte MaxPlayers { get; }
    internal ushort Ping { get; }
    [NetworkSerialize("string32")]
    internal string Address { get; }
    [NetworkSerialize("ushort")]
    internal ushort Port { get; }

    internal ServerInfo(string name, byte currentPlayers, byte maxPlayers, /*ushort ping,*/ string address, ushort port)
    {
        Name = name;
        CurrentPlayer = currentPlayers;
        MaxPlayers = maxPlayers;
        /*Ping = ping;*/
        Address = address;
        Port = port;
    }

    public override string ToString() => $"Name: {Name}\nPlayers: {CurrentPlayer} / {MaxPlayers}\nPing: {Ping}\nAddress: {Address}\nPort: {Port}";
}


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class UnityTransportSerializeAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public sealed class NetworkSerialize : Attribute
{
    internal string Type { get; }

    public NetworkSerialize(string type) => Type = type;
}
