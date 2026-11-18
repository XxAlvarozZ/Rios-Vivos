using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RiosVivos
{
    /// <summary>
    /// HUD: capacidad, resistencia, calidad del agua, monedas y mensajes.
    /// Solo escucha GameEvents; no necesita referencias a otros scripts.
    /// Los campos que no uses pueden quedar vacíos.
    /// </summary>
    public class HudController : MonoBehaviour
    {
        [Header("Barras (Slider)")]
        [SerializeField] Slider capacitySlider;
        [SerializeField] Slider durabilitySlider;
        [SerializeField] Slider qualitySlider;

        [Header("Textos (TextMeshPro)")]
        [SerializeField] TMP_Text capacityText;
        [SerializeField] TMP_Text qualityText;
        [SerializeField] TMP_Text coinsText;
        [SerializeField] TMP_Text messageText;
        [SerializeField] float messageSeconds = 3f;

        float hideAt;

        void OnEnable()
        {
            GameEvents.OnCapacityChanged += OnCapacity;
            GameEvents.OnDurabilityChanged += OnDurability;
            GameEvents.OnWaterQualityChanged += OnQuality;
            GameEvents.OnCoinsChanged += OnCoins;
            GameEvents.OnMessage += OnMessage;
        }

        void OnDisable()
        {
            GameEvents.OnCapacityChanged -= OnCapacity;
            GameEvents.OnDurabilityChanged -= OnDurability;
            GameEvents.OnWaterQualityChanged -= OnQuality;
            GameEvents.OnCoinsChanged -= OnCoins;
            GameEvents.OnMessage -= OnMessage;
        }

        void Start()
        {
            if (messageText != null) messageText.gameObject.SetActive(false);
        }

        void Update()
        {
            if (messageText != null && messageText.gameObject.activeSelf && Time.unscaledTime > hideAt)
                messageText.gameObject.SetActive(false);
        }

        void OnCapacity(int cur, int max)
        {
            if (capacitySlider != null) { capacitySlider.maxValue = max; capacitySlider.value = cur; }
            if (capacityText != null) capacityText.text = $"{cur}/{max}";
        }

        void OnDurability(int cur, int max)
        {
            if (durabilitySlider != null) { durabilitySlider.maxValue = max; durabilitySlider.value = cur; }
        }

        void OnQuality(int riverId, float q)
        {
            if (qualitySlider != null) { qualitySlider.maxValue = 100f; qualitySlider.value = q; }
            if (qualityText != null) qualityText.text = $"{Mathf.RoundToInt(q)} %";
        }

        void OnCoins(int coins)
        {
            if (coinsText != null) coinsText.text = coins.ToString();
        }

        void OnMessage(string text)
        {
            if (messageText == null) return;
            messageText.text = text;
            messageText.gameObject.SetActive(true);
            hideAt = Time.unscaledTime + messageSeconds;
        }
    }
}
