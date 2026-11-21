using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject leaderboardItemPrefab;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private LeaderboardManager leaderboardManager;
    [SerializeField] private bool resultadoGuardado;

    public void ActualizarLeaderboard()
    {
        // Limpiamos los elementos anteriores
        foreach (Transform hijo in content)
        {
            Destroy(hijo.gameObject);
        }

        // Creamos el elemento del jugador
        GameObject nuevoItem = Instantiate(leaderboardItemPrefab, content);

        // Obtenemos su componente
        LeaderboardItemUI itemUI = nuevoItem.GetComponent<LeaderboardItemUI>();

        if (itemUI == null)
        {
            Debug.LogError("El prefab no tiene LeaderboardItemUI.");
            return;
        }

        // Configuramos los datos del jugador
        itemUI.Configurar(
            1,
            GameSession.PlayerName,
            scoreSystem.TotalScore
        );
    }

    public void SiguientePregunta()
    {
        if (gameManager.CurrentQuestionIndex >= GameSession.SelectedQuiz.questions.Count - 1)
        {
            MostrarClasificacionFinal();
            return;
        }

        leaderboardPanel.SetActive(false);
        gameManager.SiguientePregunta();
    }

    public void MostrarClasificacionFinal()
    {
        leaderboardPanel.SetActive(true);

        ActualizarLeaderboard();

        if (!resultadoGuardado)
        {
            QuizResult resultado = new QuizResult();

            resultado.nickname = GameSession.PlayerName;
            resultado.score = scoreSystem.TotalScore;
            resultado.time = gameManager.TotalTime;

            leaderboardManager.GuardarResultado(
                GameSession.SelectedQuiz.title,
                resultado
            );

            resultadoGuardado = true;
        }

        titleText.text = "CLASIFICACIÓN FINAL";
        nextButtonText.text = "SALIR";

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(VolverAlMenu);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}