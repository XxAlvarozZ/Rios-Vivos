using UnityEngine;
using UnityEngine.SceneManagement;

namespace RiosVivos
{
    /// <summary>Pausa con Esc. Arrastra el panel de pausa a "panel" y conecta los botones a Resume / GoToMenu.</summary>
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        [SerializeField] string menuScene = "MainMenu";

        bool paused;

        void Start()
        {
            if (panel != null) panel.SetActive(false);
        }

        void Update()
        {
            if (InputUtil.PausePressed()) Toggle();
        }

        public void Toggle()
        {
            paused = !paused;
            if (panel != null) panel.SetActive(paused);
            Time.timeScale = paused ? 0f : 1f;
        }

        public void Resume()
        {
            if (paused) Toggle();
        }

        public void GoToMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(menuScene);
        }

        void OnDisable() => Time.timeScale = 1f;
    }
}
