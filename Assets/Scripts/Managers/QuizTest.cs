using UnityEngine;
public class QuizTest : MonoBehaviour
{
    private void Start()
    {
        // Buscar el archivo JSON dentro de Resources.
        TextAsset jsonFile = Resources.Load<TextAsset>("Kahoots/CulturaGeneral");

        // Comprobar que el archivo existe.
        if (jsonFile == null)
        {
            Debug.LogError("No se ha encontrado el archivo JSON.");
            return;
        }

        // Convertir el JSON en un objeto QuizData.
        QuizData quiz = JsonUtility.FromJson<QuizData>(jsonFile.text);

        // Mostrar los datos en la consola.
        Debug.Log("Kahoot: " + quiz.title);
        Debug.Log("Número de preguntas: " + quiz.questions.Count);

        // Mostrar cada pregunta.
        foreach (QuestionData question in quiz.questions)
        {
            Debug.Log("Pregunta: " + question.questionText);
            Debug.Log("Tipo: " + question.questionType);
            Debug.Log("Duración: " + question.duration);
            Debug.Log("Puntuación: " + question.scoreMultiplier);
        }
    }
}
