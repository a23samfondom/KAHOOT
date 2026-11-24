using UnityEngine;

// Controla el desarrollo de la partida.
public class GameManager : MonoBehaviour
{
    private QuizData currentQuiz;
    private int currentQuestionIndex = 0;

    private float startTime;

    // Devuelve el tiempo total transcurrido desde el inicio.
    public float TotalTime
    {
        get { return Time.time - startTime; }
    }

    // Devuelve la pregunta que se está mostrando.
    public QuestionData CurrentQuestion
    {
        get { return currentQuiz.questions[currentQuestionIndex]; }
    }

    // Devuelve el índice de la pregunta actual.
    public int CurrentQuestionIndex
    {
        get { return currentQuestionIndex; }
    }

    private void Awake()
    {
        // Obtener el cuestionario seleccionado.
        currentQuiz = GameSession.SelectedQuiz;

        // Comprobar que el cuestionario contiene preguntas.
        if (currentQuiz == null || currentQuiz.questions == null || currentQuiz.questions.Count == 0)
        {
            Debug.LogError("El Kahoot no tiene preguntas disponibles.");
            return;
        }

        startTime = Time.time;

        Debug.Log("Partida iniciada: " + currentQuiz.title);
    }

    // Avanzar a la siguiente pregunta o terminar la partida.
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