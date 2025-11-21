using System;
using Core;
using Core.Interfaces;
using Data;
using UnityEngine;

namespace Managers
{
    public class CurrencyManager : MonoBehaviour, ISpendMoney, ICurrencyManager
    {
        [SerializeField] private PlayerData playerData;

        [Header("Starting Money")] [SerializeField]
        private int startingMoney = 100;

        public static CurrencyManager Instance { get; private set; }

        private int _currentMoney;

        public int CurrentMoney => _currentMoney;

        private void OnEnable()
        {
            EventBus.Subscribe<EnemyKilledEvent>(OnEnemyKilled);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyKilled);
        }

        private void OnEnemyKilled(EnemyKilledEvent e)
        {
            AddMoney(e.MoneyReward);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _currentMoney = startingMoney;

        }


        public bool SpendMoney(int amount)
        {
            if (amount < 0) return false;
            if(!HasMoney(amount))
            {
                EventBus.Publish(new MoneySpendFailedEvent(amount, _currentMoney));
                return false;
            }
            _currentMoney -= amount;
            EventBus.Publish(new MoneyChangedEvent(_currentMoney, -amount));
            return true;
        }

        public bool HasMoney(int amount)
        {
            return _currentMoney >= amount;
        }

        public void SetMoney(int amount)
        {
            int prevMoney = _currentMoney;
            _currentMoney += Mathf.Max(0, amount);

            int change = _currentMoney - prevMoney;
            EventBus.Publish(new MoneyChangedEvent(_currentMoney, change));
        }


        public void AddMoney(int amount)
        {
            if (amount < 0) return;
            _currentMoney += amount;
            EventBus.Publish(new MoneyChangedEvent(_currentMoney, amount));
        }
        

        public int GetCurrentMoney() => _currentMoney;
    }
}