using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutManager : MonoBehaviour
{
    public GameObject GridList;
    public GameObject AddButton;
    public GameObject Inputs;
    public GameObject workoutPrefab;
    public GameObject finishScreen;
    //public GameObject SliderPanel;
    //public GameObject AddTimerButton;

    public InputField Input;
    public Text TimerText;
    public Text StartText;
    public Text FinalTime;
    //public Text SliderTimeText;
    private bool hasStarted = false;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    private int workoutTime = 0;
    private int trainingMinutes = 0;
    private int trainingSeconds = 0;

    //private bool createTimedExercise = false;
    //private int timedExerciseTime = 0;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //SliderPanel.SetActive(false);
    }

    public void ShowWorkoutInput()
    {
        AddButton.SetActive(false);
    }

    public void SetTimer(float timerValue)
    {
        // timedExerciseTime = (int)timerValue * 60;
        // SliderTimeText.text = ProcessWorkoutTime(timedExerciseTime);
    }

    public void AddNewWorkout(string workout)
    {

        if (workout != null)
        {
            GameObject newWorkout = Instantiate(workoutPrefab, GridList.transform);

            // grab the input and assign its onEndEdit method
            InputField newInputField = newWorkout.transform.GetChild(2).GetComponent<InputField>();
            newInputField.text = workout;
            newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });

            // now add the listener for the remove button
            Button removeButton = newWorkout.transform.GetChild(3).GetComponent<Button>();
            removeButton.onClick.AddListener(() => HandleRemoveWorkout(newWorkout));

            Input.placeholder.GetComponent<Text>().text = "Add another?";
            Input.text = string.Empty;
        }

    }

    public void HandleRemoveWorkout(GameObject removeObj)
    {
        Destroy(removeObj);
    }

    public void HandleTextChange(InputField input)
    {
        print("New Workout: " + input.text);
    }

    public void CreateTimedExercise()
    {
        //createTimedExercise = true;
        Input.gameObject.SetActive(false);
        //SliderPanel.SetActive(true);
        //AddTimerButton.transform.GetChild(0).transform.gameObject.SetActive(false);
    }

    public void DoneWithSetup()
    {
        Inputs.SetActive(false);
    }

    public void StartTimer()
    {
        hasStarted = !hasStarted;

        if (hasStarted)
        {
            StartText.text = "Stop";
            StartCoroutine(Timer());
        }
        else
        {

            StopAllCoroutines();
            finishScreen.SetActive(true);
            FinalTime.text = ProcessWorkoutTime(workoutTime - 1);
        }

    }



    IEnumerator Timer()
    {

        while (true)
        {
            TimerText.text = ProcessWorkoutTime(workoutTime);
            workoutTime++;
            yield return waitForSeconds;
        }


    }

    private string ProcessWorkoutTime(int seconds)
    {
        trainingMinutes = Mathf.FloorToInt(seconds / 60F);
        trainingSeconds = Mathf.FloorToInt(seconds - trainingMinutes * 60);
        return string.Format("{0:00}:{1:00}", trainingMinutes, trainingSeconds);
    }



}
