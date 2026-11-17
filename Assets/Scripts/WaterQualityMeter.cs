using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Medidor de calidad del agua (0-100 %) de un río. Un objeto por escena/río.
    /// Al llegar al umbral (80 %) desbloquea el siguiente río. El agua cambia de marrón a turquesa.
    /// </summary>
    public class WaterQualityMeter : MonoBehaviour
    {
        public static WaterQualityMeter Instance { get; private set; }

        [Header("Río")]
        public int riverId = 0;                 // 0 Mantaro, 1 Rímac, 2 Nanay
        [Tooltip("0 = cuenta automática la basura que hay en la escena al empezar.")]
        public int totalTrashInRiver = 0;
        public float unlockThreshold = 80f;

        [Header("Agua")]
        [SerializeField] Renderer waterRenderer;
        public Color dirtyColor = new Color(0.42f, 0.30f, 0.18f, 0.85f);
        public Color cleanColor = new Color(0.10f, 0.75f, 0.80f, 0.85f);

        float quality, shown;
        bool unlocked;
        Material mat;

        void Awake()
        {
            Instance = this;
            if (waterRenderer != null) mat = waterRenderer.material;
        }

        void Start()
        {
            if (totalTrashInRiver <= 0) totalTrashInRiver = Mathf.Max(1, TrashItem.Active.Count);
            GameEvents.WaterQualityChanged(riverId, quality);
            ApplyColor();
        }

        /// <summary>Lo llama DockPoint al descargar basura.</summary>
        public void AddCleaned(int items)
        {
            quality = Mathf.Clamp(quality + items * 100f / totalTrashInRiver, 0f, 100f);
            GameEvents.WaterQualityChanged(riverId, quality);

            if (!unlocked && quality >= unlockThreshold)
            {
                unlocked = true;
                GameEvents.RiverUnlocked(riverId + 1);
                GameEvents.Message("¡Río limpio! Se desbloqueó el siguiente río.");
            }
        }

        void Update()
        {
            shown = Mathf.MoveTowards(shown, quality, 20f * Time.deltaTime);
            ApplyColor();
        }

        void ApplyColor()
        {
            if (mat == null) return;
            Color c = Color.Lerp(dirtyColor, cleanColor, shown / 100f);
            mat.SetColor(mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color", c);
        }
    }
}
