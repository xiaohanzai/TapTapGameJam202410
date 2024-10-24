using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    // Dictionary to store services by type
    private static readonly Dictionary<System.Type, object> _services = new Dictionary<System.Type, object>();

    // Register a service to the locator
    public static void Register<T>(T service)
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            Debug.LogWarning($"Service {type.Name} is already registered.");
            return;
        }

        _services[type] = service;
    }

    // Retrieve a service from the locator
    public static T Get<T>()
    {
        var type = typeof(T);
        if (_services.TryGetValue(type, out var service))
        {
            return (T)service;
        }
        else
        {
            Debug.LogError($"Service {type.Name} not found!");
            return default;
        }
    }

    // Unregister a service
    public static void Unregister<T>()
    {
        var type = typeof(T);
        if (_services.ContainsKey(type))
        {
            _services.Remove(type);
        }
    }
}
