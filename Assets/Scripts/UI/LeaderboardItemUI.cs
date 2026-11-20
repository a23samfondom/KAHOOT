using TMPro;
using UnityEngine;
public class LeaderboardItemUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text positionNumber;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text scoreText;

    public void Configurar(int posicion, string nickname, int puntuacion)
    {
        positionNumber.text = posicion.ToString();
        nicknameText.text = nickname;
        scoreText.text = puntuacion.ToString();
    }
}