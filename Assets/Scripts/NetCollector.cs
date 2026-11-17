using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Red de la balsa: clic izquierdo o Espacio recoge la basura cercana a netPoint.
    /// Pon un objeto hijo "NetPoint" delante de la balsa y arrástralo aquí.
    /// </summary>
    public class NetCollector : MonoBehaviour
    {
        [SerializeField] Transform netPoint;
        [SerializeField] RaftCargo cargo;
        [Tooltip("La mejora 'red más grande' sube este radio.")]
        public float radius = 3f;
        public int maxPerCast = 5;
        public float cooldown = 0.5f;

        float nextTime;

        void Awake()
        {
            if (cargo == null) cargo = GetComponentInParent<RaftCargo>();
            if (netPoint == null) netPoint = transform;
        }

        void Update()
        {
            if (Time.timeScale == 0f || Time.time < nextTime || !InputUtil.NetPressed()) return;
            nextTime = Time.time + cooldown;
            Cast();
        }

        void Cast()
        {
            float r2 = radius * radius;
            int got = 0;
            for (int i = TrashItem.Active.Count - 1; i >= 0 && got < maxPerCast; i--)
            {
                TrashItem t = TrashItem.Active[i];
                if ((t.transform.position - netPoint.position).sqrMagnitude > r2) continue;

                if (!cargo.TryAdd(t.type, t.value))
                {
                    GameEvents.Message("¡La balsa está llena! Ve al punto de acopio.");
                    break;
                }
                t.Collect();
                GameEvents.TrashCollected(t.type);
                got++;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(netPoint != null ? netPoint.position : transform.position, radius);
        }
    }
}
