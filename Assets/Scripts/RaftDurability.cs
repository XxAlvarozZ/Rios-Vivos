using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Resistencia de la balsa. Baja al chocar con objetos que tengan Rock.
    /// Se repara con GameEvents.RepairRequested() (lo emite el punto de acopio).
    /// Al llegar a 0 emite RaftDestroyed y GameManager la reinicia en el último acopio.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RaftDurability : MonoBehaviour
    {
        public int maxDurability = 100;
        public float minImpactSpeed = 1.5f;
        public float damagePerSpeed = 6f;
        public float hitCooldown = 0.5f;

        int current;
        float lastHit = -99f;
        public int Current => current;

        void OnEnable()
        {
            GameEvents.OnRepairRequested += Repair;
            GameEvents.OnRaftRespawned += Repair;
        }

        void OnDisable()
        {
            GameEvents.OnRepairRequested -= Repair;
            GameEvents.OnRaftRespawned -= Repair;
        }

        void Start()
        {
            current = maxDurability;
            GameEvents.DurabilityChanged(current, maxDurability);
        }

        void OnCollisionEnter(Collision c)
        {
            Rock rock = c.collider.GetComponentInParent<Rock>();
            if (rock == null || Time.time - lastHit < hitCooldown) return;

            float speed = c.relativeVelocity.magnitude;
            if (speed < minImpactSpeed) return;

            lastHit = Time.time;
            Damage(Mathf.RoundToInt(speed * damagePerSpeed * rock.damageMultiplier));
        }

        public void Damage(int amount)
        {
            if (current <= 0) return;
            current = Mathf.Max(0, current - amount);
            GameEvents.DurabilityChanged(current, maxDurability);
            if (current == 0) GameEvents.RaftDestroyed();
        }

        public void Repair()
        {
            current = maxDurability;
            GameEvents.DurabilityChanged(current, maxDurability);
        }
    }
}
