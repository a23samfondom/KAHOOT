using UnityEngine;

// Calcula y almacena la puntuación de la partida.
public class ScoreSystem : MonoBehaviour
{
    private const int MAX_SCORE = 1000;
    private int totalScore = 0;

    // Devuelve la puntuación acumulada.
    public int TotalScore
    {
        get { return totalScore; }
    }

    // Calcula los puntos según el tiempo restante y el multiplicador.
    public int CalcularPuntuacion(float tiempoRestante, float duracionTotal, ScoreType multiplicador)
    {
        if (tiempoRestante <= 0)
            return 0;

        // Calcular la proporción de tiempo que queda.
        float porcentajeTiempo = tiempoRestante / duracionTotal;

        int puntuacion = Mathf.RoundToInt(MAX_SCORE * porcentajeTiempo);

        // Aplicar puntuación doble si corresponde.
        if (multiplicador == ScoreType.Double)
        {
            puntuacion *= 2;
        }

        return puntuacion;
    }

    // Suma los puntos obtenidos al total de la partida.
    public void AñadirPuntos(int puntos)
    {
        totalScore += puntos;

        Debug.Log("Puntos obtenidos: " + puntos);
        Debug.Log("Puntuación total: " + totalScore);
    }
}