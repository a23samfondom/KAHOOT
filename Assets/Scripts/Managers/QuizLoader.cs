using UnityEngine;
using System.Collections.Generic;
public class QuizLoader : MonoBehaviour
{
    private List<QuizData> quizzes = new List<QuizData>();

    public IReadOnlyList<QuizData> Quizzes { get { return quizzes; } }

    private void Awake()
    {
        CargarKahoots();
    }

    private void CargarKahoots()
    {
        // Buscar todos los archivos JSON de la carpeta Kahoots.
        TextAsset[] archivos = Resources.LoadAll<TextAsset>("Kahoots");

        // Recorrer todos los archivos encontrados.
        foreach (TextAsset archivo in archivos)
        {
            // Convertir el JSON en un objeto QuizData.
            QuizData quiz = JsonUtility.FromJson<QuizData>(archivo.text);

            // Guardar el Kahoot en nuestra lista.
            quizzes.Add(quiz);

            Debug.Log("Kahoot cargado: " + quiz.title);
        }

        Debug.Log("Total de Kahoots cargados: " + quizzes.Count);
    }
}
