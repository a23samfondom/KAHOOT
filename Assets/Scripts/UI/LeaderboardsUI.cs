using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Gestiona las clasificaciones independientes de cada Kahoot.
public class LeaderboardsUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private LeaderboardManager leaderboardManager;

    [Header("Paneles")]
    [SerializeField] private GameObject quizListPanel;
    [SerializeField] private GameObject leaderboardPanel;

    [Header("Lista de Kahoots")]
    [SerializeField] private Transform quizListContent;
    [SerializeField] private GameObject quizItemPrefab;

    [Header("Clasificación de jugadores")]
    [SerializeField] private Transform leaderboardContent;
    [SerializeField] private GameObject playerItemPrefab;
    [SerializeField] private TMP_Text leaderboardTitleText;

    // Mostrar la lista de Kahoots al entrar en la escena.
    private void Start()
    {
        quizListPanel.SetActive(true);
        leaderboardPanel.SetActive(false);

        CrearListaKahoots();
    }

    // Crear un elemento por cada Kahoot con resultados guardados.
    private void CrearListaKahoots()
    {
        string[] archivos = leaderboardManager.ObtenerKahootsConResultados();

        foreach (string ruta in archivos)
        {
            string nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(ruta);

            // Adaptar los nombres de archivo a los títulos visibles.
            string titulo = nombreArchivo.Replace("Culturageneral", "Cultura General")
                                         .Replace("Videojuegosclásicos", "Videojuegos Clásicos");

            GameObject nuevoItem = Instantiate(quizItemPrefab, quizListContent);

            QuizLeaderboardItemUI itemUI = nuevoItem.GetComponent<QuizLeaderboardItemUI>();

            if (itemUI == null)
            {
                Debug.LogError("El prefab no tiene QuizLeaderboardItemUI.");
                Destroy(nuevoItem);
                continue;
            }

            // Abrir la clasificación al pulsar el elemento.
            itemUI.Configurar(
                titulo,
                () => AbrirLeaderboard(titulo)
            );
        }
    }

    // Cargar y mostrar los resultados del Kahoot seleccionado.
    private void AbrirLeaderboard(string nombreKahoot)
    {
        // Eliminar los elementos de clasificaciones anteriores.
        foreach (Transform hijo in leaderboardContent)
        {
            Destroy(hijo.gameObject);
        }

        quizListPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        leaderboardTitleText.text = nombreKahoot;

        LeaderboardData leaderboard = leaderboardManager.CargarLeaderboard(nombreKahoot);

        // Ordenar primero por puntuación y después por tiempo.
        leaderboard.results.Sort((a, b) =>
        {
            int comparacionPuntos = b.score.CompareTo(a.score);

            if (comparacionPuntos == 0)
            {
                return a.time.CompareTo(b.time);
            }

            return comparacionPuntos;
        });

        // Crear un elemento visual para cada jugador.
        for (int i = 0; i < leaderboard.results.Count; i++)
        {
            QuizResult resultado = leaderboard.results[i];

            GameObject nuevoItem = Instantiate(playerItemPrefab, leaderboardContent);

            PlayerLeaderboardPrefabItemUI itemUI = nuevoItem.GetComponent<PlayerLeaderboardPrefabItemUI>();

            if (itemUI == null)
            {
                Debug.LogError("El prefab no tiene LeaderboardPlayerItemUI.");
                Destroy(nuevoItem);
                continue;
            }

            itemUI.Configurar(
                i + 1,
                resultado.nickname,
                resultado.score
            );
        }
    }

    // Volver a la lista de cuestionarios.
    public void VolverAListaKahoots()
    {
        leaderboardPanel.SetActive(false);
        quizListPanel.SetActive(true);
    }

    // Volver al menú de modos.
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }

}