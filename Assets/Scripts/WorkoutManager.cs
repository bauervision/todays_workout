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
    public GameObject rearrangeHint;
    public GameObject finishScreen;

    public InputField Input;
    public Text TimerText;
    public Text StartText;
    public Text FinalTime;
    private bool hasStarted = false;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1);
    private int workoutTime = 0;
    private int trainingMinutes = 0;
    private int trainingSeconds = 0;

    public void ShowWorkoutInput()
    {
        AddButton.SetActive(false);
    }

    public void AddNewWorkout(string workout)
    {
        if (workout != null)
        {
            if (GridList.transform.childCount > 2 && !rearrangeHint.activeInHierarchy)
                rearrangeHint.SetActive(true);

            GameObject newWorkout = Instantiate(workoutPrefab, GridList.transform);
            newWorkout.transform.GetChild(0).GetComponent<Text>().text = workout;

            //Input.ActivateInputField();
            Input.placeholder.GetComponent<Text>().text = "Add another?";
            Input.text = string.Empty;
        }
    }

    public void DoneWithSetup()
    {
        Inputs.SetActive(false);
    }

    public void StartTimer()
    {
        hasStarted = !hasStarted;

        if (hasStarted)
        {
            StartText.text = "Stop";
            rearrangeHint.SetActive(false);
            StartCoroutine(Timer());
        }
        else
        {

            StopAllCoroutines();
            finishScreen.SetActive(true);
            FinalTime.text = ProcessWorkoutTime(workoutTime - 1);
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
