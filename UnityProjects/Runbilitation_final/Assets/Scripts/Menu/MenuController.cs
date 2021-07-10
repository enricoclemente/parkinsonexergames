using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public string SceneMain;
    public string SceneStart;
    public string SceneLevel1;
    public string SceneLevel2;
    public string SceneLevel3;
    public string SceneActual;
    public string SceneNext;

    public GameObject menu;
    public GameObject loadingInterface;
    public Image loadingProgressBar;
    public GameObject chooseLevelMenu;
    public GameObject pauseMenu;
    public GameObject statisticsInterface;
    public GameObject sampleScoreText;

    List<GameObject> scoreTexts = new List<GameObject>();

    [SerializeField] public AudioClip buttonAudio;
    AudioSource menuAudio;
      
    private void Start()
    {
        menuAudio = GetComponent<AudioSource>();
        menuAudio.clip = buttonAudio;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //se premo Esc metto in pausa
        {
            menuAudio.Play();
            AudioListener.pause = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;    //la speed si ferma
        }
    }    

    IEnumerator LoadAsyncScene(string sceneName)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        float totalProgress = 0;

        // wait 3 seconds before loading the level
        for(int i=0; i<6; i++)
        {
            totalProgress += 0.15f;
            loadingProgressBar.fillAmount = totalProgress;
            // loadingProgressNumber.text = "" + totalProgress;
            yield return new WaitForSeconds(0.5f);
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            totalProgress += asyncLoad.progress * 0.1f;
            loadingProgressBar.fillAmount = totalProgress;
            //loadingProgressNumber.text = "" + totalProgress;
            yield return null;
        }
    }


    public void ResumeLevel()
    {
        menuAudio.Play();
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; //basic speed
    }

    public void RetryLevel()
    {
        menuAudio.Play();
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneActual);
    }

    public void BackToMainMenu()
    {
        menuAudio.Play();
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneMain);
    }

    public void StartGame()
    {
        menuAudio.Play();
        menu.SetActive(false);
        loadingInterface.SetActive(true);
        StartCoroutine(LoadAsyncScene(SceneStart));
    }

    public void OpenChooseLevel()
    {
        menuAudio.Play();
        menu.SetActive(false);
        chooseLevelMenu.SetActive(true);
    }

    public void BackFromChooseLevel()
    {
        menuAudio.Play();
        menu.SetActive(true);
        chooseLevelMenu.SetActive(false);
    }

    public void StartLevel1()
    {
        menuAudio.Play();
        menu.SetActive(false);
        loadingInterface.SetActive(true);
        chooseLevelMenu.SetActive(false);
        StartCoroutine(LoadAsyncScene(SceneLevel1));
    }

    public void StartLevel2()
    {
        menuAudio.Play();
        menu.SetActive(false);
        loadingInterface.SetActive(true);
        chooseLevelMenu.SetActive(false);
        StartCoroutine(LoadAsyncScene(SceneLevel2));
    }

    public void StartLevel3()
    {
        menuAudio.Play();
        menu.SetActive(false);
        loadingInterface.SetActive(true);
        chooseLevelMenu.SetActive(false);
        StartCoroutine(LoadAsyncScene(SceneLevel3));
    }

    public void StartNextLevel()
    {
        menuAudio.Play();
        AudioListener.pause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneNext);
    }

    public void OpenStatistics()
    {
        menuAudio.Play();
        ScoreStatistics statistics = new ScoreStatistics();
        statistics.LoadScores();
        List<Tuple<string, int>> level1Scores = statistics.GetLastLevel1Scores();
        List<Tuple<string, int>> level2Scores = statistics.GetLastLevel2Scores();
        List<Tuple<string, int>> level3Scores = statistics.GetLastLevel3Scores();

        float level1StartX = statisticsInterface.transform.Find("Panel")
            .transform.Find("Level1").GetComponent<RectTransform>().anchoredPosition.x + 60;
        Debug.Log("X: "+level1StartX);
        float level1StartY = statisticsInterface.transform.Find("Panel")
            .transform.Find("Level1").GetComponent<RectTransform>().anchoredPosition.y - 10;
        float level2StartX = statisticsInterface.transform.Find("Panel")
            .transform.Find("Level2").GetComponent<RectTransform>().anchoredPosition.x + 60;
        float level2StartY = statisticsInterface.transform.Find("Panel")
            .transform.Find("Level2").GetComponent<RectTransform>().anchoredPosition.y - 10;
        float level3StartX = statisticsInterface.transform.Find("Panel")
            .transform.Find("Level3").GetComponent<RectTransform>().anchoredPosition.x + 60;
        float level3StartY = statisticsInterface.transform.Find("Panel")
            .transform.Find("Level3").GetComponent<RectTransform>().anchoredPosition.y - 10;

        for(int i=0; i<level1Scores.Count; i++)
        {
            GameObject scoreText;
            scoreText = Instantiate(sampleScoreText as GameObject);
            scoreText.transform.SetParent(statisticsInterface.transform.Find("Panel").transform);
            scoreText.GetComponent<RectTransform>().anchoredPosition = new Vector2(level1StartX, level1StartY - ((i+1) * 30));
            scoreText.GetComponent<TMP_Text>().text = level1Scores[i].Item1 + " " + level1Scores[i].Item2;
            scoreTexts.Add(scoreText);
        }

        for (int i = 0; i < level2Scores.Count; i++)
        {
            GameObject scoreText;
            scoreText = Instantiate(sampleScoreText as GameObject);
            scoreText.transform.SetParent(statisticsInterface.transform.Find("Panel").transform);
            scoreText.GetComponent<RectTransform>().anchoredPosition = new Vector2(level2StartX, level2StartY - ((i + 1) * 30));
            scoreText.GetComponent<TMP_Text>().text = level2Scores[i].Item1 + " " + level2Scores[i].Item2;
            scoreTexts.Add(scoreText);
        }

        for (int i = 0; i < level3Scores.Count; i++)
        {
            GameObject scoreText;
            scoreText = Instantiate(sampleScoreText as GameObject);
            scoreText.transform.SetParent(statisticsInterface.transform.Find("Panel").transform);
            scoreText.GetComponent<RectTransform>().anchoredPosition = new Vector2(level3StartX, level3StartY - ((i + 1) * 30));
            scoreText.GetComponent<TMP_Text>().text = level3Scores[i].Item1 + " " + level3Scores[i].Item2;
            scoreTexts.Add(scoreText);
        }
        
        menu.SetActive(false);
        statisticsInterface.SetActive(true);
    }

    public void BackFromStatistics()
    {
        menuAudio.Play();
        menu.SetActive(true);
        statisticsInterface.SetActive(false);
    }

    public void QuitGame()
    {
        menuAudio.Play();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        //SceneManager.LoadScene(SceneQuit);
        //Time.timeScale = 1f; //basic speed
    }

}
