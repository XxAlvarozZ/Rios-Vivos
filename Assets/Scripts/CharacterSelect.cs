using UnityEngine;
using UnityEngine.SceneManagement;

namespace RiosVivos
{
    /// <summary>Pantalla de selección: botón Rumi -> SelectRumi, botón Sisa -> SelectSisa.
    /// Guarda la elección (0 = Rumi, 1 = Sisa) y carga el primer río.</summary>
    public class CharacterSelect : MonoBehaviour
    {
        public const string Key = "character";
        [SerializeField] string firstRiverScene = "Mantaro";

        public void SelectRumi() => Select(0);
        public void SelectSisa() => Select(1);

        void Select(int index)
        {
            PlayerPrefs.SetInt(Key, index);
            PlayerPrefs.Save();
            SceneManager.LoadScene(firstRiverScene);
        }
    }
}
