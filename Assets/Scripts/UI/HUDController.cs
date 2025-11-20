using System;
using Core;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI resourcesText;

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
            resourcesText.text = e.Money.ToString();
        }

        
    }

    
}