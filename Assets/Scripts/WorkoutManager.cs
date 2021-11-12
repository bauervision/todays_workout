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
    public GameObject workoutScreen;
    public GameObject finishScreen;
    public GameObject RoundsCounter;

    public InputField Input;
    public Text TimerText;
    public Text StartText;
    public Text FinalTime;
    public Text RoundsText;
    public Text FinalExerciseCountText;
    public Text FinalExerciseRoundText;


    private bool hasStarted = false;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    private int workoutTime = 0;
    private int trainingMinutes = 0;
    private int trainingSeconds = 0;

    int totalRounds = 0;
    public List<GameObject> exercises = new List<GameObject>();

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        RoundsCounter.SetActive(false);
    }

    public void ShowWorkoutInput()
    {
        AddButton.SetActive(false);
    }


    public void AddRound()
    {
        totalRounds++;
        RoundsText.text = totalRounds.ToString();
    }

    public void AddNewWorkout(string workout)
    {

        if (workout != null)
        {
            GameObject newWorkout = Instantiate(workoutPrefab, GridList.transform);
            exercises.Add(newWorkout);
            // grab the input
            InputField newInputField = newWorkout.transform.GetChild(2).GetComponent<InputField>();
            newInputField.text = workout;
            newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
            // now add the listener for the remove button
            Button removeButton = newWorkout.transform.GetChild(5).GetComponent<Button>();
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
        Input.gameObject.SetActive(false);
    }

    public void DoneWithSetup()
    {
        Inputs.SetActive(false);
        RoundsCounter.SetActive(true);

        foreach (GameObject item in exercises)
        {
            TimedExcercise thisTimeExcercise = item.GetComponent<TimedExcercise>();
            // if an exercise has not been set as a timed one...
            if (!thisTimeExcercise.isTimedExercise)
                item.transform.GetChild(1).gameObject.SetActive(false);
            else // it is a timed excercise
                thisTimeExcercise.activeTimedExcercise = true;
        }
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
            workoutScreen.SetActive(false);
            finishScreen.SetActive(true);
            FinalTime.text = ProcessWorkoutTime(workoutTime - 1);
            FinalExerciseRoundText.text = totalRounds.ToString() + " Rounds";
            FinalExerciseCountText.text = exercises.Count.ToString() + " Exercises";
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
