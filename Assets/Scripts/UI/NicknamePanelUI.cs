using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NicknamePanelUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_InputField nicknameInput;
    [SerializeField] private QuizSelector quizSelector;

    public void IniciarPartida()
    {
        string nombre = nicknameInput.text.Trim();

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.LogWarning("Debes introducir un nombre.");
            return;
        }

        if (quizSelector == null || quizSelector.SelectedQuiz == null)
        {
            Debug.LogError("No hay ningún Kahoot seleccionado.");
            return;
        }

        GameSession.PlayerName = nombre;
        GameSession.SelectedQuiz = quizSelector.SelectedQuiz;

        Debug.Log("Jugador: " + GameSession.PlayerName);
        Debug.Log("Kahoot: " + GameSession.SelectedQuiz.title);

        SceneManager.LoadScene("GameMode");
    }

    public void VolverAlSelector()
    {
        SceneManager.LoadScene("SelectorMenu");
    }

    public void VolverAlMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}