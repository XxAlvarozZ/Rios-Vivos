using UnityEngine;
using UnityEngine.SceneManagement;

namespace RiosVivos
{
    /// <summary>
    /// Salida al final de un río (Collider con Is Trigger). Solo deja pasar al siguiente río
    /// cuando WaterQualityMeter desbloqueó (80 % de calidad). Detrás pon una "compuerta" sólida
    /// para que la balsa no se salga del río si todavía está sucio.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class RiverExit : MonoBehaviour
    {
        [Tooltip("Índice de ESTE río: 0 Mantaro, 1 Rímac, 2 Nanay.")]
        public int riverId = 0;
        [Tooltip("Nombre de la escena siguiente (debe estar en Build Profiles > Scene List).")]
        public string nextScene = "Rimac";

        bool unlocked;

        void Reset() => GetComponent<Collider>().isTrigger = true;

        void OnEnable() => GameEvents.OnRiverUnlocked += OnUnlocked;
        void OnDisable() => GameEvents.OnRiverUnlocked -= OnUnlocked;

        void OnUnlocked(int unlockedRiver)
        {
            if (unlockedRiver == riverId + 1) unlocked = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<RaftController>() == null) return;

            if (!unlocked)
            {
                GameEvents.Message("Limpia el río al 80 % para pasar al siguiente.");
                return;
            }

            PlayerPrefs.SetInt("rios_unlocked", Mathf.Max(PlayerPrefs.GetInt("rios_unlocked", 0), riverId + 1));
            PlayerPrefs.Save();
            SceneManager.LoadScene(nextScene);
        }
    }
}
