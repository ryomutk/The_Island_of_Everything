using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public static class JsonHelper
{
    static string dataPath = Application.dataPath + "/JsonData/";

    public static bool SaveData<T>(T data, string key, string name = null)
    {
        string json = JsonUtility.ToJson(data);
        string dirPath = dataPath + typeof(T).Name;

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        if (name == null)
        {
            name = typeof(T).Name;
        }

        string fullPath = dirPath + "/" + name;

        if (!File.Exists(fullPath))
        {
            File.Create(fullPath).Close();
        }

        var sw = new StreamWriter(fullPath, true);
        sw.WriteLine(key + ">" + json);
        sw.Flush();
        sw.Close();

        return true;
    }



    public static T GetData<T>(string key,string name = null)
    where T:class
    {
        if(name == null)
        {
            name = typeof(T).Name;
        }

        string dirPath = dataPath + typeof(T).Name;
        string fullPath = dirPath + "/" + name;

        
        if (!File.Exists(fullPath))
        {
            return null;
        }

        StreamReader sr = new StreamReader(fullPath);

        return FindDataByKey<T>(sr,key);
    }

    static T FindDataByKey<T>(StreamReader sr,string key)
    where T:class
    {
        bool result = false;
        do
        {
            result = true;
            var line = sr.ReadLine();

            if(line == null)
            {
                break;
            }

            for(int i = 0;i < key.Length;i++)
            {
                if(line[i] != key[i])
                {
                    result = false;
                    break;
                }
            }
            
            if(result)
            {
                line = line.Replace(key+">","");
                sr.Close();
                Debug.Log(line);
                return JsonUtility.FromJson<T>(line);
            }

        }while(true);

        sr.Close();
        return null;
    }
}