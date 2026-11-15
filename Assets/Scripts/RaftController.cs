using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Movimiento de la balsa: W/S remar, A/D girar. La corriente la aplica RiverCurrent.
    /// Requiere Rigidbody + Collider en el mismo objeto (Unity 6: linearDamping / linearVelocity).
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RaftController : MonoBehaviour
    {
        [Header("Remo")]
        public float paddleForce = 18f;
        public float turnTorque = 6f;
        public float maxSpeed = 8f;
        [Tooltip("Las mejoras de la tienda (remo más rápido) suben este valor.")]
        public float speedMultiplier = 1f;
        public bool controlEnabled = true;

        [Header("Flotación")]
        public float waterLevelY = 0f;
        public float buoyancy = 25f;
        public float buoyancyDamping = 4f;

        [Header("Agarre lateral (evita derrapar)")]
        public float sidewaysGrip = 2.5f;

        Rigidbody rb;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false; // la flotación la hace el resorte de FixedUpdate
            rb.linearDamping = 0.8f;
            rb.angularDamping = 2f;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        void FixedUpdate()
        {
            float v = 0f, h = 0f;
            if (controlEnabled)
            {
                v = InputUtil.Vertical();
                h = InputUtil.Horizontal();
            }

            Vector3 forward = transform.forward; forward.y = 0f; forward.Normalize();
            rb.AddForce(forward * (v * paddleForce * speedMultiplier), ForceMode.Acceleration);
            rb.AddTorque(Vector3.up * (h * turnTorque), ForceMode.Acceleration);

            // Quita el movimiento de lado para que la balsa "agarre" el agua
            Vector3 vel = rb.linearVelocity;
            Vector3 sideways = Vector3.Project(vel, transform.right);
            rb.AddForce(-sideways * sidewaysGrip, ForceMode.Acceleration);

            // Limite de velocidad horizontal
            Vector3 flat = new Vector3(vel.x, 0f, vel.z);
            float limit = maxSpeed * speedMultiplier;
            if (flat.magnitude > limit)
            {
                flat = flat.normalized * limit;
                rb.linearVelocity = new Vector3(flat.x, vel.y, flat.z);
            }

            // Flotación: resorte hacia el nivel del agua
            float depth = waterLevelY - rb.position.y;
            rb.AddForce(Vector3.up * (depth * buoyancy - vel.y * buoyancyDamping), ForceMode.Acceleration);
        }
    }
}
