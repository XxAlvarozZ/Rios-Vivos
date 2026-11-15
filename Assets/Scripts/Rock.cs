using UnityEngine;

namespace RiosVivos
{
    /// <summary>Marca una roca/obstáculo. RaftDurability lo lee para calcular el daño del choque.</summary>
    public class Rock : MonoBehaviour
    {
        [Tooltip("1 = daño normal. Puentes y muros del Rímac pueden usar 1.5 o 2.")]
        public float damageMultiplier = 1f;
    }
}
