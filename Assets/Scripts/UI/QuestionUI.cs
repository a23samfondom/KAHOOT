using TMPro;
using UnityEngine;
public class QuestionUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameManager gameManager;

    [Header("Información del Kahoot")]
    //[SerializeField] private TMP_Text quizTitleText;
    [SerializeField] private TMP_Text questionNumberText;
    [SerializeField] private TMP_Text questionText;

    [Header("Paneles de respuestas")]
    [SerializeField] private GameObject normalPanel;
    [SerializeField] private GameObject multipleCorrectPanel;
    [SerializeField] private GameObject trueFalsePanel;
    [SerializeField] private GameObject sliderPanel;

    [Header("Información de la pregunta")]
    [SerializeField] private TMP_Text questionTypeText;
    [SerializeField] private TMP_Text scoreMultiplierText;
    [SerializeField] private TMP_Text durationText;

    [Header("Temporizador")]
    [SerializeField] private QuestionTimer questionTimer;

    private void Start()
    {
        MostrarPregunta();
    }

    public void MostrarPregunta()
    {
        if (GameSession.SelectedQuiz == null)
        {
            Debug.LogError("No hay ningún Kahoot seleccionado.");
            return;
        }

        QuizData quiz = GameSession.SelectedQuiz;
        QuestionData pregunta = gameManager.CurrentQuestion;

        MostrarPanelRespuesta(pregunta.questionType);
        MostrarInformacionPregunta(pregunta);
        questionTimer.StartTimer(pregunta.duration);

        //quizTitleText.text = quiz.title;

        questionNumberText.text = (gameManager.CurrentQuestionIndex + 1) + " / " + quiz.questions.Count;

        questionText.text = pregunta.questionText;
    }

    private void MostrarPanelRespuesta(QuestionType tipo)
    {
        normalPanel.SetActive(tipo == QuestionType.Normal);
        multipleCorrectPanel.SetActive(tipo == QuestionType.MultipleCorrect);
        trueFalsePanel.SetActive(tipo == QuestionType.TrueFalse);
        sliderPanel.SetActive(tipo == QuestionType.Slider);
    }

    private void MostrarInformacionPregunta(QuestionData pregunta)
    {
        // Mostrar el tipo de pregunta
        switch (pregunta.questionType)
        {
            case QuestionType.Normal:
                questionTypeText.text = "RESPUESTA ÚNICA";
                break;

            case QuestionType.MultipleCorrect:
                questionTypeText.text = "MÚLTIPLES";
                break;

            case QuestionType.TrueFalse:
                questionTypeText.text = "TRUE / FALSE";
                break;

            case QuestionType.Slider:
                questionTypeText.text = "SLIDER";
                break;
        }

        // Mostrar el multiplicador
        if (pregunta.scoreMultiplier == ScoreType.Double)
            scoreMultiplierText.text = "x2 SCORE";
        else
            scoreMultiplierText.text = "x1 SCORE";

        // Mostrar la duración
        durationText.text = pregunta.duration + " SEGUNDOS";
    }
}
