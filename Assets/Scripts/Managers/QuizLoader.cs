
using UnityEngine;
using System.Collections.Generic;
using System.IO;

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

        // Recorrer todos los Kahoots predeterminados.
        foreach (TextAsset archivo in archivos)
        {
            if (archivo.name == "Plantilla")
            {
                continue;
            }

            try
            {
                QuizData quiz = JsonUtility.FromJson<QuizData>(archivo.text);

                if (quiz == null || string.IsNullOrWhiteSpace(quiz.title)
                    || quiz.questions == null || quiz.questions.Count == 0)
                {
                    throw new System.Exception("El JSON no contiene un Kahoot válido.");
                }

                quizzes.Add(quiz);

                Debug.Log("Kahoot cargado: " + quiz.title);
            }
            catch (System.Exception excepcion)
            {
                string nombreKahoot = archivo.name;

                string mensajeError =
                    "Error al cargar el Kahoot predeterminado: " + nombreKahoot
                    + "\nFecha: " + System.DateTime.Now
                    + "\nDetalles: " + excepcion;

                Debug.LogError(mensajeError);

                GenerarInformeError(nombreKahoot, mensajeError);
            }
        }

        CargarKahootsExternos();

        Debug.Log("Total de Kahoots cargados: " + quizzes.Count);
    }

    private void CargarKahootsExternos()
    {
        string carpeta = Path.Combine(
            Application.persistentDataPath,
            "Kahoots"
        );

        if (!Directory.Exists(carpeta))
        {
            return;
        }

        string[] archivos = Directory.GetFiles(carpeta, "*.json");

        // Recorrer los archivos JSON externos.
        foreach (string ruta in archivos)
        {
            string nombreKahoot = Path.GetFileNameWithoutExtension(ruta);

            try
            {
                string json = File.ReadAllText(ruta);

                QuizData quiz = JsonUtility.FromJson<QuizData>(json);

                if (quiz == null || string.IsNullOrWhiteSpace(quiz.title)
                    || quiz.questions == null || quiz.questions.Count == 0)
                {
                    throw new System.Exception("El JSON no contiene un Kahoot válido.");
                }

                quizzes.Add(quiz);

                Debug.Log("Kahoot externo cargado: " + quiz.title);
            }
            catch (System.Exception excepcion)
            {
                string mensajeError =
                    "Error al cargar el Kahoot externo: " + nombreKahoot
                    + "\nFecha: " + System.DateTime.Now
                    + "\nDetalles: " + excepcion;

                Debug.LogError(mensajeError);

                GenerarInformeError(nombreKahoot, mensajeError);
            }
        }
    }

    private void GenerarInformeError(string nombreKahoot, string mensajeError)
    {
        string carpetaInformes = Path.Combine(
            Application.persistentDataPath,
            "Reports"
        );

        if (!Directory.Exists(carpetaInformes))
        {
            Directory.CreateDirectory(carpetaInformes);
        }

        string fecha = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        string nombreArchivo = "Error_" + nombreKahoot + "_" + fecha + ".txt";

        string ruta = Path.Combine(carpetaInformes, nombreArchivo);

        File.WriteAllText(ruta, mensajeError);

        Debug.Log("Informe de error generado: " + ruta);
    }
}
