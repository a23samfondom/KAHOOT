using TMPro;
using UnityEngine;

// Controla los datos visuales de una posición de la clasificación.
public class LeaderboardItemUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text positionNumber;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text scoreText;

    // Asigna la posición, el nombre y la puntuación del jugador.
    public void Configurar(int posicion, string nickname, int puntuacion)
    {
        positionNumber.text = posicion.ToString();
        nicknameText.text = nickname;
        scoreText.text = puntuacion.ToString();
    }
}