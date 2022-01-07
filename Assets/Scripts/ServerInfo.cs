internal class ServerInfo
{
    internal string Name { get; }
    internal byte CurrentPlayer { get; }
    internal byte MaxPlayers { get; }
    internal ushort Ping { get; }
    internal string Address { get; }
    internal ushort Port { get; }

    internal ServerInfo(string name, byte currentPlayers, byte maxPlayers, ushort ping, string address, ushort port)
    {
        Name = name;
        CurrentPlayer = currentPlayers;
        MaxPlayers = maxPlayers;
        Ping = ping;
        Address = address;
        Port = port;
    }

    public override string ToString() => $"Name: {Name}\nPlayers: {CurrentPlayer} / {MaxPlayers}\nPing: {Ping}\nAddress: {Address}\nPort: {Port}";
}
