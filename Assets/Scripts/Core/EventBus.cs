using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public static class EventBus
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
            if (_subscribers.TryGetValue(type, out var handlers))
                handlers.Remove(handler);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var handlers))
                return;

            foreach (var handler in handlers)
                ((Action<T>)handler)?.Invoke(eventData);
        }
    }

    //event type

    public readonly struct TowerPlacedEvent
    {
        public readonly Transform Tower;
        public TowerPlacedEvent(Transform tower) => Tower = tower;
    }

    public readonly struct MouseClickEvent
    {
        public readonly Vector3 WorldPosition;
        public MouseClickEvent(Vector3 worldPosition) => WorldPosition = worldPosition;
    }

    public readonly struct PathConstructedEvent
    {
        public List<Vector3> Waypoints { get; }
        public PathConstructedEvent(List<Vector3> waypoints) => Waypoints = waypoints;
    }
    
    public struct GetTargetEvent
    {
        public int TowerID;
        public Transform Target;

        public GetTargetEvent(int towerID, Transform target)
        {
            TowerID = towerID;
            Target = target;
        }
    }
    public struct TargetLostEvent
    {
        public int TowerID;

        public TargetLostEvent(int towerID)
        {
            TowerID = towerID;
        }
    }

    
}
