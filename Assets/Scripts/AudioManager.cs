using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Música por capas (gana capas cuando el río se limpia) + efectos de sonido.
    /// Pon 1 AudioSource por capa en "musicLayers" (capa 0 = base) y define a qué % de calidad entra cada una.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [Header("Música por capas")]
        [SerializeField] AudioSource[] musicLayers;
        [SerializeField] float[] layerThresholds = { 0f, 30f, 60f };
        [SerializeField] float fadeSpeed = 0.5f;

        [Header("Efectos")]
        [SerializeField] AudioSource sfxSource;
        [SerializeField] AudioClip collectClip;
        [SerializeField] AudioClip hitClip;
        [SerializeField] AudioClip coinClip;

        float quality;
        int lastDurability = -1;
        int lastCoins = -1;

        void OnEnable()
        {
            GameEvents.OnWaterQualityChanged += OnQuality;
            GameEvents.OnTrashCollected += OnTrash;
            GameEvents.OnDurabilityChanged += OnDurability;
            GameEvents.OnCoinsChanged += OnCoins;
        }

        void OnDisable()
        {
            GameEvents.OnWaterQualityChanged -= OnQuality;
            GameEvents.OnTrashCollected -= OnTrash;
            GameEvents.OnDurabilityChanged -= OnDurability;
            GameEvents.OnCoinsChanged -= OnCoins;
        }

        void Start()
        {
            for (int i = 0; i < musicLayers.Length; i++)
            {
                if (musicLayers[i] == null) continue;
                musicLayers[i].loop = true;
                musicLayers[i].volume = Threshold(i) <= 0f ? 1f : 0f;
                musicLayers[i].Play();
            }
        }

        void Update()
        {
            for (int i = 0; i < musicLayers.Length; i++)
            {
                if (musicLayers[i] == null) continue;
                float target = quality >= Threshold(i) ? 1f : 0f;
                musicLayers[i].volume = Mathf.MoveTowards(musicLayers[i].volume, target, fadeSpeed * Time.deltaTime);
            }
        }

        float Threshold(int i) => i < layerThresholds.Length ? layerThresholds[i] : 0f;

        void OnQuality(int riverId, float q) => quality = q;
        void OnTrash(TrashType t) => Play(collectClip);

        void OnDurability(int cur, int max)
        {
            if (lastDurability >= 0 && cur < lastDurability) Play(hitClip);
            lastDurability = cur;
        }

        void OnCoins(int coins)
        {
            if (lastCoins >= 0 && coins > lastCoins) Play(coinClip);
            lastCoins = coins;
        }

        void Play(AudioClip clip)
        {
            if (sfxSource != null && clip != null) sfxSource.PlayOneShot(clip);
        }
    }
}
