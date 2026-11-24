using UnityEngine;
using System.Collections.Generic;
using System.IO;

// Carga los cuestionarios predeterminados y externos.
public class QuizLoader : MonoBehaviour
{
    private List<QuizData> quizzes = new List<QuizData>();

    // Permite consultar los cuestionarios cargados.
    public IReadOnlyList<QuizData> Quizzes { get { return quizzes; } }

    private void Awake()
    {
        CargarKahoots();
    }

    // Carga los cuestionarios incluidos en Resources.
    private void CargarKahoots()
    {
        // Buscar todos los archivos JSON de la carpeta Kahoots.
        TextAsset[] archivos = Resources.LoadAll<TextAsset>("Kahoots");

        // Recorrer todos los Kahoots predeterminados.
        foreach (TextAsset archivo in archivos)
        {
            // Evitar que la plantilla aparezca en el selector.
            if (archivo.name == "Plantilla")
            {
                continue;
            }

            try
            {
                // Convertir el JSON en un objeto QuizData.
                QuizData quiz = JsonUtility.FromJson<QuizData>(archivo.text);

                // Comprobar que el cuestionario contiene datos válidos.
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
                // Generar un informe si falla la carga.
                string nombreKahoot = archivo.name;

                string mensajeError =
                    "Error al cargar el Kahoot predeterminado: " + nombreKahoot
                    + "\nFecha: " + System.DateTime.Now
                    + "\nDetalles: " + excepcion;

                Debug.LogError(mensajeError);

                GenerarInformeError(nombreKahoot, mensajeError);
            }
        }

        // Cargar también los cuestionarios creados por el usuario.
        CargarKahootsExternos();

        Debug.Log("Total de Kahoots cargados: " + quizzes.Count);
    }

    // Busca y carga los cuestionarios guardados en persistentDataPath.
    private void CargarKahootsExternos()
    {
        string carpeta = Path.Combine(
            Application.persistentDataPath,
            "Kahoots"
        );

        // Comprobar si existe la carpeta.
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
                // Leer el contenido del archivo.
                string json = File.ReadAllText(ruta);

                // Convertir el JSON en un objeto QuizData.
                QuizData quiz = JsonUtility.FromJson<QuizData>(json);

                // Comprobar que el cuestionario contiene datos válidos.
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
                // Generar un informe si falla la carga.
                string mensajeError =
                    "Error al cargar el Kahoot externo: " + nombreKahoot
                    + "\nFecha: " + System.DateTime.Now
                    + "\nDetalles: " + excepcion;

                Debug.LogError(mensajeError);

                GenerarInformeError(nombreKahoot, mensajeError);
            }
        }
    }

    // Guarda un informe de texto cuando se produce un error.
    private void GenerarInformeError(string nombreKahoot, string mensajeError)
    {
        string carpetaInformes = Path.Combine(
            Application.persistentDataPath,
            "Reports"
        );

        // Crear la carpeta de informes si no existe.
        if (!Directory.Exists(carpetaInformes))
        {
            Directory.CreateDirectory(carpetaInformes);
        }

        // Añadir la fecha al nombre para identificar cada informe.
        string fecha = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        string nombreArchivo = "Error_" + nombreKahoot + "_" + fecha + ".txt";

        string ruta = Path.Combine(carpetaInformes, nombreArchivo);

        File.WriteAllText(ruta, mensajeError);

        Debug.Log("Informe de error generado: " + ruta);
    }
}