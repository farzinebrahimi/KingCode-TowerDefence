using Core;
using Core.Interfaces;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI resourcesText;
        
        private ICurrencyManager _currencyManager;

        private void Start()
        {
            _currencyManager = CurrencyManager.Instance;
            Debug.Log($"Money Is: {_currencyManager.CurrentMoney}");
            if (_currencyManager== null) return;
            resourcesText.text = _currencyManager.CurrentMoney.ToString();
            Debug.Log(resourcesText.text);
        }


        private void OnEnable()
        {
            EventBus.Subscribe<MoneyChangedEvent>(OnResourceHandler);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<MoneyChangedEvent>(OnResourceHandler);
        }
        
        private  void OnResourceHandler(MoneyChangedEvent e)
        {
            resourcesText.text = e.CurrentAmount.ToString();
        }

        
    }

    
}