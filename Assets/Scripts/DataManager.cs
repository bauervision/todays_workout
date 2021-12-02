using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public Image saveIcon;


    public List<WorkoutTemplate> myWorkouts;
    public List<Exercise> myExercises;

    private void Start()
    {
        instance = this;
        // on start, check to see if we have saved data we need to load
        if (SaveWorkout.CheckFirstTimeData())
        {
            print("Save file detected! Loading Data...");
            LoadSavedData();
        }
        else
        {
            print("No file detected, Loading Default Data...");
            LoadDefaultData();
        }
    }

    ///<summary>Called from the UI: save button </smmary>
    public void SaveData()
    {
        print("Saving file...");
        SaveWorkout.SaveFile();
        UIManager.instance.HandleColorChangeFromSave(saveIcon);
    }

    private void LoadSavedData()
    {
        WorkoutSaveData loadedData = SaveWorkout.LoadFile();
        DataManager.instance.myWorkouts = loadedData.myWorkouts.ToList();
        DataManager.instance.myExercises = loadedData.myExercises.ToList();
        UIManager.instance.UpdateWorkoutDropDown();
    }


    private void LoadDefaultData()
    {
        // load all starting workouts
        myWorkouts.Add(new WorkoutTemplate("Abs", _Abs));
        myWorkouts.Add(new WorkoutTemplate("Arms", _Arms));
        myWorkouts.Add(new WorkoutTemplate("Back", _Back));
        myWorkouts.Add(new WorkoutTemplate("Cardio", _Cardio));
        myWorkouts.Add(new WorkoutTemplate("Chest", _Chest));
        myWorkouts.Add(new WorkoutTemplate("Legs", _Legs));
        myWorkouts.Add(new WorkoutTemplate("Mixed", _Mixed));
        myWorkouts.Add(new WorkoutTemplate("Shoulders", _Shoulders));
        // and load all starting exercises
        myExercises.Add(new Exercise("Abs", abs));
        myExercises.Add(new Exercise("Arms", arms));
        myExercises.Add(new Exercise("Back", back));
        myExercises.Add(new Exercise("Cardio", cardio));
        myExercises.Add(new Exercise("Chest", chest));
        myExercises.Add(new Exercise("Legs", legs));
        myExercises.Add(new Exercise("Shoulders", shoulders));
        // now handle the dropdowns
        UIManager.instance.UpdateWorkoutDropDown();
    }

    #region Workout Starter Templates
    string[] _Abs = new string[] { "ab bicyles", "heel taps", "plank", "jack knife", "flutter kicks", "hollow body hold", "toe touches", "mountain climbers" };
    string[] _Arms = new string[] { "hammer curls", "ab bicycle", "curls", "tricep ext", "alt curls", "knee to elbow plank", "concentration curls", "curl burnout" };
    string[] _Back = new string[] { "supermans", "plank", "bird dog", "side plank", "glute bridge", "reverse fly", "dumbell row", "deadlift" };
    string[] _Cardio = new string[] { "mountain climbers", "jump squat", "heel taps", "side 2 side", "high knees", "butt kickers", "burpees" };
    string[] _Chest = new string[] { "push ups", "tricep dips", "heel taps", "chest flies", "incline pushups", "plank", "chest abductions", "decline pushups" };
    string[] _Legs = new string[] { "squats", "lunge pulse", "ab bicycle", "burpees", "single leg bridge", "dead bug", "leg circles", "weighted lunges" };
    string[] _Mixed = new string[] { "burpees", "lunges", "heel taps", "pushups", "curls", "delt row", "tricep ext", "shoulder ext" };
    string[] _Shoulders = new string[] { "UpFrontBack", "heel taps", "reverse flies", "mountain climber", "delt row", "side extensions", "row" };

    #endregion

    #region Starting core exercises
    [HideInInspector]
    public string[] abs = new string[] { "ab bicyles", "heel taps", "plank", "jack knife", "flutter kicks", "hollow body hold", "toe touches", "reach over crunch" };
    [HideInInspector]
    public string[] arms = new string[] { "hammer curls", "curls", "tricep ext", "alt curls", "concentration curls", "curl burnout" };
    [HideInInspector]
    public string[] back = new string[] { "supermans", "bird dog", "side plank", "glute bridge", "dumbell row", "deadlift", "row" };
    [HideInInspector]
    public string[] cardio = new string[] { "mountain climbers", "jump squat", "side 2 side", "high knees", "butt kickers", "burpees" };
    [HideInInspector]
    public string[] chest = new string[] { "push ups", "chest flies", "incline pushups", "chest abductions", "decline pushups", "front pull downs" };
    [HideInInspector]
    public string[] legs = new string[] { "squats", "lunge pulse", "single leg bridge", "dead bug", "leg circles", "weighted lunges", "jump squat", "squat pulses" };
    [HideInInspector]
    public string[] shoulders = new string[] { "UpFrontBack", "reverse flies", "delt row", "side extensions", "row", "side plank" };
    #endregion


    public void AddNewWorkoutTemplate(string workoutName)
    {
        myWorkouts.Add(new WorkoutTemplate(workoutName));
    }

    public void AddNewExercise(string exerciseName)
    {
        myWorkouts.Add(new WorkoutTemplate(exerciseName));
    }

    public void AddNewExerciseForSpecificWorkout(string workoutName, string newExercise)
    {
        print("Updating: " + workoutName + " with new exercise name: " + newExercise);
        // find current workout data template
        DataManager.instance.myWorkouts.Find((workout) => workout.name == workoutName).list.ToList().Add(newExercise);
        // grab the list of exercises for this workout
        //string[] listToUpdate = currentWorkout.list;

        //string[] updatedList = DataManager.instance.myWorkouts.Find((workout) => workout.name == currentTemplate).list;
        //print("Added new Exercise..." + updatedList.ToString());
        //updatedList.ToList().Add(exerciseName);
        // now update the workout data
        //DataManager.instance.myWorkouts.Find((workout) => workout.name == currentTemplate).list = updatedList;
    }

    ///<summary>When the user has made a text change for a specific exercise within a workout, update the data. If the updated name isn't found, it will add it to the data </summary>
    public void UpdateExerciseDataForSpecificWorkout(string workoutName, string oldExerciseName, string newExerciseName, InputField textToUpdate)
    {
        // find current workout data template
        int workoutIndexToUpdate = DataManager.instance.myWorkouts.FindIndex((workout) => workout.name == workoutName);
        // make sure we found it
        if (workoutIndexToUpdate != -1)
        {
            int exerciseIndexToUpdate = DataManager.instance.myWorkouts[workoutIndexToUpdate].list.ToList().FindIndex(exercise => exercise == oldExerciseName);
            // make sure we found it
            if (exerciseIndexToUpdate != -1)
            {
                DataManager.instance.myWorkouts[workoutIndexToUpdate].list[exerciseIndexToUpdate] = newExerciseName;
                UIManager.instance.HandleColorChangeFromUpdate(textToUpdate.transform.GetChild(2).transform.GetComponent<Text>(), true);
            }
            else
            {
                // couldnt find this exercise in the current data, so we add it
                string[] updateList = DataManager.instance.myWorkouts[workoutIndexToUpdate].list;
                List<string> newList = updateList.ToList();
                newList.Add(newExerciseName);
                DataManager.instance.myWorkouts[workoutIndexToUpdate].list = newList.ToArray();
                UIManager.instance.HandleColorChangeFromUpdate(textToUpdate.transform.GetChild(2).transform.GetComponent<Text>(), true);
            }

            UIManager.instance.UpdateWorkoutDropDown();
        }
        else
        {
            Debug.LogError("UpdateExerciseDataForSpecificWorkout | Couldnt find: " + workoutName + " in the data");
            UIManager.instance.HandleColorChangeFromUpdate(textToUpdate.transform.GetChild(2).transform.GetComponent<Text>(), false);
        }

    }

    public void AddExerciseDataToMuscleGroup(string muscleGroupName, string newExercise)
    {
        print("Adding New Exercise: " + newExercise + " to muscle group " + muscleGroupName);

        // add new data to the list of this workout
        //string[] updatedList = DataManager.instance.myWorkouts.Find((workout) => workout.name == currentTemplate).list;
        //print("Added new Exercise..." + updatedList.ToString());
        //updatedList.ToList().Add(exerciseName);
        // now update the workout data
        //DataManager.instance.myWorkouts.Find((workout) => workout.name == currentTemplate).list = updatedList;
    }


    ///<summary>Called from UIManager when the user has changed the name of a workout template, includes color changing confirmation </summary>
    public void UpdateWorkoutName(string oldWorkoutName, string newWorkoutName, InputField textToUpdate)
    {
        int workoutIndexToUpdate = DataManager.instance.myWorkouts.FindIndex((workout) => workout.name == oldWorkoutName);
        // make sure we found it
        if (workoutIndexToUpdate != -1)
        {
            DataManager.instance.myWorkouts[workoutIndexToUpdate].name = newWorkoutName;
            UIManager.instance.UpdateWorkoutDropDown();
            UIManager.instance.HandleColorChangeFromUpdate(textToUpdate.transform.GetChild(2).transform.GetComponent<Text>(), true);
        }
        else
        {
            Debug.LogError("UpdateWorkoutName | Couldnt find: " + oldWorkoutName + " in the data");
            UIManager.instance.HandleColorChangeFromUpdate(textToUpdate.transform.GetChild(2).transform.GetComponent<Text>(), false);
        }

    }


    ///<summary>Called from UIManager when the user has decided to remove an entire workout routine </summary>
    public void RemoveWorkout(string workoutName)
    {
        int workoutIndexToUpdate = DataManager.instance.myWorkouts.FindIndex((workout) => workout.name == workoutName);
        // make sure we found it
        if (workoutIndexToUpdate != -1)
        {
            DataManager.instance.myWorkouts.RemoveAt(workoutIndexToUpdate);
            UIManager.instance.UpdateWorkoutDropDown();
        }
    }

    ///<summary>Called from UIManager when the user has decided to remove a specific exercise from a workout </summary>
    public void RemoveWorkoutExercise(string workoutName, string exerciseName)
    {
        int workoutIndexToUpdate = DataManager.instance.myWorkouts.FindIndex((workout) => workout.name == workoutName);
        // make sure we found it
        if (workoutIndexToUpdate != -1)
        {
            int exerciseIndexToUpdate = DataManager.instance.myWorkouts[workoutIndexToUpdate].list.ToList().FindIndex(exercise => exercise == exerciseName);
            // make sure we found it
            if (exerciseIndexToUpdate != -1)
            {
                string[] updateList = DataManager.instance.myWorkouts[workoutIndexToUpdate].list;
                List<string> newList = updateList.ToList();
                newList.RemoveAt(exerciseIndexToUpdate);
                DataManager.instance.myWorkouts[workoutIndexToUpdate].list = newList.ToArray();
            }
            else
                Debug.LogError("Didnt find exercise: " + exerciseName + " in " + workoutName);
        }
        else
            Debug.LogError("Didnt find workout: " + workoutName);
    }
}