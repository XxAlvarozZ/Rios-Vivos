using UnityEngine;
using UnityEngine.SceneManagement;

namespace RiosVivos
{
    /// <summary>Menú principal. Conecta cada botón (On Click) a Play / Quit.
    /// Las escenas deben estar en File > Build Profiles > Scene List.</summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] string characterSelectScene = "CharacterSelect";

        public void Play()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(characterSelectScene);
        }

        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
