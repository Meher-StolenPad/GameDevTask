using System;
using System.IO;
using UnityEngine;

namespace Davanci
{
    public static class SaveSystem
    {
        private const string FolderName = "Saves";

        public static void DeleteSave(string fileName)
        {
            // Append .json extension to the filename
            string fileWithExtension = fileName + ".json";


            string directoryPath = Path.Combine(Application.persistentDataPath, FolderName);

            string filePath = Path.Combine(directoryPath, fileWithExtension);

            if (Directory.Exists(directoryPath))
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                }
            }
        }


        public static void Save<T>(string fileName, T data)
        {
            string directoryPath = Path.Combine(Application.persistentDataPath, FolderName);
            string filePath = Path.Combine(directoryPath, fileName + ".json");

            try
            {
                // Create the directory if it doesn't exist
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Convert data to JSON string
                string json = JsonUtility.ToJson(data);

                // Write JSON string to file
                File.WriteAllText(filePath, json);
                Debug.Log(filePath);

            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save data to file: " + filePath + "\nError: " + e.Message);
            }
        }
        public static bool Load<T>(string fileName, T defaultData, out T returnValue)
        {
            string directoryPath = Path.Combine(Application.persistentDataPath, FolderName);
            string filePath = Path.Combine(directoryPath, fileName + ".json");

            if (File.Exists(filePath))
            {
                try
                {
                    // Read JSON string from file
                    string json = File.ReadAllText(filePath);

                    // Deserialize JSON string to object of type T
                    returnValue = JsonUtility.FromJson<T>(json);
                    return true;

                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to load data from file: " + filePath + "\nError: " + e.Message);
                    returnValue = defaultData;
                    return false;
                }
            }
            else
            {
                returnValue = defaultData;
                return false;
            }
        }
    }
}
