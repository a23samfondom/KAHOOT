using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class AnswerSystem : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private FeedbackUI feedbackUI;
    [SerializeField] private QuestionTimer questionTimer;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private int puntuacionObtenida;

    [Header("Botones de respuesta")]
    [SerializeField] private Button[] botonesRespuesta;

    [Header("Respuestas múltiples")]
    [SerializeField] private Toggle[] togglesRespuesta;

    [Header("Respuestas Slider")]
    [SerializeField] private Slider respuestaSlider;
    [SerializeField] private TMP_Text valueAnswerText;
    [SerializeField] private TMP_Text minimumValueText;
    [SerializeField] private TMP_Text maximumValueText;



    private bool respuestaEnviada;

    public void ComprobarRespuesta(int indiceRespuesta)
    {
        if (respuestaEnviada) return;

        respuestaEnviada = true;

        questionTimer.StopTimer();

        foreach (Button boton in botonesRespuesta)
        {
            boton.interactable = false;
        }

        Debug.Log("Se ha pulsado una respuesta. Índice: " + indiceRespuesta);

        QuestionData pregunta = gameManager.CurrentQuestion;

        if (pregunta.correctAnswers != null && pregunta.correctAnswers.Count > 0)
        {
            int indiceCorrecto = pregunta.correctAnswers[0];

            if (indiceCorrecto >= 0 && indiceCorrecto < pregunta.answers.Count)
            {
                string respuestaCorrecta = pregunta.answers[indiceCorrecto];
                feedbackUI.MostrarRespuestaCorrecta(respuestaCorrecta);
            }
        }

        bool esCorrecta = pregunta.correctAnswers.Contains(indiceRespuesta);


        if (esCorrecta)
        {

            puntuacionObtenida = scoreSystem.CalcularPuntuacion(
                questionTimer.TimeRemaining,
                pregunta.duration,
                pregunta.scoreMultiplier
            );

            scoreSystem.AñadirPuntos(puntuacionObtenida);

            feedbackUI.MostrarFeedback(FeedbackType.Correct, puntuacionObtenida);
        }
        else
        {
            puntuacionObtenida = 0;

            feedbackUI.MostrarFeedback(FeedbackType.Incorrect, puntuacionObtenida);
        }
    }

    public void ReiniciarRespuesta()
    {
        respuestaEnviada = false;

        foreach (Button boton in botonesRespuesta)
        {
            boton.interactable = true;
        }

        foreach (Toggle toggle in togglesRespuesta)
        {
            toggle.isOn = false;
            toggle.interactable = true;
        }

        respuestaSlider.interactable = true;

        QuestionData pregunta = gameManager.CurrentQuestion;

        respuestaSlider.minValue = pregunta.minimumValue;
        respuestaSlider.maxValue = pregunta.maximumValue;
        respuestaSlider.value = pregunta.minimumValue;
        minimumValueText.text = pregunta.minimumValue.ToString("0");
        maximumValueText.text = pregunta.maximumValue.ToString("0");
        respuestaSlider.wholeNumbers = true;

        ActualizarValorSlider(respuestaSlider.value);
    }

    public void TiempoAgotado()
    {
        if (respuestaEnviada)
            return;

        respuestaEnviada = true;

        QuestionData pregunta = gameManager.CurrentQuestion;

        if (pregunta.correctAnswers != null &&
            pregunta.correctAnswers.Count > 0 &&
            pregunta.answers != null)
        {
            int indiceCorrecto = pregunta.correctAnswers[0];

            if (indiceCorrecto >= 0 && indiceCorrecto < pregunta.answers.Count)
            {
                string respuestaCorrecta = pregunta.answers[indiceCorrecto];
                feedbackUI.MostrarRespuestaCorrecta(respuestaCorrecta);
            }
        }


        foreach (Button boton in botonesRespuesta)
        {
            boton.interactable = false;
        }

        puntuacionObtenida = 0;

        scoreSystem.AñadirPuntos(puntuacionObtenida);

        feedbackUI.MostrarFeedback(FeedbackType.Timeout, puntuacionObtenida);
    }

    private void OnEnable()
    {
        questionTimer.OnTimerFinished += TiempoAgotado;
    }

    private void OnDisable()
    {
        questionTimer.OnTimerFinished -= TiempoAgotado;
    }

    public void ComprobarRespuestasMultiples()
    {
        if (respuestaEnviada)
            return;

        respuestaEnviada = true;

        questionTimer.StopTimer();

        List<int> respuestasSeleccionadas = new List<int>();

        // Recogemos los Toggle seleccionados
        for (int i = 0; i < togglesRespuesta.Length; i++)
        {
            if (togglesRespuesta[i].isOn)
            {
                respuestasSeleccionadas.Add(i);
            }

            togglesRespuesta[i].interactable = false;
        }

        QuestionData pregunta = gameManager.CurrentQuestion;

        // Comprobamos que coincidan todas las respuestas
        bool esCorrecta = respuestasSeleccionadas.Count == pregunta.correctAnswers.Count;

        foreach (int respuesta in pregunta.correctAnswers)
        {
            if (!respuestasSeleccionadas.Contains(respuesta))
            {
                esCorrecta = false;
                break;
            }
        }

        // Obtenemos los textos de las respuestas correctas
        List<string> textosCorrectos = new List<string>();

        foreach (int indice in pregunta.correctAnswers)
        {
            textosCorrectos.Add(pregunta.answers[indice]);
        }

        feedbackUI.MostrarRespuestaCorrecta(string.Join(", ", textosCorrectos));

        if (esCorrecta)
        {
            puntuacionObtenida = scoreSystem.CalcularPuntuacion(
                questionTimer.TimeRemaining,
                pregunta.duration,
                pregunta.scoreMultiplier
            );

            scoreSystem.AñadirPuntos(puntuacionObtenida);

            feedbackUI.MostrarFeedback(FeedbackType.Correct, puntuacionObtenida);
        }
        else
        {
            puntuacionObtenida = 0;

            feedbackUI.MostrarFeedback(FeedbackType.Incorrect, puntuacionObtenida);
        }
    }

    public void ActualizarValorSlider(float valor)
    {
        valueAnswerText.text = Mathf.RoundToInt(valor).ToString();
    }

    public void ComprobarRespuestaSlider()
    {
        if (respuestaEnviada)
            return;

        respuestaEnviada = true;

        questionTimer.StopTimer();

        respuestaSlider.interactable = false;

        QuestionData pregunta = gameManager.CurrentQuestion;

        float valorSeleccionado = respuestaSlider.value;

        bool esCorrecta = Mathf.Approximately(
            valorSeleccionado,
            pregunta.correctValue
        );

        feedbackUI.MostrarRespuestaCorrecta(
            Mathf.RoundToInt(pregunta.correctValue).ToString()
        );

        if (esCorrecta)
        {
            puntuacionObtenida = scoreSystem.CalcularPuntuacion(
                questionTimer.TimeRemaining,
                pregunta.duration,
                pregunta.scoreMultiplier
            );

            scoreSystem.AñadirPuntos(puntuacionObtenida);

            feedbackUI.MostrarFeedback(FeedbackType.Correct, puntuacionObtenida);
        }
        else
        {
            puntuacionObtenida = 0;

            feedbackUI.MostrarFeedback(FeedbackType.Incorrect, puntuacionObtenida);
        }
    }
}