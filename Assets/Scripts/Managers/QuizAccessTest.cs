using UnityEngine;

public class QuizAccessTest : MonoBehaviour
{
    [SerializeField] private QuizLoader quizLoader;
    [SerializeField] private QuizSelector quizSelector;

    private void Start()
    {
        if (quizLoader == null) { Debug.LogError("No se ha asignado el QuizLoader."); return; }

        Debug.Log("Total de Kahoots: " + quizLoader.Quizzes.Count);

        quizSelector.SeleccionarQuiz(1);

        foreach (QuizData quiz in quizLoader.Quizzes)
        {
            Debug.Log("Kahoot disponible: " + quiz.title);
        }
    }
}
