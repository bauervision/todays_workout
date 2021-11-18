using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutManager : MonoBehaviour
{
    [Header("Lists")]
    public List<GameObject> exercises = new List<GameObject>();

    [Header("Game Objects")]
    public GameObject StartScreen;
    public GameObject MainScreen;
    public GameObject GridList;
    public GameObject AddButton;
    public GameObject Inputs;
    public GameObject workoutPrefab;
    public GameObject workoutScreen;
    public GameObject finishScreen;
    public GameObject RoundsCounter;
    public GameObject Randomizer;


    [Header("Inputs")]
    public Button LaunchTemplateBtn;
    public InputField Input;
    public Dropdown TemplateDropdown;
    List<Dropdown.OptionData> DD_options;

    [Header("Text Objects")]
    public Text TimerText;
    public Text StartText;
    public Text FinalTime;
    public Text RoundsText;
    public Text FinalExerciseCountText;
    public Text FinalExerciseRoundText;

    #region Private Variables
    private bool hasStarted = false;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    private int workoutTime = 0;
    private int trainingMinutes = 0;
    private int trainingSeconds = 0;
    int totalRounds = 0;
    int SelectedTemplate = 0;

    string[] ActiveWorkoutTemplate = new string[] { };
    string[] AbWorkouts = new string[] { "ab bicyles", "heel taps", "plank", "jack knife", "flutter kicks", "hollow body hold", "toe touches", "mountain climbers" };
    string[] ArmWorkouts = new string[] { "hammer curls", "ab bicycle", "curls", "tricep ext", "alt curls", "knee to elbow plank", "concentration curls", "curl burnout" };
    string[] BackWorkouts = new string[] { "supermans", "plank", "bird dog", "side plank", "glute bridge", "reverse fly", "dumbell row", "deadlift" };
    string[] CardioWorkouts = new string[] { "mountain climbers", "jump squat", "heel taps", "side 2 side", "high knees", "butt kickers", "burpees" };
    string[] ChestWorkouts = new string[] { "push ups", "tricep dips", "heel taps", "chest flies", "incline pushups", "plank", "chest abductions", "decline pushups" };
    string[] LegWorkouts = new string[] { "squats", "lunge pulse", "ab bicycle", "burpees", "single leg bridge", "dead bug", "leg circles", "weighted lunges" };
    string[] MixedWorkouts = new string[] { "burpees", "lunges", "heel taps", "pushups", "curls", "delt row", "tricep ext", "shoulder ext" };
    string[] ShoulderWorkouts = new string[] { "UpFrontBack", "heel taps", "reverse flies", "mountain climber", "delt row", "side extensions", "row" };

    // core exercises
    string[] abs = new string[] { "ab bicyles", "heel taps", "plank", "jack knife", "flutter kicks", "hollow body hold", "toe touches", "reach over crunch" };
    string[] arms = new string[] { "hammer curls", "curls", "tricep ext", "alt curls", "concentration curls", "curl burnout" };
    string[] back = new string[] { "supermans", "bird dog", "side plank", "glute bridge", "dumbell row", "deadlift", "row" };
    string[] cardio = new string[] { "mountain climbers", "jump squat", "side 2 side", "high knees", "butt kickers", "burpees" };
    string[] chest = new string[] { "push ups", "chest flies", "incline pushups", "chest abductions", "decline pushups", "front pull downs" };
    string[] legs = new string[] { "squats", "lunge pulse", "single leg bridge", "dead bug", "leg circles", "weighted lunges", "jump squat", "squat pulses" };
    string[] shoulders = new string[] { "UpFrontBack", "reverse flies", "delt row", "side extensions", "row", "side plank" };


    List<string> selectedWorkoutTemplate = new List<string>();

    #endregion


    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        RoundsCounter.SetActive(false);
        DD_options = TemplateDropdown.options;
        LaunchTemplateBtn.interactable = false;
        LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().color = Color.grey;
        MainScreen.SetActive(false);
        StartScreen.SetActive(true);
        Randomizer.SetActive(false);
    }

    public void StartFresh()
    {
        StartScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void GoBackToStart()
    {
        StartScreen.SetActive(true);
        MainScreen.SetActive(false);
        finishScreen.SetActive(false);
        workoutScreen.SetActive(true);
        workoutTime = 0;
        StartText.text = "Start";
        hasStarted = false;
        TimerText.text = ProcessWorkoutTime(workoutTime);
        Inputs.SetActive(true);
    }

    public void RandomWorkout()
    {
        StartScreen.SetActive(false);
        MainScreen.SetActive(true);
        ShowWorkoutInput();

        RandomizeWorkout();
        Randomizer.SetActive(true);

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
            case 1: return arms[Random.Range(0, arms.Length)];
            case 2: return back[Random.Range(0, back.Length)];
            case 3: return cardio[Random.Range(0, cardio.Length)];
            case 4: return chest[Random.Range(0, chest.Length)];
            case 5: return legs[Random.Range(0, legs.Length)];
            case 6: return shoulders[Random.Range(0, shoulders.Length)];
            default: return abs[Random.Range(0, abs.Length)];
        }

    }

    public void SetTemplateIndex(int index)
    {
        SelectedTemplate = index;
        if (index != 0)
        {
            LaunchTemplateBtn.interactable = true;
            LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().text = "Launch " + DD_options[index].text + " Template";
        }
    }

    public void LoadSelectedTemplate()
    {
        ClearExerciseList();

        if (SelectedTemplate != 0)
        {
            // handle UI switch
            StartScreen.SetActive(false);
            MainScreen.SetActive(true);
            ShowWorkoutInput();
            ActiveWorkoutTemplate = GetActiveWorkout();
            // run thru and add all the workouts for this template
            if (ActiveWorkoutTemplate != null)
                foreach (string workout in ActiveWorkoutTemplate)
                    AddNewWorkout(workout);

        }
    }

    void ClearExerciseList()
    {
        if (GridList.transform.childCount > 0)
            for (int i = 0; i < GridList.transform.childCount; i++)
                HandleRemoveWorkout(GridList.transform.GetChild(i).gameObject);
    }

    private string[] GetActiveWorkout()
    {
        switch (SelectedTemplate)
        {
            case 1: return AbWorkouts;
            case 2: return ArmWorkouts;
            case 3: return BackWorkouts;
            case 4: return CardioWorkouts;
            case 5: return ChestWorkouts;
            case 6: return LegWorkouts;
            case 7: return MixedWorkouts;
            case 8: return ShoulderWorkouts;
            default: return null;
        }
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
            newInputField.text = HandleCapitalCase(workout);
            newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
            // now add the listener for the remove button
            Button removeButton = newWorkout.transform.GetChild(5).GetComponent<Button>();
            removeButton.onClick.AddListener(() => HandleRemoveWorkout(newWorkout));

            Input.placeholder.GetComponent<Text>().text = "Add another?";
            Input.text = string.Empty;
        }

    }

    string HandleCapitalCase(string incomingString)
    {
        // spit string into an array
        List<string> splitArray = incomingString.Split(char.Parse(" ")).ToList();
        List<string> capitalCaseString = new List<string>();
        foreach (string word in splitArray)
        {
            capitalCaseString.Add(UppercaseFirst(word));
        }
        return string.Join(" ", capitalCaseString.ToArray());
    }


    string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
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
