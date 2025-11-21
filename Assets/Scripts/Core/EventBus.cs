using System;
using System.Collections.Generic;
using Data;
using TowerSystem;
using UnityEngine;

namespace Core
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> Subscribers = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!Subscribers.ContainsKey(type))
                Subscribers[type] = new List<Delegate>();
            Subscribers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (Subscribers.TryGetValue(type, out var handlers))
                handlers.Remove(handler);
        }

        public static void Publish<T>(T eventData)
        {
            var type = typeof(T);
            if (!Subscribers.TryGetValue(type, out var handlers))
                return;

            foreach (var handler in handlers)
                ((Action<T>)handler)?.Invoke(eventData);
        }
    }

    

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

    public struct MouseClickEvent
    {
        public  Vector3 WorldPosition;
        public  bool IsConsumed;

        public MouseClickEvent(Vector3 worldPosition)
        {
            WorldPosition = worldPosition;
            IsConsumed = false;
        }
        public void Consume()
        {
            IsConsumed = true;
        }
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

    public  struct MoneyChangedEvent
    {
        public  int CurrentAmount;
        public  int ChangeAmount;

        public MoneyChangedEvent(int currentAmount, int changeAmount)
        {
            CurrentAmount = currentAmount;
            ChangeAmount = changeAmount;
        }
    }
    public struct MoneySpendFailedEvent
    {
        public int RequiredAmount;
        public int CurrentAmount;

        public MoneySpendFailedEvent(int requiredAmount, int currentAmount)
        {
            RequiredAmount = requiredAmount;
            CurrentAmount = currentAmount;
        }
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

    #region Upgrade System Events

    public struct TowerUpgradedEvent
    {
        public Tower UpgradedTower;

        public TowerUpgradedEvent(Tower upgradedTower)
        {
            UpgradedTower = upgradedTower;
        }
    }
    public struct TowerUpgradeFailedEvent
    {
        public Tower Tower;
        public string Reason;

        public TowerUpgradeFailedEvent(Tower tower, string reason)
        {
            Tower = tower;
            Reason = reason;
        }
    }

    public struct TowerSelectedEvent
    {
        public Transform SelectedTower;
        public TowerSelectedEvent(Transform selectedTower) => SelectedTower = selectedTower;
    }

    public struct TowerDeselectedEvent
    {
        public Transform DeselectedTower;
        public TowerDeselectedEvent(Transform deselectedTower) => DeselectedTower = deselectedTower;
    }

    #endregion

    
}
