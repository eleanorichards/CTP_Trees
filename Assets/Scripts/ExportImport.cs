using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class ExportImport : MonoBehaviour
{
    public void ExportLevel()
    {
        ////string destination = Application.persistentDataPath + "/save.dat";
        //FileStream file;
        //map = mapInfo.GetMap();
        //string destination = Application.persistentDataPath + "/";
        //string fileName = destination + levelName.text;

        //if (File.Exists(fileName))
        //{
        //    Debug.Log(fileName + " already exists.");
        //    return;
        //}
        //else if (fileName == "")
        //{
        //    Debug.Log("Please enter a file name");
        //    return;
        //}

        //file = File.Create(fileName);

        //data = new GameData(map, fileName, mapInfo.width, mapInfo.height);
        //BinaryFormatter bf = new BinaryFormatter();
        //bf.Serialize(file, data);
        //file.Close();
    }

    public void LoadFile()
    {
        //FileStream file;
        //int[,] map;
        //string destination = Application.persistentDataPath;
        //string fileName = destination + levelName.text;
        //if (File.Exists(fileName)) file = File.OpenRead(fileName);
        //else
        //{
        //    Debug.LogError("File not found");
        //    return;
        //}

        //BinaryFormatter bf = new BinaryFormatter();
        //GameData data = (GameData)bf.Deserialize(file);
        //file.Close();

        //map = data.map;
        //mapInfo.SetMapDimensions(data.width, data.height);
        //mapInfo.SetMap(map);

        //string currentName = data.fileName;

        //Debug.Log(data.fileName);
        //Debug.Log(data.map.Length);
    }
}