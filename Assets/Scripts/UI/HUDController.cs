using Core;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI resourcesText;

        private void Awake()
        {
            if (CurrencyManager.Instance == null) return;
            resourcesText.text = CurrencyManager.Instance.CurrentMoney.ToString();
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