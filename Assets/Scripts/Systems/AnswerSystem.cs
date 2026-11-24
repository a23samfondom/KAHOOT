using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Gestiona las respuestas del jugador y comprueba si son correctas.
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

    // Comprueba una respuesta de tipo Normal o TrueFalse.
    public void ComprobarRespuesta(int indiceRespuesta)
    {
        if (respuestaEnviada) return;

        respuestaEnviada = true;

        questionTimer.StopTimer();

        // Desactivar los botones después de responder.
        foreach (Button boton in botonesRespuesta)
        {
            boton.interactable = false;
        }

        Debug.Log("Se ha pulsado una respuesta. Índice: " + indiceRespuesta);

        QuestionData pregunta = gameManager.CurrentQuestion;

        // Mostrar cuál era la respuesta correcta.
        if (pregunta.correctAnswers != null && pregunta.correctAnswers.Count > 0)
        {
            int indiceCorrecto = pregunta.correctAnswers[0];

            if (indiceCorrecto >= 0 && indiceCorrecto < pregunta.answers.Count)
            {
                string respuestaCorrecta = pregunta.answers[indiceCorrecto];
                feedbackUI.MostrarRespuestaCorrecta(respuestaCorrecta);
            }
        }

        // Comprobar si el índice seleccionado es correcto.
        bool esCorrecta = pregunta.correctAnswers.Contains(indiceRespuesta);


        if (esCorrecta)
        {
            // Calcular los puntos según el tiempo restante.
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

    // Reactivar los controles al mostrar una nueva pregunta.
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

        // Configurar el Slider según los valores de la pregunta.
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

    // Gestiona el caso en el que se termina el tiempo.
    public void TiempoAgotado()
    {
        if (respuestaEnviada)
            return;

        respuestaEnviada = true;

        QuestionData pregunta = gameManager.CurrentQuestion;

        // Mostrar la respuesta correcta aunque no se haya respondido.
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

        // Desactivar los botones cuando se acaba el tiempo.
        foreach (Button boton in botonesRespuesta)
        {
            boton.interactable = false;
        }

        puntuacionObtenida = 0;

        scoreSystem.AñadirPuntos(puntuacionObtenida);

        feedbackUI.MostrarFeedback(FeedbackType.Timeout, puntuacionObtenida);
    }

    // Suscribirse al evento cuando se activa el objeto.
    private void OnEnable()
    {
        questionTimer.OnTimerFinished += TiempoAgotado;
    }

    // Dejar de escuchar el evento cuando se desactiva el objeto.
    private void OnDisable()
    {
        questionTimer.OnTimerFinished -= TiempoAgotado;
    }

    // Comprueba las respuestas de tipo MultipleCorrect.
    public void ComprobarRespuestasMultiples()
    {
        if (respuestaEnviada)
            return;

        respuestaEnviada = true;

        questionTimer.StopTimer();

        List<int> respuestasSeleccionadas = new List<int>();

        // Recogemos los Toggle seleccionados.
        for (int i = 0; i < togglesRespuesta.Length; i++)
        {
            if (togglesRespuesta[i].isOn)
            {
                respuestasSeleccionadas.Add(i);
            }

            togglesRespuesta[i].interactable = false;
        }

        QuestionData pregunta = gameManager.CurrentQuestion;

        // Comprobamos que coincidan todas las respuestas.
        bool esCorrecta = respuestasSeleccionadas.Count == pregunta.correctAnswers.Count;

        foreach (int respuesta in pregunta.correctAnswers)
        {
            if (!respuestasSeleccionadas.Contains(respuesta))
            {
                esCorrecta = false;
                break;
            }
        }

        // Obtenemos los textos de las respuestas correctas.
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

    // Actualiza el número que muestra el Slider.
    public void ActualizarValorSlider(float valor)
    {
        valueAnswerText.text = Mathf.RoundToInt(valor).ToString();
    }

    // Comprueba la respuesta seleccionada mediante el Slider.
    public void ComprobarRespuestaSlider()
    {
        if (respuestaEnviada)
            return;

        respuestaEnviada = true;

        questionTimer.StopTimer();

        respuestaSlider.interactable = false;

        QuestionData pregunta = gameManager.CurrentQuestion;

        float valorSeleccionado = respuestaSlider.value;

        // Comparar el valor seleccionado con el valor correcto.
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