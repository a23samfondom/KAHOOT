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

    public LeaderboardData CargarLeaderboard(string nombreKahoot)
    {
        string nombreArchivo = nombreKahoot.Replace(" ", "");
        string ruta = Path.Combine(leaderboardFolder, nombreArchivo + ".xml");

        if (!File.Exists(ruta))
        {
            return new LeaderboardData();
        }

        XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));

        using (FileStream archivo = new FileStream(ruta, FileMode.Open))
        {
            return (LeaderboardData)serializer.Deserialize(archivo);
        }
    }

    public string[] ObtenerKahootsConResultados()
    {
        string[] archivos = Directory.GetFiles(leaderboardFolder, "*.xml");

        return archivos;
    }
}