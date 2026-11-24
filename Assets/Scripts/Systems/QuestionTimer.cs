using UnityEngine;
using UnityEngine.UI;
using System;

// Controla el tiempo disponible para responder cada pregunta.
public class QuestionTimer : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Slider durationSlider;

    private float timeRemaining;
    private float totalDuration;
    private bool isRunning;

    // Permite consultar el tiempo que queda.
    public float TimeRemaining
    {
        get { return timeRemaining; }
    }

    // Evento que se activa cuando termina el tiempo.
    public event Action OnTimerFinished;

    private void Update()
    {
        // No actualizar el tiempo si el temporizador está parado.
        if (!isRunning)
            return;

        timeRemaining -= Time.deltaTime;

        // Actualizar la barra visual del temporizador.
        durationSlider.value = timeRemaining;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            durationSlider.value = 0;
            isRunning = false;

            // Avisar de que el tiempo ha terminado.
            OnTimerFinished?.Invoke();
        }
    }

    // Inicia el temporizador con la duración de la pregunta.
    public void StartTimer(float duration)
    {
        totalDuration = duration;
        timeRemaining = duration;

        durationSlider.maxValue = totalDuration;
        durationSlider.value = totalDuration;

        isRunning = true;
    }

    // Detiene el temporizador.
    public void StopTimer()
    {
        isRunning = false;
    }
}