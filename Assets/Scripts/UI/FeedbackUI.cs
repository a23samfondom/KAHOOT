using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Muestra los resultados de cada respuesta y gestiona su animación.
public class FeedbackUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CanvasGroup feedbackCanvasGroup;
    [SerializeField] private Image feedbackBackground;
    [SerializeField] private TMP_Text feedbackTitleText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text correctAnswerText;

    [Header("Animación")]
    [SerializeField] private float fadeDuration = 0.3f;

    [Header("Leaderboard")]
    [SerializeField] private LeaderboardUI leaderboardUI;
    [SerializeField] private GameObject leaderboardPanel;

    private float fadeTimer;
    private bool isFading;

    // Controla la aparición gradual del panel.
    private void Update()
    {
        if (!isFading) return;

        fadeTimer += Time.deltaTime;

        float progreso = fadeTimer / fadeDuration;

        feedbackCanvasGroup.alpha = Mathf.Lerp(0f, 1f, progreso);

        if (progreso >= 1f)
        {
            feedbackCanvasGroup.alpha = 1f;
            isFading = false;
        }
    }

    // Activa el panel y comienza su animación.
    public void MostrarFeedback()
    {
        gameObject.SetActive(true);

        feedbackCanvasGroup.alpha = 0f;
        feedbackCanvasGroup.interactable = true;
        feedbackCanvasGroup.blocksRaycasts = true;

        fadeTimer = 0f;
        isFading = true;
    }

    // Muestra el resultado según el tipo de respuesta.
    public void MostrarFeedback(FeedbackType tipo, int puntuacion)
    {
        switch (tipo)
        {
            case FeedbackType.Correct:
                feedbackTitleText.text = "¡CORRECTO!";
                feedbackBackground.color = new Color32(160, 250, 68, 255);
                break;

            case FeedbackType.Incorrect:
                feedbackTitleText.text = "¡INCORRECTO!";
                feedbackBackground.color = new Color32(250, 79, 68, 255);
                break;

            case FeedbackType.Timeout:
                feedbackTitleText.text = "¡TIEMPO AGOTADO!";
                feedbackBackground.color = new Color32(250, 175, 68, 255);
                break;
        }

        scoreText.text = "SCORE: " + "+" + puntuacion;

        MostrarFeedback();
    }

    // Cierra el feedback y muestra la clasificación.
    public void Continuar()
    {

        gameObject.SetActive(false);

        leaderboardUI.ActualizarLeaderboard();

        leaderboardPanel.SetActive(true);

    }

    // Muestra el texto de la respuesta correcta.
    public void MostrarRespuestaCorrecta(string respuesta)
    {
        correctAnswerText.text = "Respuesta correcta: " + respuesta;
    }
}