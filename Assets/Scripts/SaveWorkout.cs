using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveWorkout : MonoBehaviour
{

    // void Start()
    // {
    //     SaveFile();
    //     LoadFile();
    // }

    public static bool CheckFirstTimeData()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        return File.Exists(destination);
    }

    public static void SaveFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        print("Saving file to: " + Application.persistentDataPath);
        FileStream file;

        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        WorkoutSaveData data = new WorkoutSaveData(DataManager.instance.myWorkouts.ToArray(), DataManager.instance.myMuscleGroups.ToArray());
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }

    public static WorkoutSaveData LoadFile()
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        WorkoutSaveData data = (WorkoutSaveData)bf.Deserialize(file);
        file.Close();
        return data;
    }


}