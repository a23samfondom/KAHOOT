using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestionItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private Button button;

    public void Configurar(int numero, Action alSeleccionar)
    {
        questionText.text = "Pregunta " + numero;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => alSeleccionar());
    }
}