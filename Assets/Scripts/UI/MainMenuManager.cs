using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroup creditPanel;


    /*VALORES INICIALES CREDITOS*/
    private void Start()
    {
        creditPanel.alpha = 0;
        creditPanel.interactable = false;
        creditPanel.blocksRaycasts = false;
    }


    /* JUGAR */
    public void Jugar()
    {
        SceneManager.LoadScene("ModosMenu");
    }



    /* CRÉDITOS */
    public void Creditos()
    {
        creditPanel.alpha = 1;
        creditPanel.interactable = true;
        creditPanel.blocksRaycasts = true;

    }


    /* VOLVER AL MENÚ */
    public void VolverAlMenu()
    {
        creditPanel.alpha = 0;
        creditPanel.interactable = false;
        creditPanel.blocksRaycasts = false;
    }


    /* CERRAR JUEGO */
    public void CerrarApp()
    {
        Application.Quit();
        Debug.Log("Cerrado");
    }
}