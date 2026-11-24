using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Gestiona la introducción del nombre antes de empezar una partida.
public class NicknamePanelUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private QuizSelector quizSelector;

    // Comprueba los datos e inicia la partida.
    public void IniciarPartida()
    {
        // Obtener el nombre sin espacios al principio o al final.
        string nombre = nicknameInput.text.Trim();

        // Comprobar que se ha introducido un nombre.
        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Debes introducir un nombre.");
            return;
        }

        // Comprobar que hay un Kahoot seleccionado.
        if (quizSelector == null || quizSelector.SelectedQuiz == null)
        {
            Debug.LogError("No hay ningún Kahoot seleccionado.");
            return;
        }

        // Guardar los datos para utilizarlos durante la partida.
        GameSession.PlayerName = nombre;
        GameSession.SelectedQuiz = quizSelector.SelectedQuiz;

        Debug.Log("Jugador: " + GameSession.PlayerName);
        Debug.Log("Kahoot: " + GameSession.SelectedQuiz.title);

        // Cargar la escena de juego.
        SceneManager.LoadScene("GameMode");
    }

    // Volver a la selección de Kahoots.
    public void VolverAlSelector()
    {
        SceneManager.LoadScene("SelectorMenu");
    }

    // Volver al menú de modos.
    public void VolverAlMainMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }
}