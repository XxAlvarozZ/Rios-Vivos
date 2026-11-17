using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Punto de acopio en la orilla (Collider con Is Trigger). Cuando la balsa entra:
    /// descarga la basura, da monedas, sube la calidad del agua, repara la balsa
    /// y guarda este punto como checkpoint de reinicio.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class DockPoint : MonoBehaviour
    {
        [Tooltip("Dónde reaparece la balsa si se rompe (un objeto vacío en el agua, mirando río abajo).")]
        public Transform respawnPoint;
        public int coinsPerValue = 2;

        void Reset() => GetComponent<Collider>().isTrigger = true;

        void OnTriggerEnter(Collider other)
        {
            RaftCargo cargo = other.GetComponentInParent<RaftCargo>();
            if (cargo == null) return;

            GameEvents.CheckpointSet(respawnPoint != null ? respawnPoint : transform);
            GameEvents.RepairRequested();

            if (cargo.Total == 0)
            {
                GameEvents.Message("Punto de acopio: balsa reparada.");
                return;
            }

            cargo.Unload(out int items, out int value);
            int coins = value * coinsPerValue;
            if (CoinWallet.Instance != null) CoinWallet.Instance.Add(coins);
            if (WaterQualityMeter.Instance != null) WaterQualityMeter.Instance.AddCleaned(items);
            GameEvents.Message($"Entregaste {items} residuos: +{coins} monedas.");
        }
    }
}
