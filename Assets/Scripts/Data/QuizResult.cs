using System;

// Guarda el resultado de una partida.
[Serializable]
public class QuizResult
{
    public string nickname; // Nombre del jugador.
    public int score;       // Puntuación obtenida.
    public float time;      // Tiempo empleado.
}