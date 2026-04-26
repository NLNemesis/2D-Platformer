using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    #region Save/Load/Delete - Settings File
    //Save Settings File
    public static void SaveSettings(SaveGameController SGC)
    {
        Debug.Log("I saved Player's Settings");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Settings.exe";
        FileStream stream = new FileStream(path, FileMode.Create);

        Settings data = new Settings(SGC);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    //Load Settings File
    public static Settings LoadSettings()
    {
        string path = Application.persistentDataPath + "/Settings.exe";
        if (File.Exists(path))
        {
            Debug.Log("I loaded settings from " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Settings data = formatter.Deserialize(stream) as Settings;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Saved Settings not found " + path);
            return null;
        }
    }

    //Delete Settings File
    public static void DeleteSettings()
    {
        string path = Application.persistentDataPath + "/Settings.exe";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Settings File Deleted" + path);
        }
        else
        {
            Debug.Log("No Settings File");
        }
    }
    #endregion

    #region Save/Load/Delete - Progress File
    //Save Progress File
    public static void SaveProgress(SaveGameController SGC)
    {
        Debug.Log("I saved Player's Progress");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Progress.exe";
        FileStream stream = new FileStream(path, FileMode.Create);

        Progress data = new Progress(SGC);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    //Load Progress File
    public static Progress LoadProgress()
    {
        string path = Application.persistentDataPath + "/Progress.exe";
        if (File.Exists(path))
        {
            Debug.Log("I loaded progress from " + path);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Progress data = formatter.Deserialize(stream) as Progress;

            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Saved progress not found " + path);
            return null;
        }
    }

    //Delete Progress File
    public static void DeleteProgress()
    {
        string path = Application.persistentDataPath + "/Progress.exe";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Progress File Deleted" + path);
        }
        else
        {
            Debug.Log("No Progress File");
        }
    }
    #endregion
}
