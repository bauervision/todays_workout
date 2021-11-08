using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkoutManager : MonoBehaviour
{
    public GameObject GridList;
    public GameObject AddButton;
    public GameObject Inputs;
    public GameObject workoutPrefab;

    public InputField Input;
    public Text TimerText;

    private List<GameObject> workouts = new List<GameObject>();

    public void ShowWorkoutInput()
    {
        AddButton.SetActive(false);
    }

    public void AddNewWorkout(string workout)
    {
        GameObject newWorkout = Instantiate(workoutPrefab, GridList.transform);
        newWorkout.transform.GetChild(0).GetComponent<Text>().text = workout;

        Input.placeholder.GetComponent<Text>().text = "Add another?";
        Input.text = string.Empty;
    }

    public void DoneWithSetup()
    {
        Inputs.SetActive(false);
    }

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    private int workoutTime = 0;
    private int trainingMinutes = 0;
    private int trainingSeconds = 0;

    IEnumerator Timer()
    {
        print("Timer started");

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
