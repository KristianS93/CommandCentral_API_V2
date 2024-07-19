using System.Collections.Concurrent;
using API.GroceryList.Models;

namespace API.GroceryList.Hubs;

public class SharedConnections
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new();
    public ConcurrentDictionary<string, UserConnection> Connections => _connections;
}