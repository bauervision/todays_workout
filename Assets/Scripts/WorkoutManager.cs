using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutManager : MonoBehaviour
{
    public static WorkoutManager instance;
    [Header("Lists")]
    public List<GameObject> exercises = new List<GameObject>();

    [Header("Game Objects")]

    public GameObject GridList;
    public GameObject AddButton;
    public GameObject Inputs;
    public GameObject workoutPrefab;
    public GameObject RoundsCounter;
    public GameObject Randomizer;


    [Header("Inputs")]

    public InputField Input;
    public Dropdown TemplateDropdown;


    [Header("Text Objects")]
    public Text TimerText;
    public Text StartText;
    public Text FinalTime;
    public Text RoundsText;
    public Text FinalExerciseCountText;
    public Text FinalExerciseRoundText;
    public Text ActiveWorkoutText;

    #region Private Variables
    private bool hasStarted = false;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    private int workoutTime = 0;
    private int trainingMinutes = 0;
    private int trainingSeconds = 0;
    int totalRounds = 0;
    public int SelectedTemplate = 0;

    string[] ActiveWorkoutTemplate = new string[] { };


    List<string> selectedWorkoutTemplate = new List<string>();

    #endregion


    private void Start()
    {
        instance = this;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        RoundsCounter.SetActive(false);
        Randomizer.SetActive(false);
    }

    public void StartFresh()
    {
        UIManager.instance.StartFresh();
        ActiveWorkoutText.text = "Custom";
        ClearExerciseList();
        RoundsCounter.SetActive(false);
    }

    public void GoBackToStart()
    {
        if (UIManager.instance.isEditingWorkoutExercises)
        {
            GoToEditScreen();
        }
        else
        {
            UIManager.instance.BackToStart();
            workoutTime = 0;
            StartText.text = "Start";
            hasStarted = false;
            TimerText.text = ProcessWorkoutTime(workoutTime);
            Inputs.SetActive(true);
            RoundsCounter.SetActive(false);
            totalRounds = 0;
            if (Randomizer.activeInHierarchy)
                Randomizer.SetActive(false);
        }

    }

    public void GoToEditScreen()
    {
        UIManager.instance.GoToEditScreen();
        workoutTime = 0;
        StartText.text = "Start";
        hasStarted = false;
        TimerText.text = ProcessWorkoutTime(workoutTime);
        Inputs.SetActive(true);
        RoundsCounter.SetActive(false);
        totalRounds = 0;
        if (Randomizer.activeInHierarchy)
            Randomizer.SetActive(false);
    }
    public void RandomWorkout()
    {
        UIManager.instance.RandomWorkoutScreens();
        ShowWorkoutInput();
        RandomizeWorkout();
        Randomizer.SetActive(true);
        ActiveWorkoutText.text = "Random";
    }


    public void RandomizeWorkout()
    {
        ClearExerciseList();

        for (int i = 0; i < 7; i++)
            AddNewWorkout(GetRandomWorkout(i));


    }

    string GetRandomWorkout(int workoutIndex)
    {
        switch (workoutIndex)
        {
            case 1: return DataManager.instance.arms[Random.Range(0, DataManager.instance.arms.Length)];
            case 2: return DataManager.instance.back[Random.Range(0, DataManager.instance.back.Length)];
            case 3: return DataManager.instance.cardio[Random.Range(0, DataManager.instance.cardio.Length)];
            case 4: return DataManager.instance.chest[Random.Range(0, DataManager.instance.chest.Length)];
            case 5: return DataManager.instance.legs[Random.Range(0, DataManager.instance.legs.Length)];
            case 6: return DataManager.instance.shoulders[Random.Range(0, DataManager.instance.shoulders.Length)];
            default: return DataManager.instance.abs[Random.Range(0, DataManager.instance.abs.Length)];
        }

    }


    ///<summary>Called from the start screen, when the user has chosen a specific workout to load. </summary>
    public void LoadSelectedTemplate()
    {
        ClearExerciseList();
        // since we have loaded a specific workout, we have not chosen to go random
        Randomizer.SetActive(false);

        // handle UI screen switch
        UIManager.instance.ShowMainScreen();
        // we want to be able to "Add Another..."
        ShowWorkoutInput();
        // show the name of the current workout template at the top
        ActiveWorkoutText.text = Utils.GetActiveWorkoutName(SelectedTemplate);
        // set the active workout data
        ActiveWorkoutTemplate = Utils.GetActiveWorkout(SelectedTemplate);
        // finally run thru and add all the workouts for this template
        if (ActiveWorkoutTemplate != null)
            foreach (string workout in ActiveWorkoutTemplate)
                AddNewWorkout(workout);
    }

    void ClearExerciseList()
    {
        if (GridList.transform.childCount > 0)
            for (int i = 0; i < GridList.transform.childCount; i++)
                HandleRemoveWorkout(GridList.transform.GetChild(i).gameObject);
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
            newInputField.text = Utils.HandleCapitalCase(workout);
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
        exercises.Remove(removeObj);
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
        Randomizer.SetActive(false);
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
            UIManager.instance.WorkoutComplete();
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
