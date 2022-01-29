using System.Collections;
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

    string currentWorkoutTemplateName = string.Empty;
    int currentMuscleGroupIndex = -1;

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


    #region Help Methods
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

        // HelpParent.transform.GetChild(3).gameObject.SetActive(!EditDataChoices.activeInHierarchy);
        // HelpParent.transform.GetChild(4).gameObject.SetActive(EditDataChoices.activeInHierarchy);

    }

    public void HideHelp()
    {
        HelpScreen.SetActive(false);
    }

    #endregion


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

        currentWorkoutTemplateName = string.Empty;
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

        //EditDataChoices.SetActive(false);


    }

    public void ExerciseButtonSelected()
    {
        currentWorkoutTemplateName = string.Empty;
        isEditingWorkoutExercises = false;
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().text = "Templates";
        isEditingTemplates = false;
        AddTemplateExerciseBtn.text = "Add New Exercise";
        ExerciseBtn.Select();
        ExerciseBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Bold;
        //change non selected
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Normal;
        ClearOutCurrentGridList();
        //EditDataChoices.SetActive(true);
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
            if (!isEditingWorkoutExercises)//if we want to add a whole new workout...
                AddTemplateToList("New Workout...", true);
            else//otherwise we want to add a new exercise to this workout
                AddExercisesToList("New Exercise...", true, true);
        else//we just want to add a new exercise
            AddExercisesToList("New Exercise...", true, false);


    }

    ///<summary>User has pressed the button to add a new workout routine </summary>
    void AddTemplateToList(string templateName, bool isNew)
    {
        if (isNew)
            DataManager.instance.AddNewWorkoutTemplate(templateName);

        GameObject newItem = Instantiate(TemplateBtnItem, WorkoutGridList.transform);
        newItem.name = templateName;
        // give the input placeholder the initial name
        newItem.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = templateName;
        // assign the button listeners
        newItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => LoadWorkoutTemplate(newItem));
        newItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RemoveWorkoutHandler(newItem));
        // grab the input
        InputField newInputField = newItem.transform.GetChild(1).GetComponent<InputField>();
        // assign the listener
        newInputField.onEndEdit.AddListener(delegate { HandleWorkoutNameChange(newInputField); });
    }

    ///<summary>User has pressed the button to add a new exercise </summary>
    void AddExercisesToList(string exerciseName, bool isNew, bool isTemplateExercise)
    {
        //spawn the new list item
        GameObject newItem = Instantiate(WorkoutNameItem, WorkoutGridList.transform);
        newItem.name = exerciseName;
        // give the input placeholder the initial name
        newItem.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = exerciseName;

        // grab the input
        InputField newInputField = newItem.transform.GetChild(0).GetComponent<InputField>();


        if (isNew)
        {
            // this is a new exercise for a workout
            if (isTemplateExercise)
            {
                // add the new data
                DataManager.instance.AddNewExerciseForSpecificWorkout(currentWorkoutTemplateName, exerciseName);
                // assign the listeners
                newInputField.onEndEdit.AddListener(delegate { HandleExerciseChangeForWorkout(newInputField, currentWorkoutTemplateName); });
                newItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveWorkoutExerciseHandler(newItem));
            }
            else
            {
                //this is a new exercise, add it to muscle groups
                DataManager.instance.AddNewExerciseForSpecificMuscleGroup(currentMuscleGroupIndex, exerciseName);
                // assign the listener
                newInputField.onEndEdit.AddListener(delegate { HandleExerciseChangeForMuscleGroup(newInputField, currentWorkoutTemplateName); });
                newItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveExerciseHandler(newItem));
            }

        }
        else //We have loaded exercises from a template for editing
        {
            if (isEditingWorkoutExercises)
            {
                // assign the listeners
                newInputField.onEndEdit.AddListener(delegate { HandleExerciseChangeForWorkout(newInputField, currentWorkoutTemplateName); });
                newItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveWorkoutExerciseHandler(newItem));
            }
            else
            {
                // assign the listeners
                newInputField.onEndEdit.AddListener(delegate { HandleExerciseChangeForMuscleGroup(newInputField, currentWorkoutTemplateName); });
                newItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveExerciseHandler(newItem));
            }

        }

    }

    public void HandleExerciseChangeForWorkout(InputField input, string workoutName)
    {
        // grab the new name string
        string newExerciseName = Utils.HandleCapitalCase(input.text);
        // grab the current name of this exercise
        string oldExerciseName = input.transform.parent.name;
        // update the input to store the new name
        input.text = newExerciseName;
        //and update the parent name
        input.transform.parent.name = newExerciseName;
        // push the data update
        DataManager.instance.UpdateExerciseDataForSpecificWorkout(workoutName, oldExerciseName, newExerciseName, input);
    }

    public void HandleExerciseChangeForMuscleGroup(InputField input, string workoutName)
    {
        // grab the new name string
        string newExerciseName = Utils.HandleCapitalCase(input.text);
        // grab the current name of this exercise
        string oldExerciseName = input.transform.parent.name;
        // update the input to store the new name
        input.text = newExerciseName;
        //and update the parent name
        input.transform.parent.name = newExerciseName;
        // push the data update
        DataManager.instance.UpdateExerciseDataToMuscleGroup(oldExerciseName, newExerciseName, input, currentMuscleGroupIndex);
    }

    public void HandleWorkoutNameChange(InputField input)
    {
        if (input.text.Length == 0)
            return;

        string newWorkoutName = input.text;
        string currentWorkoutName = input.transform.parent.name;
        input.text = Utils.HandleCapitalCase(input.text);
        input.transform.parent.name = input.text;
        DataManager.instance.UpdateWorkoutName(currentWorkoutName, newWorkoutName, input);
    }

    public void RemoveExerciseHandler(GameObject thisItem)
    {
        DataManager.instance.RemoveMuscleGroupExercise(currentMuscleGroupIndex, thisItem.name);
        Destroy(thisItem);
    }

    public void RemoveWorkoutHandler(GameObject thisItem)
    {
        DataManager.instance.RemoveWorkout(thisItem.name);
        Destroy(thisItem);
    }

    public void RemoveWorkoutExerciseHandler(GameObject thisItem)
    {
        DataManager.instance.RemoveWorkoutExercise(currentWorkoutTemplateName, thisItem.name);
        Destroy(thisItem);
    }


    ///<summary>Dynamically assigned method when the workout templates are added to the list
    /// Handles the loading of a workout's specific exercises for editing </summary>
    public void LoadWorkoutTemplate(GameObject templateObj)
    {

        //grab the name of this object, which also will be the name of the workout
        currentWorkoutTemplateName = templateObj.name;
        // we are loading exercises for this template
        isEditingWorkoutExercises = true;
        // clearout the current list
        ClearOutCurrentGridList();

        // we need to grab the exercises for this workout
        WorkoutTemplate currentTemplate = DataManager.instance.myWorkouts.Find((workout) => workout.name == currentWorkoutTemplateName);
        // and add them to the list
        foreach (var item in currentTemplate.list)
            AddExercisesToList(item, false, false);

        // update the template button to show that we can go back to the templates
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().text = "Return To Templates";
        // update the add new button to show that we can now add exercises explicitly to this workout
        AddTemplateExerciseBtn.text = $"Add New {currentWorkoutTemplateName} Exercise";
    }


    ///<summary>Called from the start screen dropdown when the user wants to load a template workout</summary>
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



    public void LoadSelectedMuscleGroup(int selectedIndex)
    {
        int dropDownOffset = selectedIndex - 1;
        currentMuscleGroupIndex = dropDownOffset;
        string dataNameToLoad = Utils.GetActiveMuscleGroupName(dropDownOffset);
        ClearOutCurrentGridList();

        MuscleGroup currentExercise = DataManager.instance.myMuscleGroups.Find((group) => group.name == Utils.GetActiveMuscleGroupName(dropDownOffset));
        foreach (var item in currentExercise.list)
            AddExercisesToList(item, false, false);

    }

    void ClearOutCurrentGridList()
    {
        // before we load anything, clear out what was there
        if (WorkoutGridList.transform.childCount > 0)
            for (int i = 0; i < WorkoutGridList.transform.childCount; i++)
                Destroy(WorkoutGridList.transform.GetChild(i).gameObject);
    }

    #region Color Updates
    public void HandleColorChangeFromUpdate(Text textToChange, bool successfulUpdate)
    {
        StartCoroutine(HandleColorChange(textToChange, successfulUpdate));
    }

    IEnumerator HandleColorChange(Text textToChange, bool successfulUpdate)
    {
        float timeElapsed = 0f;
        float totalTime = 1f;

        Color startColor = Color.green;
        Color endColor = textToChange.color;

        while (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime;
            textToChange.color = Color.Lerp(startColor, endColor, timeElapsed / totalTime);
            yield return null;
        }
    }

    public void HandleColorChangeFromSave(Image imageToChange)
    {
        StartCoroutine(HandleColorChange(imageToChange));
    }

    IEnumerator HandleColorChange(Image imageToChange)
    {
        float timeElapsed = 0f;
        float totalTime = 1f;

        Color startColor = Color.green;
        Color endColor = imageToChange.color;

        while (timeElapsed < totalTime)
        {
            timeElapsed += Time.deltaTime;
            imageToChange.color = Color.Lerp(startColor, endColor, timeElapsed / totalTime);
            yield return null;
        }
    }

    #endregion


}