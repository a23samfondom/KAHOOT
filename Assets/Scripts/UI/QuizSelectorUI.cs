using UnityEngine;
public class QuizSelectorUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private QuizLoader quizLoader;
    [SerializeField] private QuizSelector quizSelector;

    [Header("Elementos de la interfaz")]
    [SerializeField] private Transform content;
    [SerializeField] private GameObject quizItemPrefab;

    private void Start()
    {
        CrearListaKahoots();
    }

    private void CrearListaKahoots()
    {
        // Comprobar las referencias.
        if (quizLoader == null || quizSelector == null)
        {
            Debug.LogError("Faltan las referencias del QuizLoader o QuizSelector.");
            return;
        }

        if (content == null || quizItemPrefab == null)
        {
            Debug.LogError("Falta asignar el Content o el prefab.");
            return;
        }

        // Comprobar si hay Kahoots disponibles.
        if (quizLoader.Quizzes.Count == 0)
        {
            Debug.LogWarning("No hay Kahoots disponibles.");
            return;
        }

        // Recorrer todos los Kahoots cargados.
        for (int i = 0; i < quizLoader.Quizzes.Count; i++)
        {
            QuizData quiz = quizLoader.Quizzes[i];

            // Crear un elemento dentro del ScrollView.
            GameObject nuevoItem = Instantiate(quizItemPrefab, content);

            // Obtener el script del prefab.
            QuizItemUI itemUI = nuevoItem.GetComponent<QuizItemUI>();

            if (itemUI == null)
            {
                Debug.LogError("El prefab no tiene QuizItemUI.");
                Destroy(nuevoItem);
                continue;
            }

            // Guardar el índice del Kahoot actual.
            int indiceQuiz = i;

            // Rellenar el elemento y configurar su botón.
            itemUI.Configurar(
                quiz,
                () => quizSelector.SeleccionarQuiz(indiceQuiz)
            );
        }

        Debug.Log("Lista de Kahoots creada correctamente.");
    }
}