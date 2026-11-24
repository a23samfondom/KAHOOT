using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// Gestiona el guardado y la lectura de las clasificaciones.
public class LeaderboardManager : MonoBehaviour
{
    private string leaderboardFolder;

    private void Awake()
    {
        // Crear la ruta donde se guardarán los XML.
        leaderboardFolder = Path.Combine(
            Application.persistentDataPath,
            "Leaderboards"
        );

        // Crear la carpeta si todavía no existe.
        if (!Directory.Exists(leaderboardFolder))
        {
            Directory.CreateDirectory(leaderboardFolder);
        }
    }

    // Guarda un resultado en el XML del Kahoot correspondiente.
    public void GuardarResultado(string nombreKahoot, QuizResult resultado)
    {
        string nombreArchivo = nombreKahoot.Replace(" ", "");
        string ruta = Path.Combine(leaderboardFolder, nombreArchivo + ".xml");

        LeaderboardData leaderboard = new LeaderboardData();

        try
        {
            // Si ya existe un XML, recuperamos sus resultados.
            if (File.Exists(ruta))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));

                using (FileStream archivo = new FileStream(ruta, FileMode.Open))
                {
                    leaderboard = (LeaderboardData)serializer.Deserialize(archivo);
                }
            }

            // Añadimos el nuevo resultado.
            leaderboard.results.Add(resultado);

            // Guardamos todos los resultados en el XML.
            XmlSerializer guardarSerializer = new XmlSerializer(typeof(LeaderboardData));

            using (FileStream archivo = new FileStream(ruta, FileMode.Create))
            {
                guardarSerializer.Serialize(archivo, leaderboard);
            }

            Debug.Log("Resultado guardado en: " + ruta);
        }

        catch (System.Exception excepcion)
        {
            // Generar un informe si ocurre un error al guardar.
            string mensajeError = "Error al guardar el resultado del Kahoot: " + nombreKahoot
                + "\nFecha: " + System.DateTime.Now
                + "\nDetalles: " + excepcion.ToString();

            Debug.LogError(mensajeError);

            GenerarInformeError(nombreKahoot, mensajeError);
        }
    }

    // Carga la clasificación de un Kahoot.
    public LeaderboardData CargarLeaderboard(string nombreKahoot)
    {
        string nombreArchivo = nombreKahoot.Replace(" ", "");
        string ruta = Path.Combine(leaderboardFolder, nombreArchivo + ".xml");

        // Devolver una clasificación vacía si no existe el archivo.
        if (!File.Exists(ruta))
        {
            return new LeaderboardData();
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));

            using (FileStream archivo = new FileStream(ruta, FileMode.Open))
            {
                LeaderboardData leaderboard =
                    (LeaderboardData)serializer.Deserialize(archivo);

                return leaderboard ?? new LeaderboardData();
            }
        }

        catch (System.Exception excepcion)
        {
            // Generar un informe si el XML está dañado.
            string mensajeError = "Error al cargar el XML del Kahoot: " + nombreKahoot
                + "\nFecha: " + System.DateTime.Now
                + "\nDetalles: " + excepcion.ToString();

            Debug.LogError(mensajeError);

            GenerarInformeError(nombreKahoot, mensajeError);

            return new LeaderboardData();
        }
    }

    // Obtener las rutas de los XML de clasificaciones existentes.
    public string[] ObtenerKahootsConResultados()
    {
        string[] archivos = Directory.GetFiles(leaderboardFolder, "*.xml");

        return archivos;
    }

    // Crea un informe de texto con los detalles de un error.
    public void GenerarInformeError(string nombreKahoot, string mensajeError)
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

        // Añadir la fecha y hora al nombre del informe.
        string fecha = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

        string nombreArchivo = "Error_" + nombreKahoot + "_" + fecha + ".txt";
        string ruta = Path.Combine(carpetaInformes, nombreArchivo);

        File.WriteAllText(ruta, mensajeError);

        Debug.Log("Informe de error generado: " + ruta);
    }

    // Genera un informe para comprobar el funcionamiento del sistema.
    public void GenerarErrorPrueba()
    {
        GenerarInformeError(
            "Prueba",
            "Este es un informe de prueba generado desde la aplicación."
        );
    }

    // Devuelve las rutas de los informes de error disponibles.
    public string[] ObtenerInformes()
    {
        string carpetaInformes = Path.Combine(
            Application.persistentDataPath,
            "Reports"
        );

        // Si no existe la carpeta, devolver una lista vacía.
        if (!Directory.Exists(carpetaInformes))
        {
            return new string[0];
        }

        return Directory.GetFiles(carpetaInformes, "*.txt");
    }
}