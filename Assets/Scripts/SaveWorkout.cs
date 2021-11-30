using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveWorkout : MonoBehaviour
{

    void Start()
    {
        SaveFile();
        LoadFile();
    }

    public void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        WorkoutSaveData data = new WorkoutSaveData(DataManager.instance.myWorkouts.ToArray(), DataManager.instance.myExercises.ToArray());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        WorkoutSaveData data = (WorkoutSaveData)bf.Deserialize(file);
        file.Close();

        DataManager.instance.myWorkouts = data.myWorkouts.ToList();
        DataManager.instance.myExercises = data.myExercises.ToList();

    }


}