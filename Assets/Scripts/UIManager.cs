using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Main Screens")]
    public GameObject StartScreen;
    public GameObject MainScreen;
    public GameObject EditScreen;
    public GameObject WorkoutScreen;
    public GameObject FinishScreen;
    public GameObject HelpScreen;

    [Header("Game Objects")]
    public Button TemplateBtn;
    public Button ExerciseBtn;
    public Text AddTemplateExerciseBtn;
    public Text DataStartupChecker;
    public GameObject HelpParent;
    public Button LaunchTemplateBtn;
    public GameObject EditDataChoices;

    [Header("Spawner")]
    public GameObject WorkoutGridList;
    public GameObject TemplateBtnItem;
    public GameObject WorkoutNameItem;

    [Header("Dropdowns")]
    ///<summary>Contains both Workouts and Exercises depending on the users need </summary>
    public Dropdown EditDataDropdown;
    ///<summary>Contains all of the loaded Workouts </summary>
    public Dropdown LoadDataDropdown;


    List<string> dropDownOptions = new List<string>();
    bool isEditingTemplates = false;
    private bool isEditingWorkoutExercises;

    string currentTemplate = string.Empty;

    private void Start()
    {
        instance = this;

        MainScreen.SetActive(false);
        EditScreen.SetActive(false);
        StartScreen.SetActive(true);
        HelpScreen.SetActive(false);
        DataStartupChecker.text = (SaveWorkout.CheckFirstTimeData()) ? "Edit Your Data" : "Start Your Data";

        LaunchTemplateBtn.interactable = false;
        LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().color = Color.grey;
    }


    public void ShowMainHelp()
    {
        HelpScreen.SetActive(true);
        HelpParent.transform.GetChild(0).gameObject.SetActive(true);
        HelpParent.transform.GetChild(1).gameObject.SetActive(false);
        HelpParent.transform.GetChild(2).gameObject.SetActive(false);
        HelpParent.transform.GetChild(3).gameObject.SetActive(false);
        HelpParent.transform.GetChild(4).gameObject.SetActive(false);
    }

    public void ShowWorkoutHelp()
    {
        HelpScreen.SetActive(true);

        // if the inputs are still showing then we are still in "setup" mode
        if (WorkoutManager.instance.Inputs.activeInHierarchy)
        {
            HelpParent.transform.GetChild(0).gameObject.SetActive(false);
            HelpParent.transform.GetChild(1).gameObject.SetActive(true);
            HelpParent.transform.GetChild(2).gameObject.SetActive(false);
            HelpParent.transform.GetChild(3).gameObject.SetActive(false);
            HelpParent.transform.GetChild(4).gameObject.SetActive(false);
        }
        else//setup is complete
        {
            HelpParent.transform.GetChild(0).gameObject.SetActive(false);
            HelpParent.transform.GetChild(1).gameObject.SetActive(false);
            HelpParent.transform.GetChild(2).gameObject.SetActive(true);
            HelpParent.transform.GetChild(3).gameObject.SetActive(false);
            HelpParent.transform.GetChild(4).gameObject.SetActive(false);
        }

    }

    public void ShowEditDataHelp()
    {
        HelpScreen.SetActive(true);
        HelpParent.transform.GetChild(0).gameObject.SetActive(false);
        HelpParent.transform.GetChild(1).gameObject.SetActive(false);
        HelpParent.transform.GetChild(2).gameObject.SetActive(false);

        HelpParent.transform.GetChild(3).gameObject.SetActive(!EditDataChoices.activeInHierarchy);
        HelpParent.transform.GetChild(4).gameObject.SetActive(EditDataChoices.activeInHierarchy);

    }



    public void HideHelp()
    {
        HelpScreen.SetActive(false);
    }

    public void UpdateWorkoutDropDown()
    {
        // clearout what we currently have
        EditDataDropdown.ClearOptions();
        LoadDataDropdown.ClearOptions();
        dropDownOptions.Clear();

        // start off with the "Select..." option
        dropDownOptions.Add("Select...");

        // now populate with our data
        foreach (WorkoutTemplate workout in DataManager.instance.myWorkouts)
            dropDownOptions.Add(workout.name);

        // finally load the options into the dropdowns
        LoadDataDropdown.AddOptions(dropDownOptions);
        EditDataDropdown.AddOptions(dropDownOptions);
    }
    public void StartFresh()
    {
        StartScreen.SetActive(false);
        EditScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void BackToStart()
    {
        StartScreen.SetActive(true);
        EditScreen.SetActive(false);
        MainScreen.SetActive(false);
        FinishScreen.SetActive(false);
        WorkoutScreen.SetActive(true);
    }

    public void GoToEditScreen()
    {
        StartScreen.SetActive(false);
        EditScreen.SetActive(true);
        MainScreen.SetActive(false);
        FinishScreen.SetActive(false);
        WorkoutScreen.SetActive(true);
        TemplateButtonSelected();
    }


    public void TemplateButtonSelected()
    {
        // if we are currently editing the exercises for a workout template then we just need to go back to our template page
        if (isEditingWorkoutExercises)
        {
            TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().text = "Templates";
            isEditingWorkoutExercises = false;
        }

        currentTemplate = string.Empty;
        isEditingTemplates = true;
        AddTemplateExerciseBtn.text = "Add New Workout";
        TemplateBtn.Select();
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Bold;
        //change non selected
        ExerciseBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Normal;
        ClearOutCurrentGridList();
        // now populate the list with the main categories for all of the workouts
        foreach (WorkoutTemplate workout in DataManager.instance.myWorkouts)
            AddTemplateToList(workout.name, false);

        EditDataChoices.SetActive(false);


    }

    public void ExerciseButtonSelected()
    {
        currentTemplate = string.Empty;
        isEditingWorkoutExercises = false;
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().text = "Templates";
        isEditingTemplates = false;
        AddTemplateExerciseBtn.text = "Add New Exercise";
        ExerciseBtn.Select();
        ExerciseBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Bold;
        //change non selected
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Normal;
        ClearOutCurrentGridList();
        EditDataChoices.SetActive(true);
    }

    public void RandomWorkoutScreens()
    {
        StartScreen.SetActive(false);
        MainScreen.SetActive(true);
    }

    public void WorkoutComplete()
    {
        WorkoutScreen.SetActive(false);
        FinishScreen.SetActive(true);
    }

    public void ShowMainScreen()
    {
        StartScreen.SetActive(false);
        MainScreen.SetActive(true);
    }


    ///<summary>Called from the UI: Adds a new item to the list based on which mode we are in  </summary>
    public void EditDataAddHandler()
    {
        // templates get an item that allows user to load exercises from a specific template for editing
        if (isEditingTemplates)
            if (!isEditingWorkoutExercises)
                AddTemplateToList("New Workout...", true);
            else
                AddExercisesToList("New Exercise...", true, true);
        else
            AddExercisesToList("New Exercise...", true, false);


    }

    void AddTemplateToList(string templateName, bool isNew)
    {
        if (isNew)
            DataManager.instance.myWorkouts.Add(new WorkoutTemplate(templateName));

        GameObject newItem = Instantiate(TemplateBtnItem, WorkoutGridList.transform);
        newItem.name = templateName;
        // give the input placeholder a starting text
        newItem.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = templateName;
        // assign the button listeners
        newItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => LoadTemplate(newItem));
        newItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RemoveWorkoutHandler(newItem));
        // grab the input
        InputField newInputField = newItem.transform.GetChild(1).GetComponent<InputField>();
        // assign the listner
        newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
    }

    void AddExercisesToList(string exerciseName, bool isNew, bool isTemplateExercise)
    {
        GameObject newItem = Instantiate(WorkoutNameItem, WorkoutGridList.transform);
        newItem.name = exerciseName;
        // give the input placeholder a starting text
        newItem.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = exerciseName;
        // assign the button listner
        newItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveExerciseHandler(newItem));
        // grab the input
        InputField newInputField = newItem.transform.GetChild(0).GetComponent<InputField>();


        if (isNew)
        {

            if (isTemplateExercise)
            {
                // add new data to the list of this workout
                string[] updatedList = DataManager.instance.myWorkouts.Find((workout) => workout.name == currentTemplate).list;
                updatedList.ToList().Add(exerciseName);
                // now update the workout data
                DataManager.instance.myWorkouts.Find((workout) => workout.name == currentTemplate).list = updatedList;


                // assign the listner
                newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField, currentTemplate); });
            }
            else
            {
                //this is a new exercise, add it to exercise
                DataManager.instance.myExercises.Add(new Exercise(exerciseName));
                // assign the listener
                newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
            }

        }
        else //this is not new
        {
            // assign the listener
            newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
        }

    }

    public void HandleTextChange(InputField input, string workoutName)
    {
        string currentName = input.transform.parent.name;
        input.text = Utils.HandleCapitalCase(input.text);
        input.transform.parent.name = input.text;

        // we want to update the name of this exercise--within a workout, so we need to find out the workout first
        WorkoutTemplate currentWorkout = DataManager.instance.myWorkouts.Find((workout) => workout.name == workoutName);
        //now find the exercise within that workout
        int indexOfUpdatedExercise = currentWorkout.list.ToList().FindIndex(exercisename => exercisename == currentName);
        // and make the update
        if (indexOfUpdatedExercise != -1)
            currentWorkout.list[indexOfUpdatedExercise] = input.text;

    }

    public void HandleTextChange(InputField input)
    {
        string currentName = input.transform.parent.name;
        input.text = Utils.HandleCapitalCase(input.text);
        input.transform.parent.name = input.text;

        // we want to update the name, so we need to find out which item this represents
        if (isEditingTemplates)
            DataManager.instance.myWorkouts.Find((workout) => workout.name == currentName).name = input.text;
        else
            DataManager.instance.myExercises.Find((exercise) => exercise.name == currentName).name = input.text;

    }

    public void RemoveExerciseHandler(GameObject thisItem)
    {
        Exercise exerciseDataToRemove = DataManager.instance.myExercises.Find((workout) => workout.name == thisItem.name);
        DataManager.instance.myExercises.Remove(exerciseDataToRemove);
        Destroy(thisItem);
    }

    public void RemoveWorkoutHandler(GameObject thisItem)
    {
        WorkoutTemplate workoutDataToRemove = DataManager.instance.myWorkouts.Find((workout) => workout.name == thisItem.name);
        DataManager.instance.myWorkouts.Remove(workoutDataToRemove);
        Destroy(thisItem);
    }

    public void LoadTemplate(GameObject templateObj)
    {
        LoadTemplateExercises(templateObj.name);
    }

    void LoadTemplateExercises(string templateName)
    {
        currentTemplate = templateName;
        isEditingWorkoutExercises = true;
        ClearOutCurrentGridList();

        if (isEditingTemplates)
        {
            WorkoutTemplate currentTemplate = DataManager.instance.myWorkouts.Find((workout) => workout.name == templateName);
            foreach (var item in currentTemplate.list)
                AddExercisesToList(item, false, false);
        }

        // update the template button
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().text = "Return To Templates";
        AddTemplateExerciseBtn.text = $"Add New {templateName} Exercise";
    }


    public void SetTemplateIndex(int index)
    {
        if (index != 0)
        {
            WorkoutManager.instance.SelectedTemplate = index - 1;
            LaunchTemplateBtn.interactable = true;
            LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().color = Color.cyan;
            LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().text = "Launch " + dropDownOptions[index] + " Template";
        }
        else
        {
            LaunchTemplateBtn.interactable = false;
            LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().color = Color.grey;
            LaunchTemplateBtn.transform.GetChild(0).transform.GetComponent<Text>().text = "Select something first";
        }
    }



    public void LoadSelectedEditData(int selectedIndex)
    {
        int dropDownOffset = selectedIndex - 1;

        string dataNameToLoad = (isEditingTemplates ? Utils.GetActiveWorkoutName(dropDownOffset) : Utils.GetActiveExerciseName(dropDownOffset));
        int dataCount = (isEditingTemplates ? Utils.GetActiveWorkout(dropDownOffset).Length : Utils.GetActiveExercises(dropDownOffset).Length);
        ClearOutCurrentGridList();

        if (isEditingTemplates)
        {
            WorkoutTemplate currentTemplate = DataManager.instance.myWorkouts.Find((workout) => workout.name == Utils.GetActiveWorkoutName(dropDownOffset));
            foreach (var item in currentTemplate.list)
                AddTemplateToList(item, false);
        }
        else
        {
            Exercise currentExercise = DataManager.instance.myExercises.Find((exercise) => exercise.name == Utils.GetActiveExerciseName(dropDownOffset));
            foreach (var item in currentExercise.list)
                AddExercisesToList(item, false, false);
        }
    }

    void ClearOutCurrentGridList()
    {
        // before we load anything, clear out what was there
        if (WorkoutGridList.transform.childCount > 0)
            for (int i = 0; i < WorkoutGridList.transform.childCount; i++)
                Destroy(WorkoutGridList.transform.GetChild(i).gameObject);
    }



}