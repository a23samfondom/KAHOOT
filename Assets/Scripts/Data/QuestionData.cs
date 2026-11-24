using System;
using System.Collections.Generic;

// Contiene los datos de una pregunta.
[Serializable]
public class QuestionData
{
    public string questionText; // Enunciado de la pregunta.
    public QuestionType questionType; // Tipo de pregunta.
    public int duration; // Tiempo disponible para responder.
    public ScoreType scoreMultiplier; // Multiplicador de puntuación.
    public List<string> answers; // Respuestas posibles.
    public List<int> correctAnswers; // Índices de las respuestas correctas.
    public float minimumValue; // Valor mínimo del Slider.
    public float maximumValue; // Valor máximo del Slider.
    public float correctValue; // Valor correcto del Slider.
}