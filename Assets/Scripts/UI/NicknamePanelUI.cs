using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NicknamePanelUI : MonoBehaviour
{
    [Header("Referencias")] [SerializeField] private TMP_InputField nicknameInput;

    public void IniciarPartida()
    {
        // Obtener el nombre introducido.
        string nombre = nicknameInput.text.Trim();

        // Comprobar que el nombre no esté vacío.
        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Debes introducir un nombre.");
            return;
        }

        // Mostrar el nombre en consola.
        Debug.Log("Jugador: " + nombre);
    }

    public void VolverAlSelector()
    {
        SceneManager.LoadScene("SelectorMenu");
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }
}
