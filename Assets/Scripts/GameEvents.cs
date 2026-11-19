using System;
using UnityEngine;

namespace RiosVivos
{
    public enum TrashType { Plastic, Organic, Metal, NonRecyclable }
    public enum ObjectiveType { Barrier, DredgeReport }

    /// <summary>
    /// Bus de eventos compartido. Cada script emite y escucha eventos aquí,
    /// así nadie depende del script de otro y cada commit compila solo.
    /// </summary>
    public static class GameEvents
    {
        public static event Action<TrashType> OnTrashCollected;
        public static event Action<int, int> OnCapacityChanged;       // actual, máximo
        public static event Action<int, int> OnDurabilityChanged;     // actual, máximo
        public static event Action<int, float> OnWaterQualityChanged; // riverId, 0-100
        public static event Action<int> OnCoinsChanged;
        public static event Action<int> OnRiverUnlocked;              // índice del río desbloqueado
        public static event Action<Transform> OnCheckpointSet;
        public static event Action OnRaftDestroyed;
        public static event Action OnRaftRespawned;
        public static event Action OnRepairRequested;
        public static event Action<string> OnMessage;
        public static event Action<int> OnBonusCleanup;                 // residuos "equivalentes" (barreras, reportes)
        public static event Action<ObjectiveType> OnObjectiveCompleted;

        public static void TrashCollected(TrashType t) => OnTrashCollected?.Invoke(t);
        public static void CapacityChanged(int cur, int max) => OnCapacityChanged?.Invoke(cur, max);
        public static void DurabilityChanged(int cur, int max) => OnDurabilityChanged?.Invoke(cur, max);
        public static void WaterQualityChanged(int river, float q) => OnWaterQualityChanged?.Invoke(river, q);
        public static void CoinsChanged(int coins) => OnCoinsChanged?.Invoke(coins);
        public static void RiverUnlocked(int river) => OnRiverUnlocked?.Invoke(river);
        public static void CheckpointSet(Transform t) => OnCheckpointSet?.Invoke(t);
        public static void RaftDestroyed() => OnRaftDestroyed?.Invoke();
        public static void RaftRespawned() => OnRaftRespawned?.Invoke();
        public static void RepairRequested() => OnRepairRequested?.Invoke();
        public static void Message(string text) => OnMessage?.Invoke(text);
        public static void BonusCleanup(int items) => OnBonusCleanup?.Invoke(items);
        public static void ObjectiveCompleted(ObjectiveType t) => OnObjectiveCompleted?.Invoke(t);
    }
}
