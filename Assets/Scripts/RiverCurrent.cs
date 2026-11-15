using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Zona de corriente. Pon un BoxCollider (Is Trigger) sobre un tramo del río;
    /// todo Rigidbody dentro es empujado en la dirección indicada.
    /// Río Rímac: strength más alto. Mantaro: moderado.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class RiverCurrent : MonoBehaviour
    {
        [Tooltip("Dirección local de la corriente (por defecto hacia +Z del objeto).")]
        public Vector3 localDirection = Vector3.forward;
        public float strength = 3f;

        void Reset() => GetComponent<BoxCollider>().isTrigger = true;

        void OnTriggerStay(Collider other)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb == null) return;
            Vector3 dir = transform.TransformDirection(localDirection);
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;
            rb.AddForce(dir.normalized * strength, ForceMode.Acceleration);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Vector3 dir = transform.TransformDirection(localDirection).normalized;
            Gizmos.DrawRay(transform.position, dir * 4f);
        }
    }
}
