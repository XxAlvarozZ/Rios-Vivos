using System.Collections.Generic;
using UnityEngine;

namespace RiosVivos
{
    /// <summary>Basura flotante. Cada prefab (botella, bolsa, lata, llanta, caja) lleva este script.</summary>
    public class TrashItem : MonoBehaviour
    {
        public static readonly List<TrashItem> Active = new List<TrashItem>();

        public TrashType type = TrashType.Plastic;
        [Tooltip("Valor al entregar (más valor = más monedas).")]
        public int value = 1;
        public float bobHeight = 0.08f;
        public float bobSpeed = 1.5f;

        Vector3 basePos;
        float phase;

        void OnEnable()
        {
            Active.Add(this);
            basePos = transform.position;
            phase = Random.value * 6.28f;
        }

        void OnDisable() => Active.Remove(this);

        void Update()
        {
            transform.position = basePos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed + phase) * bobHeight);
        }

        public void Collect() => gameObject.SetActive(false);
    }
}
