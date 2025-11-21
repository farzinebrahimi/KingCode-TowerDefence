using Core;
using Core.Interfaces;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI resourcesText;
        [SerializeField]
        private Slider castleHealthSlider;
        [SerializeField]
        private TextMeshProUGUI messageText;
        
        private ICurrencyManager _currencyManager;

        private void Start()
        {
            _currencyManager = CurrencyManager.Instance;
            if (_currencyManager== null) return;
            resourcesText.text = _currencyManager.CurrentMoney.ToString();
            castleHealthSlider.value = 500;
        }


        private void OnEnable()
        {
            EventBus.Subscribe<MoneyChangedEvent>(OnResourceHandler);
            EventBus.Subscribe<CastleAttackEvent>(OnCastleAttack);
            EventBus.Subscribe<SendMessageEvent>(OnSendMessage);
        }
        private void OnDisable()
        {
            EventBus.Unsubscribe<MoneyChangedEvent>(OnResourceHandler);
            EventBus.Unsubscribe<CastleAttackEvent>(OnCastleAttack);
            EventBus.Unsubscribe<SendMessageEvent>(OnSendMessage);
        }

        private void OnSendMessage(SendMessageEvent e)
        {
            messageText.text = e.Message;
        }

        private void OnCastleAttack(CastleAttackEvent e)
        {
            castleHealthSlider.value = e.CurrentHealth / 100;
        }

        private  void OnResourceHandler(MoneyChangedEvent e)
        {
            resourcesText.text = e.CurrentAmount.ToString();
        }
        

        
    }

    
}