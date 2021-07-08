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
