using UnityEngine;

namespace RiosVivos
{
    /// <summary>Carga de la balsa: capacidad y conteo por tipo de basura. Va en el objeto Raft.</summary>
    public class RaftCargo : MonoBehaviour
    {
        public int capacity = 10;

        int[] counts;
        int total;
        int totalValue;

        public int Total => total;
        public int TotalValue => totalValue;
        public bool IsFull => total >= capacity;
        public int GetCount(TrashType t) => counts[(int)t];

        void Awake() => counts = new int[System.Enum.GetValues(typeof(TrashType)).Length];

        void OnEnable() => GameEvents.OnRaftRespawned += Clear;
        void OnDisable() => GameEvents.OnRaftRespawned -= Clear;

        void Start() => GameEvents.CapacityChanged(total, capacity);

        public bool TryAdd(TrashType type, int value)
        {
            if (IsFull) return false;
            counts[(int)type]++;
            total++;
            totalValue += value;
            GameEvents.CapacityChanged(total, capacity);
            return true;
        }

        /// <summary>Descarga todo y devuelve cuántos objetos y cuánto valor había.</summary>
        public void Unload(out int items, out int value)
        {
            items = total;
            value = totalValue;
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < counts.Length; i++) counts[i] = 0;
            total = 0;
            totalValue = 0;
            GameEvents.CapacityChanged(total, capacity);
        }

        /// <summary>Mejora "Red más grande / más capacidad" de la tienda.</summary>
        public void UpgradeCapacity(int extra)
        {
            capacity += extra;
            GameEvents.CapacityChanged(total, capacity);
        }
    }
}
