using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class QuizItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text numberText;
    [SerializeField] private Button playButton;

    public void Configurar(QuizData quiz, Action alPulsarJugar)
    {
        titleText.text = quiz.title;

        numberText.text = quiz.questions.Count.ToString();

        playButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(() => alPulsarJugar());
    }
}
