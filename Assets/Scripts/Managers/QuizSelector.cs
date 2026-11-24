using UnityEngine;

// Gestiona la selección del cuestionario.
public class QuizSelector : MonoBehaviour
{
    [SerializeField] private QuizLoader quizLoader;

    private QuizData selectedQuiz;

    // Permite consultar el cuestionario seleccionado.
    public QuizData SelectedQuiz
    {
        get { return selectedQuiz; }
    }

    private void Awake()
    {
        // Comprobar que tiene asignado el cargador.
        if (quizLoader == null)
        {
            Debug.LogError("No se ha asignado el QuizLoader.");
        }
    }

    // Selecciona un cuestionario mediante su posición en la lista.
    public void SeleccionarQuiz(int indice)
    {
        if (quizLoader == null)
        {
            Debug.LogError("No se puede seleccionar: falta el QuizLoader.");
            return;
        }

        // Comprobar que el índice existe.
        if (indice < 0 || indice >= quizLoader.Quizzes.Count)
        {
            Debug.LogWarning("El índice del Kahoot no es válido.");
            return;
        }

        selectedQuiz = quizLoader.Quizzes[indice];

        Debug.Log("Kahoot seleccionado: " + selectedQuiz.title);
    }
}