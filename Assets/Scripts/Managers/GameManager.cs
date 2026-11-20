using UnityEngine;
public class GameManager : MonoBehaviour
{
    private QuizData currentQuiz;
    private int currentQuestionIndex = 0;

    public QuestionData CurrentQuestion
    {
        get { return currentQuiz.questions[currentQuestionIndex]; }
    }

    public int CurrentQuestionIndex
    {
        get { return currentQuestionIndex; }
    }

    private void Awake()
    {
        currentQuiz = GameSession.SelectedQuiz;

        if (currentQuiz == null || currentQuiz.questions == null || currentQuiz.questions.Count == 0)
        {
            Debug.LogError("El Kahoot no tiene preguntas disponibles.");
            return;
        }

        Debug.Log("Partida iniciada: " + currentQuiz.title);
    }

    public void SiguientePregunta()
    {
        currentQuestionIndex++;

        if (currentQuestionIndex >= currentQuiz.questions.Count)
        {
            Debug.Log("¡Kahoot terminado!");

            FindFirstObjectByType<LeaderboardUI>().MostrarClasificacionFinal();
            return;
        }

        Debug.Log("Pregunta actual: " + (currentQuestionIndex + 1));

        FindFirstObjectByType<QuestionUI>().MostrarPregunta();
    }
}
