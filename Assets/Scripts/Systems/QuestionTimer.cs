using UnityEngine;
using UnityEngine.UI;
using System;


public class QuestionTimer : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Slider durationSlider;

    private float timeRemaining;
    private float totalDuration;
    private bool isRunning;

    public float TimeRemaining
    {
        get { return timeRemaining; }
    }

    public event Action OnTimerFinished;

    private void Update()
    {
        if (!isRunning)
            return;

        timeRemaining -= Time.deltaTime;

        durationSlider.value = timeRemaining;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            durationSlider.value = 0;
            isRunning = false;

            OnTimerFinished?.Invoke();
        }
    }

    public void StartTimer(float duration)
    {
        totalDuration = duration;
        timeRemaining = duration;

        durationSlider.maxValue = totalDuration;
        durationSlider.value = totalDuration;

        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }


}


