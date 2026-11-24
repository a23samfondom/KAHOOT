using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private string leaderboardFolder;

    private void Awake()
    {
        leaderboardFolder = Path.Combine(
            Application.persistentDataPath,
            "Leaderboards"
        );

        if (!Directory.Exists(leaderboardFolder))
        {
            Directory.CreateDirectory(leaderboardFolder);
        }
    }

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
            string mensajeError = "Error al guardar el resultado del Kahoot: " + nombreKahoot
                + "\nFecha: " + System.DateTime.Now
                + "\nDetalles: " + excepcion.ToString();

            Debug.LogError(mensajeError);

            GenerarInformeError(nombreKahoot, mensajeError);
        }
    }

    public LeaderboardData CargarLeaderboard(string nombreKahoot)
    {
        string nombreArchivo = nombreKahoot.Replace(" ", "");
        string ruta = Path.Combine(leaderboardFolder, nombreArchivo + ".xml");

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
            string mensajeError = "Error al cargar el XML del Kahoot: " + nombreKahoot
                + "\nFecha: " + System.DateTime.Now
                + "\nDetalles: " + excepcion.ToString();

            Debug.LogError(mensajeError);

            GenerarInformeError(nombreKahoot, mensajeError);

            return new LeaderboardData();
        }
    }

    public string[] ObtenerKahootsConResultados()
    {
        string[] archivos = Directory.GetFiles(leaderboardFolder, "*.xml");

        return archivos;
    }

    public void GenerarInformeError(string nombreKahoot, string mensajeError)
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

    public void GenerarErrorPrueba()
    {
        GenerarInformeError(
            "Prueba",
            "Este es un informe de prueba generado desde la aplicación."
        );
    }

    public string[] ObtenerInformes()
    {
        string carpetaInformes = Path.Combine(
            Application.persistentDataPath,
            "Reports"
        );

        if (!Directory.Exists(carpetaInformes))
        {
            return new string[0];
        }

        return Directory.GetFiles(carpetaInformes, "*.txt");
    }
}