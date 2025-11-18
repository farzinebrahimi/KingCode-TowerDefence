using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
                _subscribers[type] = new List<Delegate>();
            _subscribers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var list))
                list.Remove(handler);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var list))
                return;
            foreach (var handler in list)
                ((Action<T>)handler)?.Invoke(eventData);
        }
    }
    
    //TODO add all event types here ^^
    public readonly struct TowerPlacedEvent
    {
        public readonly Transform Tower;
        public TowerPlacedEvent(Transform tower) => Tower = tower;
    }
    public readonly struct MouseClickEvent
    {
        public readonly Vector3 WorldPosition;

        public MouseClickEvent(Vector3 worldPosition)
        {
            WorldPosition = worldPosition;
        }
    }
    public class PathConstructedEvent
    {
        public List<Vector3> Waypoints { get; }

        public PathConstructedEvent(List<Vector3> waypoints)
        {
            Waypoints = waypoints;
        }
    }

}