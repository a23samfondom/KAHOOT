using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private void Start()
    {
        quizListPanel.SetActive(true);
        leaderboardPanel.SetActive(false);

        CrearListaKahoots();
    }

    private void CrearListaKahoots()
    {
        string[] archivos = leaderboardManager.ObtenerKahootsConResultados();

        foreach (string ruta in archivos)
        {
            string nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(ruta);

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

            itemUI.Configurar(
                titulo,
                () => AbrirLeaderboard(titulo)
            );
        }
    }

    private void AbrirLeaderboard(string nombreKahoot)
    {
        foreach (Transform hijo in leaderboardContent)
        {
            Destroy(hijo.gameObject);
        }

        quizListPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        leaderboardTitleText.text = nombreKahoot;

        LeaderboardData leaderboard = leaderboardManager.CargarLeaderboard(nombreKahoot);

        leaderboard.results.Sort((a, b) =>
        {
            int comparacionPuntos = b.score.CompareTo(a.score);

            if (comparacionPuntos == 0)
            {
                return a.time.CompareTo(b.time);
            }

            return comparacionPuntos;
        });

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

    public void VolverAListaKahoots()
    {
        leaderboardPanel.SetActive(false);
        quizListPanel.SetActive(true);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }

}