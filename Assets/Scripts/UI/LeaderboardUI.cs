using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Gestiona la clasificación durante y al finalizar una partida.
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

    // Actualiza la clasificación con los datos del jugador.
    public void ActualizarLeaderboard()
    {
        // Limpiamos los elementos anteriores.
        foreach (Transform hijo in content)
        {
            Destroy(hijo.gameObject);
        }

        // Creamos el elemento del jugador.
        GameObject nuevoItem = Instantiate(leaderboardItemPrefab, content);

        // Obtenemos su componente.
        LeaderboardItemUI itemUI = nuevoItem.GetComponent<LeaderboardItemUI>();

        if (itemUI == null)
        {
            Debug.LogError("El prefab no tiene LeaderboardItemUI.");
            return;
        }

        // Configuramos los datos del jugador.
        itemUI.Configurar(
            1,
            GameSession.PlayerName,
            scoreSystem.TotalScore
        );
    }

    // Avanza a la siguiente pregunta o muestra el resultado final.
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

    // Muestra y guarda la clasificación final de la partida.
    public void MostrarClasificacionFinal()
    {
        leaderboardPanel.SetActive(true);

        ActualizarLeaderboard();

        // Evitar guardar el mismo resultado más de una vez.
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

        // Cambiar el título y el botón para finalizar la partida.
        titleText.text = "CLASIFICACIÓN FINAL";
        nextButtonText.text = "SALIR";

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(VolverAlMenu);
    }

    // Regresar al menú de modos.
    public void VolverAlMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }
}