using System;
using System.Collections.Generic;

// Contiene los datos de un cuestionario.
[Serializable]
public class QuizData
{
    public string title; // Título del cuestionario.
    public List<QuestionData> questions; // Lista de preguntas.
}