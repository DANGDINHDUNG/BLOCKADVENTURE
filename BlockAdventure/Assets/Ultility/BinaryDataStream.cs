using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryDataStream
{
    public static void Save<T>(T serializeObject, string filename)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path + filename + ".dat", FileMode.Create);

        try
        {
            formatter.Serialize(stream, serializeObject);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to save data to " + path + filename + ".dat\n" + e.Message);
        }
        finally
        {
            stream.Close();
        }
    }

    public static bool Exist(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        string fullFileName = fileName + ".dat";
        return File.Exists(path + fullFileName);
    }

    public static T Read<T>(string filename)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path + filename + ".dat", FileMode.Open);
        T returnType = default(T);

        try
        {
            returnType = (T)formatter.Deserialize(stream);
        }
        catch (SerializationException e)
        {
            Debug.Log("Failed to read data from " + path + filename + ".dat\n" + e.Message);
        }
        finally
        {
            stream.Close();
        }

        return returnType;
    }
}
