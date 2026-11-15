using UnityEngine;

namespace RiosVivos
{
    /// <summary>Monedas del jugador. Un solo objeto en la escena.</summary>
    public class CoinWallet : MonoBehaviour
    {
        public static CoinWallet Instance { get; private set; }

        int coins;
        public int Coins => coins;

        void Awake() => Instance = this;
        void Start() => GameEvents.CoinsChanged(coins);

        public void Add(int amount)
        {
            coins += Mathf.Max(0, amount);
            GameEvents.CoinsChanged(coins);
        }

        public bool TrySpend(int amount)
        {
            if (coins < amount) return false;
            coins -= amount;
            GameEvents.CoinsChanged(coins);
            return true;
        }
    }
}
