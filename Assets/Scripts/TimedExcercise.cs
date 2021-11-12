using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedExcercise : MonoBehaviour
{
    public GameObject MakeTimer;
    public GameObject InputField;
    public GameObject SliderPanel;
    public GameObject ActiveTimePanel;

    public AudioSource timerSource;

    public Slider TimeSlider;
    public Text TimerText;
    public Text SetTimerText;


    public Color activeColor;
    public Color defaultColor;

    public Image timerImage;
    public Image playButtonImage;

    public Sprite play;
    public Sprite stop;

    public bool isTimedExercise = false;
    public bool activeTimedExcercise = false;

    int timedExerciseTime = 60;
    int minutes = 1;
    int seconds = 0;

    int actualTime = 0;



    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    Coroutine timer;

    public void CreateTimedExercise()
    {

        SliderPanel.SetActive(!activeTimedExcercise);
        ActiveTimePanel.SetActive(activeTimedExcercise);

        MakeTimer.SetActive(false);
        InputField.SetActive(false);

        actualTime = timedExerciseTime;
        if (ActiveTimePanel.activeInHierarchy)
        {
            TimerText.text = ProcessWorkoutTime(actualTime);
            TimeSlider.value = actualTime;
        }


    }

    public void ConfirmTimedExercise()
    {
        timerImage.color = activeColor;
        isTimedExercise = true;
        MakeTimer.SetActive(true);
        InputField.SetActive(true);
        SliderPanel.SetActive(false);
    }
    public void CancelTimedExercise()
    {
        timerImage.color = defaultColor;
        isTimedExercise = false;
        MakeTimer.SetActive(true);
        InputField.SetActive(true);
        SliderPanel.SetActive(false);
    }

    public void SetTimer(float timerValue)
    {
        timedExerciseTime = (int)timerValue * 60;
        SetTimerText.text = ProcessWorkoutTime(timedExerciseTime);
    }

    private string ProcessWorkoutTime(int time)
    {
        minutes = Mathf.FloorToInt(time / 60F);
        seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }




    bool hasStarted = false;
    public void HandleTimer()
    {
        hasStarted = !hasStarted;
        if (hasStarted)
        {
            playButtonImage.sprite = stop;
            timerSource.Play();
            timer = StartCoroutine(Timer());
        }
        else
        {
            playButtonImage.sprite = play;

            SliderPanel.SetActive(false);
            ActiveTimePanel.SetActive(false);
            MakeTimer.SetActive(true);
            InputField.SetActive(true);
            StopCoroutine(timer);
            timerSource.Play();

        }

    }


    IEnumerator Timer()
    {
        while (actualTime != 0)
        {
            TimerText.text = ProcessWorkoutTime(actualTime);
            TimeSlider.value = Mathf.InverseLerp(0, timedExerciseTime, actualTime);
            actualTime--;
            yield return waitForSeconds;
        }
        timerSource.Play();
        playButtonImage.sprite = play;
        SliderPanel.SetActive(false);
        ActiveTimePanel.SetActive(false);
        MakeTimer.SetActive(true);
        InputField.SetActive(true);
        hasStarted = false;
    }

}
