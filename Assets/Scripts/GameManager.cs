using UnityEngine;

namespace RiosVivos
{
    /// <summary>
    /// Guarda el último punto de acopio y reinicia la balsa ahí cuando se destruye.
    /// (Derrota = volver al último acopio, se pierde la basura que llevabas. No hay game over.)
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] Rigidbody raft;
        [SerializeField] Transform startPoint;

        Vector3 respawnPos;
        Quaternion respawnRot;

        void Awake()
        {
            Instance = this;
            if (startPoint != null) { respawnPos = startPoint.position; respawnRot = startPoint.rotation; }
            else if (raft != null) { respawnPos = raft.position; respawnRot = raft.rotation; }
        }

        void OnEnable()
        {
            GameEvents.OnCheckpointSet += SetCheckpoint;
            GameEvents.OnRaftDestroyed += RespawnRaft;
        }

        void OnDisable()
        {
            GameEvents.OnCheckpointSet -= SetCheckpoint;
            GameEvents.OnRaftDestroyed -= RespawnRaft;
        }

        public void SetCheckpoint(Transform t)
        {
            respawnPos = t.position;
            respawnRot = t.rotation;
        }

        public void RespawnRaft()
        {
            if (raft == null) return;
            raft.linearVelocity = Vector3.zero;
            raft.angularVelocity = Vector3.zero;
            raft.transform.SetPositionAndRotation(respawnPos, respawnRot);
            GameEvents.Message("La balsa se rompió. Volviste al último punto de acopio.");
            GameEvents.RaftRespawned();
        }
    }
}
