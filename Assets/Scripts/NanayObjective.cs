using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Objetivo del río Nanay: instalar una barrera de contención o reportar una draga ilegal.
    /// Pon este script en un Empty con un Collider (Is Trigger) alrededor del punto.
    /// La balsa entra a la zona y el jugador MANTIENE la tecla E hasta completar la barra.
    /// Al terminar suma "residuos equivalentes" al medidor de calidad del agua.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class NanayObjective : MonoBehaviour
    {
        public ObjectiveType type = ObjectiveType.Barrier;
        [Tooltip("Segundos que hay que mantener E.")]
        public float holdSeconds = 2f;
        [Tooltip("Cuántos residuos equivalentes suma al medidor (barrera ~3, reporte de draga ~5).")]
        public int bonusItems = 3;

        [Header("Visual")]
        [Tooltip("Se ACTIVAN al completar (barrera instalada, bandera de 'reportada').")]
        public GameObject[] showOnComplete;
        [Tooltip("Se DESACTIVAN al completar (por ejemplo, el aviso del punto marcado).")]
        public GameObject[] hideOnComplete;

        int raftContacts;
        float held;
        bool completed;

        public float Progress => holdSeconds <= 0f ? 1f : Mathf.Clamp01(held / holdSeconds);

        void Reset() => GetComponent<Collider>().isTrigger = true;

        void Start()
        {
            foreach (var go in showOnComplete) if (go != null) go.SetActive(false);
        }

        void OnTriggerEnter(Collider other)
        {
            if (completed || other.GetComponentInParent<RaftController>() == null) return;
            raftContacts++;
            GameEvents.Message(type == ObjectiveType.Barrier
                ? "Mantén E para instalar la barrera de contención."
                : "Mantén E para reportar la draga ilegal.");
        }

        void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<RaftController>() == null) return;
            raftContacts = Mathf.Max(0, raftContacts - 1);
            if (raftContacts == 0) held = 0f;
        }

        void Update()
        {
            if (completed || raftContacts == 0) return;

            if (InputUtil.HookHeld()) held += Time.deltaTime;
            else held = 0f;

            if (held >= holdSeconds) Complete();
        }

        void Complete()
        {
            completed = true;
            foreach (var go in showOnComplete) if (go != null) go.SetActive(true);
            foreach (var go in hideOnComplete) if (go != null) go.SetActive(false);

            GameEvents.BonusCleanup(bonusItems);
            GameEvents.ObjectiveCompleted(type);
            GameEvents.Message(type == ObjectiveType.Barrier
                ? "¡Barrera instalada! Llegará menos contaminación al río."
                : "¡Draga reportada a las autoridades!");
        }
    }
}
