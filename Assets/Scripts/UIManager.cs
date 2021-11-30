using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Game Objects")]
    public GameObject StartScreen;
    public GameObject MainScreen;
    public GameObject EditScreen;
    public GameObject WorkoutScreen;
    public GameObject FinishScreen;

    [Header("Buttons")]
    public Button TemplateBtn;
    public Button ExerciseBtn;
    public Text AddTemplateExerciseBtn;

    [Header("Spawner")]
    public GameObject WorkoutGridList;
    public GameObject TemplateBtnItem;
    public GameObject WorkoutNameItem;


    bool isEditingTemplates = false;

    private void Start()
    {
        instance = this;

        MainScreen.SetActive(false);
        EditScreen.SetActive(false);
        StartScreen.SetActive(true);
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
        isEditingTemplates = true;
        AddTemplateExerciseBtn.text = "+ Templates";
        TemplateBtn.Select();
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Bold;
        //change non selected
        ExerciseBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Normal;
    }

    public void ExerciseButtonSelected()
    {
        isEditingTemplates = false;
        AddTemplateExerciseBtn.text = "+ Exercises";
        ExerciseBtn.Select();
        ExerciseBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Bold;
        //change non selected
        TemplateBtn.transform.GetChild(1).transform.GetComponent<Text>().fontStyle = FontStyle.Normal;
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
            AddTemplateToList("New Template");
        else
            AddExercisesToList("New Exercise");


    }

    void AddTemplateToList(string templateName)
    {
        GameObject newItem = Instantiate(TemplateBtnItem, WorkoutGridList.transform);
        // give the input placeholder a starting text
        newItem.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = templateName;
        // assign the button listeners
        newItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => LoadTemplate(newItem));
        newItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RemoveItemHandler(newItem));
        // grab the input
        InputField newInputField = newItem.transform.GetChild(1).GetComponent<InputField>();
        // assign the listner
        newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
    }

    void AddExercisesToList(string exerciseName)
    {
        GameObject newItem = Instantiate(WorkoutNameItem, WorkoutGridList.transform);
        // give the input placeholder a starting text
        newItem.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = exerciseName;
        // assign the button listner
        newItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => RemoveItemHandler(newItem));
        // grab the input
        InputField newInputField = newItem.transform.GetChild(0).GetComponent<InputField>();
        // assign the listner
        newInputField.onEndEdit.AddListener(delegate { HandleTextChange(newInputField); });
    }
    public void HandleTextChange(InputField input)
    {
        input.text = Utils.HandleCapitalCase(input.text);
        input.transform.parent.name = input.text;
    }

    public void RemoveItemHandler(GameObject thisItem)
    {
        Destroy(thisItem);
    }

    public void LoadTemplate(GameObject templateObj)
    {
        LoadTemplateExercises(templateObj.name);
    }

    void LoadTemplateExercises(string templateName)
    {
        print("Load Template for: " + templateName);
    }

    public void LoadSelectedEditData(int selectedIndex)
    {
        int dropDownOffset = selectedIndex + 1;
        string dataNameToLoad = (isEditingTemplates ? Utils.GetActiveWorkoutName(dropDownOffset) : Utils.GetActiveExerciseName(dropDownOffset));
        int dataCount = (isEditingTemplates ? Utils.GetActiveWorkout(dropDownOffset).Length : Utils.GetActiveExercises(dropDownOffset).Length);
        ClearOutCurrentGridList();

        if (isEditingTemplates)
        {
            // first thing we need to get the specific workout to load
            WorkoutTemplate currentTemplate = DataManager.instance.myWorkouts.Find((workout) => workout.name == Utils.GetActiveWorkoutName(dropDownOffset));
            foreach (var item in currentTemplate.list)
                AddTemplateToList(item);
        }
        else
        {
            Exercise currentExercise = DataManager.instance.myExercises.Find((exercise) => exercise.name == Utils.GetActiveExerciseName(dropDownOffset));
            foreach (var item in currentExercise.list)
                AddTemplateToList(item);
        }


    }

    void ClearOutCurrentGridList()
    {
        // before we load anything, clear out what was there
        if (WorkoutGridList.transform.childCount > 0)
            for (int i = 0; i < WorkoutGridList.transform.childCount; i++)
                Destroy(WorkoutGridList.transform.GetChild(i).gameObject);
    }

    public void LoadAllTemplates()
    {
        ClearOutCurrentGridList();
        // now we can load
        foreach (WorkoutTemplate template in DataManager.instance.myWorkouts)
            AddTemplateToList(template.name);
    }

    public void LoadAllExercises()
    {
        ClearOutCurrentGridList();
        foreach (Exercise exercise in DataManager.instance.myExercises)
            AddTemplateToList(exercise.name);
    }
}