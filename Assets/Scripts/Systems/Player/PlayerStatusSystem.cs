using System;
using UnityEngine;

namespace Lingtianlu
{
    public class PlayerStatusSystem : MonoBehaviour
    {
        [SerializeField, Min(0)] private int startGold = 120;
        [SerializeField, Min(1)] private int maxStamina = 100;

        public event Action<int> GoldChanged;
        public event Action<int> StaminaChanged;

        public int Gold { get; private set; }
        public int Stamina { get; private set; }

        private void Awake()
        {
            Gold = startGold;
            Stamina = maxStamina;
        }

        public bool SpendStamina(int amount)
        {
            if (amount <= 0 || Stamina < amount)
            {
                return false;
            }

            Stamina -= amount;
            StaminaChanged?.Invoke(Stamina);
            return true;
        }

        public void RecoverDaily(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Stamina = Mathf.Clamp(Stamina + amount, 0, maxStamina);
            StaminaChanged?.Invoke(Stamina);
        }

        public void AddGold(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            Gold += amount;
            GoldChanged?.Invoke(Gold);
        }

        public bool SpendGold(int amount)
        {
            if (amount <= 0 || Gold < amount)
            {
                return false;
            }

            Gold -= amount;
            GoldChanged?.Invoke(Gold);
            return true;
        }
    }
}
