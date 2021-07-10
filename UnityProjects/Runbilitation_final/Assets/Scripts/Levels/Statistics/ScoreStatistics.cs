using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreStatistics
{
    private List<Dictionary<string, object>> allLevelScores;
    private SortedList<int, Dictionary<string, object>> level1Scores = new SortedList<int, Dictionary<string, object>>();
    private SortedList<int, Dictionary<string, object>> level2Scores = new SortedList<int, Dictionary<string, object>>();
    private SortedList<int, Dictionary<string, object>> level3Scores = new SortedList<int, Dictionary<string, object>>();

    string filePath = "/Resources/Levels/Statistics/ScoreStatistics.csv";

    /*
    private void Start()
    {
        // CSVWriter.Write("/Resources/Levels/Statistics/ScoreStatistics.csv", 1, 500, 0);
        LoadScores();
        List<Tuple<string, int>> scores = GetTopLevel1Scores();

        for(int i=0; i<scores.Count; i++)
        {
            Debug.Log("date: " + scores[i].Item1 + " score: " + scores[i].Item2);
        }
    }
    */

    public void SaveScore(int level, int score, int coinCounter = 0)
    {
        CSVWriter.Write("/Resources/Levels/Statistics/ScoreStatistics.csv", level, score, coinCounter);
        Debug.Log("Ho scritto su file csv");
    }

    public void LoadScores()
    {
        allLevelScores = CSVReader.Read("Levels/Statistics/ScoreStatistics");

        List<string> columnList = new List<string>(allLevelScores[0].Keys);

        Debug.Log("Ci sono " + columnList.Count + " colonne nel CSV");

        foreach (string key in columnList)
            Debug.Log("Il nome della colonna è " + key);
        
        // read from csv file
        for(int i=0; i < allLevelScores.Count; i++)
        {
            // Debug.Log("date: " + (int)allLevelScores[i]["date"] + " score: " + (int)allLevelScores[i]["score"]);
            int date = (int)allLevelScores[i]["date"];
            if ((int)allLevelScores[i]["level"] == 1)
            {
                level1Scores.Add(date*10 + i, allLevelScores[i]);
            } else if((int)allLevelScores[i]["level"] == 2)
            {
                level2Scores.Add(date * 10 + i, allLevelScores[i]);
            } else if ((int)allLevelScores[i]["level"] == 3)
            {
                level3Scores.Add(date * 10 + i, allLevelScores[i]);
            }
        }
    }

    public List<Tuple<string, int>> GetLastLevel1Scores(int numberOfScores = 6)
    {
        List<Tuple<string, int>> scores = new List<Tuple<string, int>>();
        
        for (int i=level1Scores.Count-1; i >level1Scores.Count - numberOfScores -1; i--)
        {
            if (i == -1)
                return scores;
            int dateNumber = (int)level1Scores.Values[i]["date"];
            int year = dateNumber / 10000;
            int month = (dateNumber - year * 10000) / 100;
            int day = (dateNumber - year * 10000 - month * 100);
            string date = day + "-" + month + "-" + year;
            scores.Add(new Tuple<string, int>(date, (int)level1Scores.Values[i]["score"]));
        }

        return scores;
    }

    public List<Tuple<string, int>> GetLastLevel2Scores(int numberOfScores = 6)
    {
        List<Tuple<string, int>> scores = new List<Tuple<string, int>>();

        for (int i = level2Scores.Count - 1; i > level2Scores.Count - numberOfScores - 1; i--)
        {
            if (i == -1)
                return scores;
            int dateNumber = (int)level2Scores.Values[i]["date"];
            int year = dateNumber / 10000;
            int month = (dateNumber - year * 10000) / 100;
            int day = (dateNumber - year * 10000 - month * 100);
            string date = day + "-" + month + "-" + year;
            scores.Add(new Tuple<string, int>(date, (int)level2Scores.Values[i]["score"]));
        }

        return scores;
    }

    public List<Tuple<string, int>> GetLastLevel3Scores(int numberOfScores = 6)
    {
        List<Tuple<string, int>> scores = new List<Tuple<string, int>>();

        for (int i = level3Scores.Count - 1; i > level3Scores.Count - numberOfScores - 1; i--)
        {
            if (i == -1)
                return scores;
            int dateNumber = (int)level3Scores.Values[i]["date"];
            int year = dateNumber / 10000;
            int month = (dateNumber - year * 10000) / 100;
            int day = (dateNumber - year * 10000 - month * 100);
            string date = day + "-" + month + "-" + year;
            scores.Add(new Tuple<string, int>(date, (int)level3Scores.Values[i]["score"]));
        }

        return scores;
    }

}
