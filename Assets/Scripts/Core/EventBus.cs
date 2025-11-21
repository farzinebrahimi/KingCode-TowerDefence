using System;
using System.Collections.Generic;
using Data;
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

    #region Tower Placement Events

    public readonly struct BeginTowerPlacementEvent
    {
        public readonly Sprite TowerSprite;
        public BeginTowerPlacementEvent(Sprite towerSprite) => TowerSprite = towerSprite;
    }
    public readonly struct TowerPlacementStateChangedEvent
    {
        public readonly bool IsPlacementActive;

        public TowerPlacementStateChangedEvent(bool isActive) => IsPlacementActive = isActive;
        
    }
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

    #endregion

    #region Tower Target Events

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

    #endregion

    #region Currency Events

    public readonly struct MoneyChangedEvent
    {
        public readonly int Money;
        public MoneyChangedEvent(int money) => Money = money;
    }
    public readonly struct EnemyKilledEvent
    {
        public readonly int MoneyReward;
        public readonly Vector3 Position;

        public EnemyKilledEvent(int moneyReward, Vector3 position)
        {
            MoneyReward = moneyReward;
            Position = position;
        }
    }

    #endregion
    
    public struct TowerUpgradedEvent
    {
        public GameObject Tower;
        public int NewLevel;

        public TowerUpgradedEvent(GameObject tower, int newLevel)
        {
            Tower = tower;
            NewLevel = newLevel;
        }
    }

    
}
