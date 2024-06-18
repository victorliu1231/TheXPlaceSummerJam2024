using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static int numSaveFiles = 1;

    // `ref` keyword to make sure saveData is passed by reference
    public static void readFile(ref SaveData saveData, int saveIndex)
    {
        // Generate the pathway to get the data
        string saveFile = getSaveFilePath(saveIndex);
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data into a pattern matching the SaveData class.
            saveData = JsonUtility.FromJson<SaveData>(fileContents);
        }
    }

    public static void writeFile(ref SaveData saveData, int saveIndex)
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(saveData);

        // Generate the pathway to store the data
        string saveFile = getSaveFilePath(saveIndex);

        print(saveFile);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }

    // Deletes save file from local system
    public static void deleteSaveFile(int saveIndex){
        string saveFile = getSaveFilePath(saveIndex);
        File.Delete(saveFile);
        #if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
		#endif
    }

    // Get the filepath for this saveIndex
    public static string getSaveFilePath(int saveIndex)
    {
        // saveIndex 0 is autosave
        return Application.persistentDataPath + "/save" + saveIndex + "data.json";
    }
}