using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class QuizCreatorUI : MonoBehaviour
{
    [Header("Datos del Kahoot")]
    [SerializeField] private TMP_InputField quizTitleInput;
    [SerializeField] private Button saveQuizButton;

    [Header("Lista de preguntas")]
    [SerializeField] private Transform questionsContent;
    [SerializeField] private GameObject questionItemPrefab;
    [SerializeField] private Button addQuestionButton;
    [SerializeField] private Button removeQuestionButton;

    [Header("Editor de preguntas")]
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_InputField[] answerInputs;
    [SerializeField] private Button[] correctAnswerButtons;
    [SerializeField] private TMP_InputField durationInput;
    [SerializeField] private Toggle doubleScoreToggle;

    [Header("Panel de mensajes")]
    [SerializeField] private GameObject feedbackPanel;
    [SerializeField] private TMP_Text feedbackText;
    [SerializeField] private Button closeFeedbackButton;

    [Header("Panel inicial")]
    [SerializeField] private GameObject panelInicio;
    [SerializeField] private Button usarPlantillaButton;
    [SerializeField] private Button desdeCeroButton;

    private List<QuestionData> questions = new List<QuestionData>();

    private int selectedQuestionIndex = -1;
    private int correctAnswerIndex = -1;

    private void Start()
    {
        addQuestionButton.onClick.AddListener(AñadirPregunta);
        removeQuestionButton.onClick.AddListener(EliminarPregunta);

        ConfigurarBotonesCorrectos();
        ConfigurarCamposTexto();
        ConfigurarOpcionesPregunta();

        saveQuizButton.onClick.AddListener(GuardarKahoot);

        closeFeedbackButton.onClick.AddListener(CerrarFeedback);
        feedbackPanel.SetActive(false);

        usarPlantillaButton.onClick.AddListener(UsarPlantilla);
        desdeCeroButton.onClick.AddListener(DesdeCero);
    }

    private void AñadirPregunta()
    {
        QuestionData nuevaPregunta = new QuestionData();

        nuevaPregunta.questionText = "";
        nuevaPregunta.questionType = QuestionType.Normal;
        nuevaPregunta.duration = 20;
        nuevaPregunta.scoreMultiplier = ScoreType.Standard;
        nuevaPregunta.answers = new List<string>()
    {
        "", "", "", ""
    };
        nuevaPregunta.correctAnswers = new List<int>();

        questions.Add(nuevaPregunta);

        int numeroPregunta = questions.Count;

        GameObject nuevoItem = Instantiate(questionItemPrefab, questionsContent);

        QuestionItemUI itemUI = nuevoItem.GetComponent<QuestionItemUI>();

        itemUI.Configurar(
            numeroPregunta,
            () => SeleccionarPregunta(numeroPregunta - 1)
        );

        SeleccionarPregunta(numeroPregunta - 1);
    }

    private void SeleccionarPregunta(int indice)
    {
        GuardarPreguntaActual();

        selectedQuestionIndex = indice;

        QuestionData pregunta = questions[indice];

        questionInput.text = pregunta.questionText;

        for (int i = 0; i < answerInputs.Length; i++)
        {
            answerInputs[i].text = pregunta.answers[i];
        }

        durationInput.text = pregunta.duration.ToString();

        doubleScoreToggle.isOn = pregunta.scoreMultiplier == ScoreType.Double;

        correctAnswerIndex = pregunta.correctAnswers.Count > 0
            ? pregunta.correctAnswers[0]
            : -1;

        ActualizarBotonesCorrectos();
    }

    private void GuardarPreguntaActual()
    {
        if (selectedQuestionIndex < 0 || selectedQuestionIndex >= questions.Count)
        {
            return;
        }

        QuestionData pregunta = questions[selectedQuestionIndex];

        pregunta.questionText = questionInput.text;

        for (int i = 0; i < answerInputs.Length; i++)
        {
            pregunta.answers[i] = answerInputs[i].text;
        }

        int duracion;

        if (int.TryParse(durationInput.text, out duracion))
        {
            pregunta.duration = duracion;
        }

        pregunta.scoreMultiplier = doubleScoreToggle.isOn
            ? ScoreType.Double
            : ScoreType.Standard;

        pregunta.correctAnswers.Clear();

        if (correctAnswerIndex >= 0)
        {
            pregunta.correctAnswers.Add(correctAnswerIndex);
        }
    }

    private void SeleccionarRespuestaCorrecta(int indice)
    {
        correctAnswerIndex = indice;

        GuardarPreguntaActual();

        ActualizarBotonesCorrectos();
    }

    private void ActualizarBotonesCorrectos()
    {
        for (int i = 0; i < correctAnswerButtons.Length; i++)
        {
            Image imagen = correctAnswerButtons[i].GetComponent<Image>();

            if (i == correctAnswerIndex)
            {
                imagen.color = new Color(0.65f, 0.9f, 0.65f);
            }
            else
            {
                imagen.color = Color.white;
            }
        }
    }

    private void ConfigurarBotonesCorrectos()
    {
        for (int i = 0; i < correctAnswerButtons.Length; i++)
        {
            int indice = i;

            correctAnswerButtons[i].onClick.AddListener(
                () => SeleccionarRespuestaCorrecta(indice)
            );
        }
    }

    private void ConfigurarCamposTexto()
    {
        questionInput.onValueChanged.AddListener((texto) =>
        {
            if (selectedQuestionIndex >= 0)
            {
                questions[selectedQuestionIndex].questionText = texto;
            }
        });

        for (int i = 0; i < answerInputs.Length; i++)
        {
            int indice = i;

            answerInputs[i].onValueChanged.AddListener((texto) =>
            {
                if (selectedQuestionIndex >= 0)
                {
                    questions[selectedQuestionIndex].answers[indice] = texto;
                }
            });
        }
    }

    private void ConfigurarOpcionesPregunta()
    {
        durationInput.onValueChanged.AddListener((texto) =>
        {
            if (selectedQuestionIndex >= 0 && int.TryParse(texto, out int duracion) && duracion > 0)
            {
                questions[selectedQuestionIndex].duration = duracion;
            }
        });

        doubleScoreToggle.onValueChanged.AddListener((activado) =>
        {
            if (selectedQuestionIndex >= 0)
            {
                questions[selectedQuestionIndex].scoreMultiplier = activado
                    ? ScoreType.Double
                    : ScoreType.Standard;
            }
        });
    }

    private void EliminarPregunta()
    {
        if (questions.Count <= 1)
        {
            return;
        }

        GuardarPreguntaActual();

        int ultimaPregunta = questions.Count - 1;

        questions.RemoveAt(ultimaPregunta);

        Destroy(questionsContent.GetChild(ultimaPregunta).gameObject);

        for (int i = 0; i < questions.Count; i++)
        {
            int indice = i;

            QuestionItemUI itemUI = questionsContent.GetChild(i)
                .GetComponent<QuestionItemUI>();

            itemUI.Configurar(
                i + 1,
                () => SeleccionarPregunta(indice)
            );
        }

        if (selectedQuestionIndex >= questions.Count)
        {
            selectedQuestionIndex = -1;
            SeleccionarPregunta(questions.Count - 1);
        }
    }

    private void GuardarKahoot()
    {
        GuardarPreguntaActual();

        if (!Validaciones())
        {
            return;
        }

        QuizData nuevoKahoot = new QuizData();

        nuevoKahoot.title = quizTitleInput.text;
        nuevoKahoot.questions = questions;

        string carpeta = Path.Combine(
            Application.persistentDataPath,
            "Kahoots"
        );

        Directory.CreateDirectory(carpeta);

        string ruta = Path.Combine(
            carpeta,
            nuevoKahoot.title + ".json"
        );

        if (File.Exists(ruta))
        {
            MostrarFeedback("CUIDADO! Ya existe un Kahoot con ese título. Elige otro.");
            return;
        }

        string json = JsonUtility.ToJson(nuevoKahoot, true);

        File.WriteAllText(ruta, json);

        MostrarFeedback("Kahoot guardado correctamente");
    }

    private bool Validaciones()
    {
        if (string.IsNullOrWhiteSpace(quizTitleInput.text))
        {
            MostrarFeedback("CUIDADO! El Kahoot necesita un título.");
            return false;
        }

        for (int i = 0; i < questions.Count; i++)
        {
            QuestionData pregunta = questions[i];

            if (string.IsNullOrWhiteSpace(pregunta.questionText))
            {
                MostrarFeedback("CUIDADO! La pregunta " + (i + 1) + " no tiene enunciado.");
                return false;
            }

            for (int j = 0; j < pregunta.answers.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(pregunta.answers[j]))
                {
                    MostrarFeedback("CUIDADO! La respuesta " + (j + 1) + " de la pregunta " + (i + 1) + " está vacía.");
                    return false;
                }
            }

            if (pregunta.correctAnswers.Count == 0)
            {
                MostrarFeedback("CUIDADO! La pregunta " + (i + 1) + " no tiene una respuesta correcta seleccionada.");
                return false;
            }

            if (pregunta.duration <= 0)
            {
                MostrarFeedback("CUIDADO! La duración de la pregunta " + (i + 1) + " debe ser mayor que 0.");
                return false;
            }
        }

        return true;
    }

    private void MostrarFeedback(string mensaje)
    {
        feedbackText.text = mensaje;
        feedbackPanel.SetActive(true);
    }

    private void CerrarFeedback()
    {
        feedbackPanel.SetActive(false);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }

    private void UsarPlantilla()
    {
        panelInicio.SetActive(false);

        // Buscar el archivo JSON de la plantilla.
        TextAsset archivo = Resources.Load<TextAsset>("Kahoots/Plantilla");

        if (archivo == null)
        {
            MostrarFeedback("No se ha encontrado el archivo Plantilla.json.");
            return;
        }

        // Convertir el JSON en un objeto QuizData.
        QuizData plantilla = JsonUtility.FromJson<QuizData>(archivo.text);

        if (plantilla == null || plantilla.questions == null)
        {
            MostrarFeedback("No se ha podido cargar la plantilla.");
            return;
        }

        quizTitleInput.text = "";

        questions.Clear();
        selectedQuestionIndex = -1;

        // Eliminar los elementos anteriores de la lista.
        foreach (Transform hijo in questionsContent)
        {
            Destroy(hijo.gameObject);
        }

        // Añadir las preguntas de la plantilla al editor.
        for (int i = 0; i < plantilla.questions.Count; i++)
        {
            questions.Add(plantilla.questions[i]);

            int indice = i;

            GameObject nuevoItem = Instantiate(questionItemPrefab, questionsContent);

            QuestionItemUI itemUI = nuevoItem.GetComponent<QuestionItemUI>();

            itemUI.Configurar(
                i + 1,
                () => SeleccionarPregunta(indice)
            );
        }

        // Mostrar la primera pregunta en el editor.
        SeleccionarPregunta(0);
    }

    private void DesdeCero()
    {
        panelInicio.SetActive(false);
        AñadirPregunta();
    }
}