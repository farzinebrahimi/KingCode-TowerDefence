using Core;

using UnityEngine;
using UnityEngine.UI;
using EventBus = Core.EventBus;

namespace UI
{
    public class TowerBuildButton: MonoBehaviour
    {
        [SerializeField]
        private Button placementTowerBtn;

        private void Start()
        {
            placementTowerBtn.onClick.AddListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            EventBus.Publish(new TowerPlacementStateChangedEvent(true));
            EventBus.Publish(new BeginTowerPlacementEvent());
        }
    }
}