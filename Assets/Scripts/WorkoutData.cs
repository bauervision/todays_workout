
[System.Serializable]
///<summary>These are all of the exercises for one targeted muscle group </summary>
public class Exercise
{
    public string name;
    public string[] list;

    public Exercise()
    {
        name = string.Empty;
        list = new string[] { };
    }

    public Exercise(string exerciseName, string[] exerciseList)
    {
        name = exerciseName;
        list = exerciseList;
    }
}


[System.Serializable]
///<summary>These contains groups of exercises designed to be executed during one round of a workout  </summary>
public class WorkoutTemplate
{
    public string name;
    public string[] list;

    public WorkoutTemplate()
    {
        name = string.Empty;
        list = new string[] { };
    }

    public WorkoutTemplate(string exerciseName, string[] exerciseList)
    {
        name = exerciseName;
        list = exerciseList;
    }
}

///<summary>Contains all of the current save data for the user including exercises and workout templates.  </summary>
[System.Serializable]
public class WorkoutSaveData
{
    public WorkoutTemplate[] myWorkouts;
    public Exercise[] myExercises;

    public WorkoutSaveData(WorkoutTemplate[] currentWorkouts, Exercise[] currentExercises)
    {
        myWorkouts = currentWorkouts;
        myExercises = currentExercises;
    }
}