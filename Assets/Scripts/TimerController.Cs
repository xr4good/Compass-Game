using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float timerDuration = 180f;
    private float timeRemaining;
    public Slider timerSlider;

    private bool isRunning = true;

    void Start()
    {
        timerDuration = PlayerPrefs.GetFloat("selectTime");
        timeRemaining = timerDuration;
        //debug
        Debug.Log("Tempo restante: " + timerDuration);
        if (timerSlider != null)
        {
            timerSlider.maxValue = timerDuration;
            timerSlider.value = timerDuration;
        }
    }

    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timerSlider != null)
            {
                timerSlider.value = timeRemaining;
            }
        }
        else if (timeRemaining <= 0)
        {
            isRunning = false;
            timeRemaining = 0;
            Debug.Log("Tempo esgotado!");
        }
    }
}