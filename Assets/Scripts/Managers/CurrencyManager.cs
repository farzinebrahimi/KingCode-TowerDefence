using System;
using Core;
using Core.Interfaces;
using Data;
using UnityEngine;

namespace Managers
{
    public class CurrencyManager : MonoBehaviour, ISpendMoney 
    {
        [SerializeField] private PlayerData playerData;
        private  int _currentMoney;

        public static CurrencyManager Instance { get; private set; }


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
    
            if (playerData != null)
                _currentMoney = playerData.money;

            PublishMoneyUpdate();
        }

       

        public bool SpendMoney(int amount)
        {
            if (_currentMoney >= amount)
            {
                Debug.Log($"Spending {amount} money");
                _currentMoney -= amount;
                _currentMoney = Mathf.Clamp(_currentMoney, 0, int.MaxValue);
                PublishMoneyUpdate();
                return true;
            }
            return false;
        }

        public void AddMoney(int money)
        {
            _currentMoney += money;
            PublishMoneyUpdate();
        }

        private void PublishMoneyUpdate()
        {
            EventBus.Publish(new MoneyChangedEvent(_currentMoney));
        }

        public int GetCurrentMoney() => _currentMoney;
    }
}