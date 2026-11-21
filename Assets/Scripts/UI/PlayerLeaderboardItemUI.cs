using TMPro;
using UnityEngine;

public class PlayerLeaderboardPrefabItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text scoreText;

    public void Configurar(int posicion, string nickname, int puntuacion)
    {
        positionText.text = posicion.ToString();
        nicknameText.text = nickname;
        scoreText.text = puntuacion.ToString();
    }
}