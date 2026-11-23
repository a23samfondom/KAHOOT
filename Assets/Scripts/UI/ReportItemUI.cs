using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReportItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button openButton;

    public void Configurar(string titulo, Action alPulsarAbrir)
    {
        titleText.text = titulo;

        openButton.onClick.RemoveAllListeners();
        openButton.onClick.AddListener(() => alPulsarAbrir());
    }
}