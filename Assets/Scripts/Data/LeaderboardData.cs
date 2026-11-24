using System;
using System.Collections.Generic;

// Contiene los resultados de una clasificación.
[Serializable]
public class LeaderboardData
{
    public List<QuizResult> results = new List<QuizResult>(); // Lista de resultados guardados.
}