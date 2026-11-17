using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Cámara en tercera persona detrás de la balsa, suavizada y sin atravesar paredes/rocas.
    /// Ponlo en Main Camera y arrastra la balsa a "target".
    /// El agua NO debe tener collider sólido (o debe ser Trigger) para que no bloquee la cámara.
    /// </summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 4f, -8f);
        public float lookHeight = 1.5f;
        public float positionSmooth = 0.15f;
        public float yawSmooth = 0.25f;
        public float collisionRadius = 0.3f;
        public LayerMask obstacles = ~0;

        Vector3 velocity;
        float yaw, yawVelocity;

        void Start()
        {
            if (target != null) yaw = target.eulerAngles.y;
        }

        void LateUpdate()
        {
            if (target == null) return;

            yaw = Mathf.SmoothDampAngle(yaw, target.eulerAngles.y, ref yawVelocity, yawSmooth);
            Vector3 pivot = target.position + Vector3.up * lookHeight;
            Vector3 desired = target.position + Quaternion.Euler(0f, yaw, 0f) * offset;

            Vector3 dir = desired - pivot;
            float dist = dir.magnitude;
            if (dist > 0.01f)
            {
                dir /= dist;
                if (Physics.SphereCast(pivot, collisionRadius, dir, out RaycastHit hit, dist, obstacles, QueryTriggerInteraction.Ignore)
                    && !hit.collider.transform.IsChildOf(target))
                {
                    desired = pivot + dir * Mathf.Max(1f, hit.distance);
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, positionSmooth);
            transform.LookAt(pivot);
        }
    }
}
