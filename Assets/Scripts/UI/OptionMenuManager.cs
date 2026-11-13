using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionMenuManager : MonoBehaviour
{
    [SerializeField] Toggle fullscreen;

    /* VALORES INICIALES */
    private void Start()
    {
        fullscreen.isOn = Screen.fullScreen;
        fullscreen.onValueChanged.AddListener(CambiarPantallaCompleta);
    }


    /* PANTALLA COMPLETA */
    private void CambiarPantallaCompleta(bool activado)
    {
        Screen.fullScreen = activado;

        if (!activado) { Debug.Log("NO fullscreen"); }
        else { Debug.Log("SI fullscreen"); }

    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
