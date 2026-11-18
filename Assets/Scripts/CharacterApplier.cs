using UnityEngine;

namespace RiosVivos
{
    /// <summary>Va en la balsa. Muestra el modelo de Rumi o Sisa según lo elegido en el menú.
    /// Los dos juegan igual: solo cambia el modelo.</summary>
    public class CharacterApplier : MonoBehaviour
    {
        [SerializeField] GameObject rumiModel;
        [SerializeField] GameObject sisaModel;

        void Start()
        {
            int index = PlayerPrefs.GetInt(CharacterSelect.Key, 0);
            if (rumiModel != null) rumiModel.SetActive(index == 0);
            if (sisaModel != null) sisaModel.SetActive(index == 1);
        }
    }
}
