using System;
using System.Collections.Generic;

public static class ServiceLoader
{
    private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void AddService<T>(T service)
    {
        if (!_services.ContainsKey(typeof(T)))
            _services.Add(typeof(T), service); 
        else
            throw new Exception($"Service of type {typeof(T)} already exists");
    }

    public static T GetService<T>() where T : class
    {
        if (_services.TryGetValue(typeof(T), out var service))
            return (T)service;
        else
            throw new Exception($"Service of type {typeof(T)} not found");
    }

    public static void RemoveService<T>()
    {
        if (_services.ContainsKey(typeof(T)))
            _services.Remove(typeof(T));
        else
            throw new Exception($"Service of type {typeof(T)} not found");
    }

    public static void ClearServices()
    {
        _services.Clear();
    }
}
