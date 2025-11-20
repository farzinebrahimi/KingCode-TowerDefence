using System;
using Core;
using Core.Interfaces;
using Data;
using UnityEngine;

namespace Managers
{
    public class CurrencyManager : MonoBehaviour, ISpendMoney , IAddMoney
    {
        [SerializeField] private PlayerData playerData;
        private  int _currentMoney;

        private void Awake()
        {
            if(playerData != null)
                _currentMoney = playerData.money;
            PublishMoneyUpdate();
        }

        private void OnEnable()
        {
            
        }


        public bool SpendMoney(int amount)
        {
            if (_currentMoney >= amount)
            {
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