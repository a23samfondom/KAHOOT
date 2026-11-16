using UnityEngine;
public class QuizSelector : MonoBehaviour
{
    [SerializeField] private QuizLoader quizLoader;

    private QuizData selectedQuiz;

    public QuizData SelectedQuiz
    {
        get { return selectedQuiz; }
    }

    private void Awake()
    {
        if (quizLoader == null)
        {
            Debug.LogError("No se ha asignado el QuizLoader.");
        }
    }

    public void SeleccionarQuiz(int indice)
    {
        if (quizLoader == null)
        {
            Debug.LogError("No se puede seleccionar: falta el QuizLoader.");
            return;
        }

        if (indice < 0 || indice >= quizLoader.Quizzes.Count)
        {
            Debug.LogWarning("El índice del Kahoot no es válido.");
            return;
        }

        selectedQuiz = quizLoader.Quizzes[indice];

        Debug.Log("Kahoot seleccionado: " + selectedQuiz.title);
    }
}
