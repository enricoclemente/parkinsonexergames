using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CSVWriter
{
    public static void Write(string filePath, int level, int score, int coinCounter)
    {
        //string filePath = getPath();
        filePath = Application.dataPath + filePath;
        StreamWriter outStream;
        string delimiter = ",";
        string date = getActualDate();

        if (!File.Exists(filePath))
        {
            outStream = File.CreateText(filePath);
            outStream.WriteLine("level,date,score,coin");
            outStream.Close();
        }

        outStream = File.AppendText(filePath);
        outStream.WriteLine(string.Join(delimiter, level, date, score));
        outStream.Close();

    }

    public static string getActualDate()
    {
        string finalDate = "";
        DateTime now = DateTime.UtcNow;


        finalDate = now.Date.ToString("yyyyMMdd");
        //string date = now.Date.ToString();
        //Debug.Log(date);

        return finalDate;
    }


    // Following method is used to retrive the relative path as device platform
    public static string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/Resources/Levels/Statistics/"+ "ScoreStatistics.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath + "/" + "Saved_data.csv";
#endif
    }

}
