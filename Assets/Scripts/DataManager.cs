using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public List<WorkoutTemplate> myWorkouts;
    public List<Exercise> myExercises;

    private void Start()
    {
        instance = this;
        // on start, check to see if we have saved data we need to load

        // we don't have any saved data so start with default data
        LoadDefaultData();

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
    public string[] abs = new string[] { "ab bicyles", "heel taps", "plank", "jack knife", "flutter kicks", "hollow body hold", "toe touches", "reach over crunch" };
    public string[] arms = new string[] { "hammer curls", "curls", "tricep ext", "alt curls", "concentration curls", "curl burnout" };
    public string[] back = new string[] { "supermans", "bird dog", "side plank", "glute bridge", "dumbell row", "deadlift", "row" };
    public string[] cardio = new string[] { "mountain climbers", "jump squat", "side 2 side", "high knees", "butt kickers", "burpees" };
    public string[] chest = new string[] { "push ups", "chest flies", "incline pushups", "chest abductions", "decline pushups", "front pull downs" };
    public string[] legs = new string[] { "squats", "lunge pulse", "single leg bridge", "dead bug", "leg circles", "weighted lunges", "jump squat", "squat pulses" };
    public string[] shoulders = new string[] { "UpFrontBack", "reverse flies", "delt row", "side extensions", "row", "side plank" };
    #endregion



}