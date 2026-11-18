using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Tronco o desecho grande que flota y es arrastrado por la corriente (RiverCurrent).
    /// Sirve de obstáculo móvil en el Rímac y en el Nanay (troncos flotando).
    /// Requiere Rigidbody + Collider + Rock (el daño al chocar lo calcula RaftDurability).
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Rock))]
    public class DriftingLog : MonoBehaviour
    {
        [Header("Flotación")]
        public float waterLevelY = 0f;
        public float buoyancy = 20f;
        public float damping = 4f;

        [Header("Reciclaje (para que el río siempre tenga obstáculos)")]
        public bool recycle = true;
        [Tooltip("Si pasa de esta Z, vuelve a su punto inicial.")]
        public float endZ = 40f;

        Rigidbody rb;
        Vector3 startPos;
        Quaternion startRot;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = 1.5f;
            rb.linearDamping = 0.8f;
            rb.angularDamping = 1.5f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            startPos = transform.position;
            startRot = transform.rotation;
        }

        void FixedUpdate()
        {
            float depth = waterLevelY - rb.position.y;
            rb.AddForce(Vector3.up * (depth * buoyancy - rb.linearVelocity.y * damping), ForceMode.Acceleration);

            if (recycle && rb.position.z > endZ)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.position = startPos;
                rb.rotation = startRot;
            }
        }
    }
}
