using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Player", menuName = "Data/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public string playerName;
        public int money;
        public int moneyPerKill;
        

    }
}