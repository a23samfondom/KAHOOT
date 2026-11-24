using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ReportsUI : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject reportListPanel;
    [SerializeField] private GameObject reportViewerPanel;

    [Header("Lista de informes")]
    [SerializeField] private Transform reportListContent;
    [SerializeField] private GameObject reportItemPrefab;

    [Header("Visor de informes")]
    [SerializeField] private TMP_Text reportTitleText;
    [SerializeField] private TMP_Text reportContentText;

    [SerializeField] private LeaderboardManager leaderboardManager;

    private void Start()
    {
        reportListPanel.SetActive(true);
        reportViewerPanel.SetActive(false);

        CrearListaInformes();
    }

    private void CrearListaInformes()
    {
        string[] informes = leaderboardManager.ObtenerInformes();

        foreach (string ruta in informes)
        {
            string nombreInforme = System.IO.Path.GetFileNameWithoutExtension(ruta);

            GameObject nuevoItem = Instantiate(reportItemPrefab, reportListContent);

            ReportItemUI itemUI = nuevoItem.GetComponent<ReportItemUI>();

            if (itemUI == null)
            {
                Debug.LogError("El prefab no tiene ReportItemUI.");
                Destroy(nuevoItem);
                continue;
            }

            itemUI.Configurar(
                nombreInforme,
                () => AbrirInforme(ruta)
            );
        }
    }

    private void AbrirInforme(string ruta)
    {
        string nombreInforme = System.IO.Path.GetFileNameWithoutExtension(ruta);

        string contenido = System.IO.File.ReadAllText(ruta);

        reportTitleText.text = nombreInforme;
        reportContentText.text = contenido;

        reportListPanel.SetActive(false);
        reportViewerPanel.SetActive(true);
    }

    public void VolverAListaInformes()
    {
        reportViewerPanel.SetActive(false);
        reportListPanel.SetActive(true);
    }

    public void GenerarInformePrueba()
    {
        leaderboardManager.GenerarErrorPrueba();

        foreach (Transform hijo in reportListContent)
        {
            Destroy(hijo.gameObject);
        }

        CrearListaInformes();
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene("ModosMenu");
    }
}