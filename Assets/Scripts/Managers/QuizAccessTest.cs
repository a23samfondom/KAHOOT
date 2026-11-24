using UnityEngine;

// Comprueba el acceso a los cuestionarios cargados.
public class QuizAccessTest : MonoBehaviour
{
    [SerializeField] private QuizLoader quizLoader;
    [SerializeField] private QuizSelector quizSelector;

    private void Start()
    {
        // Comprobar que existe el cargador.
        if (quizLoader == null) { Debug.LogError("No se ha asignado el QuizLoader."); return; }

        Debug.Log("Total de Kahoots: " + quizLoader.Quizzes.Count);

        // Seleccionar un cuestionario de prueba.
        quizSelector.SeleccionarQuiz(1);

        // Mostrar los cuestionarios disponibles en la consola.
        foreach (QuizData quiz in quizLoader.Quizzes)
        {
            Debug.Log("Kahoot disponible: " + quiz.title);
        }
    }
}