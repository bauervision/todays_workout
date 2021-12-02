
using System.Collections.Generic;

[System.Serializable]
///<summary>This data holds the name of a muscle group, and all of the exercises associated with it</summary>
public class MuscleGroup
{
    public string name;
    public string[] list;

    public MuscleGroup()
    {
        name = string.Empty;
        list = new string[] { };
    }

    public MuscleGroup(string groupName, string[] exerciseList)
    {
        name = groupName;

        List<string> capitalizedList = new List<string>();
        foreach (string item in exerciseList)
            capitalizedList.Add(Utils.HandleCapitalCase(item));

        list = capitalizedList.ToArray();

    }

    public MuscleGroup(string groupName)
    {
        name = groupName;
        list = new string[] { "First Exercise" };
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
        List<string> capitalizedList = new List<string>();
        foreach (string item in exerciseList)
            capitalizedList.Add(Utils.HandleCapitalCase(item));

        list = capitalizedList.ToArray();
    }

    public WorkoutTemplate(string exerciseName)
    {
        name = exerciseName;
        list = new string[] { "First Workout" };
    }
}

///<summary>Contains all of the current save data for the user including exercises and workout templates.  </summary>
[System.Serializable]
public class WorkoutSaveData
{
    public WorkoutTemplate[] myWorkouts;
    public MuscleGroup[] myMuscleGroups;

    public WorkoutSaveData(WorkoutTemplate[] currentWorkouts, MuscleGroup[] currentGroups)
    {
        myWorkouts = currentWorkouts;
        myMuscleGroups = currentGroups;
    }
}