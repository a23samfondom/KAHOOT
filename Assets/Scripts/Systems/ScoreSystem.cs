using UnityEngine;
public class ScoreSystem : MonoBehaviour
{
    private const int MAX_SCORE = 1000;
    private int totalScore = 0;
    public int TotalScore
    {
        get { return totalScore; }
    }

    public int CalcularPuntuacion(float tiempoRestante, float duracionTotal, ScoreType multiplicador)
    {
        if (tiempoRestante <= 0)
            return 0;

        float porcentajeTiempo = tiempoRestante / duracionTotal;

        int puntuacion = Mathf.RoundToInt(MAX_SCORE * porcentajeTiempo);

        if (multiplicador == ScoreType.Double)
        {
            puntuacion *= 2;
        }

        return puntuacion;
    }

    public void AñadirPuntos(int puntos)
    {
        totalScore += puntos;

        Debug.Log("Puntos obtenidos: " + puntos);
        Debug.Log("Puntuación total: " + totalScore);
    }
}