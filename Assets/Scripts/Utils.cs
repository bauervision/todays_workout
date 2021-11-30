using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static string HandleCapitalCase(string incomingString)
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

    public static string UppercaseFirst(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return string.Empty;
        }
        return char.ToUpper(s[0]) + s.Substring(1);
    }

    public static string[] GetActiveWorkout(int selectedItem)
    {
        return DataManager.instance.myWorkouts.ToArray().ElementAt(selectedItem).list;
    }

    public static string GetActiveWorkoutName(int selectedItem)
    {
        return DataManager.instance.myWorkouts.ToArray().ElementAt(selectedItem).name;
    }

    public static string[] GetActiveExercises(int selectedItem)
    {
        return DataManager.instance.myExercises.ToArray().ElementAt(selectedItem).list;
    }

    public static string GetActiveExerciseName(int selectedItem)
    {
        return DataManager.instance.myExercises.ToArray().ElementAt(selectedItem).name;
    }
}