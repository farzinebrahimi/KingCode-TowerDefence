using System;
using Core;
using UnityEngine;

namespace Managers
{
    public class SoundFxManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource sfxSource;

        [Header("Audio Clips")] 
        [SerializeField]
        private AudioClip shotFireSfx;
        [SerializeField]
        private AudioClip enemyDeadSfx;
        [SerializeField]
        private AudioClip collectMoneySfx;
        [SerializeField]
        private AudioClip clickBtnSfx;


        private void OnEnable()
        {
            EventBus.Subscribe<ShotFiredEvent>(OnFire);
            EventBus.Subscribe<EnemyKilledEvent>(OnEnemyDie);
            EventBus.Subscribe<MoneyChangedEvent>(OnCollectMoney);
            EventBus.Subscribe<BeginTowerPlacementEvent>(OnClickBtn);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<ShotFiredEvent>(OnFire);
            EventBus.Unsubscribe<EnemyKilledEvent>(OnEnemyDie);
            EventBus.Unsubscribe<MoneyChangedEvent>(OnCollectMoney);
            EventBus.Unsubscribe<BeginTowerPlacementEvent>(OnClickBtn);
        }

        private void OnClickBtn(BeginTowerPlacementEvent obj)
        {
            sfxSource.PlayOneShot(clickBtnSfx);
        }

        private void OnCollectMoney(MoneyChangedEvent obj)
        {
            sfxSource.PlayOneShot(collectMoneySfx);
        }

        private void OnEnemyDie(EnemyKilledEvent obj)
        {
            sfxSource.PlayOneShot(enemyDeadSfx);
        }

        private void OnFire(ShotFiredEvent e)
        {
            sfxSource.PlayOneShot(shotFireSfx);
        }
    }
}