using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScore : MonoBehaviour
{
    public int levelNumber = 1;
    int totalScore = 0;
    int coinCounter = 0;
    //int obstacleCounter = 0;

    [SerializeField] public AudioClip runningSound;
    [SerializeField] public AudioClip coinSound;
    [SerializeField] public AudioClip obstacleSound;

    AudioSource [] playingSounds;
    public GameObject actualScoreCanvas;
    public TMP_Text actualScoreText;

    public GameObject finalScoreCanvas;
    public TMP_Text finalScoreText;
    public TMP_Text finalCoinCountText;

    public GameObject debugCanvas;


    // Start is called before the first frame update
    void Start()
    {
        actualScoreText.text = "Score: " + totalScore;
        playingSounds = GetComponents<AudioSource>();
        playingSounds[1].clip = runningSound;
        playingSounds[1].Play();
    }
    
    void OnTriggerEnter(Collider c)
    {
        Coin coin = c.gameObject.GetComponent<Coin>();
        if (coin != null)
        {   // a coin was hit
            // play coin sound 
            playingSounds[0].clip = coinSound;
            playingSounds[0].Play();
            // update score
            coinCounter++;
            totalScore += coin.GetCoinValue();
            Destroy(coin.gameObject);

            // Debug.Log("Total score: " + totalScore);
            actualScoreText.text = "Score: " + totalScore;
            
        }

        Obstacle obstacle = c.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {   // an obstacle was hit
            // play obstacle sound
            playingSounds[0].clip = obstacleSound;
            playingSounds[0].Play();
            // update score
            // obstacleCounter++;
            totalScore -= obstacle.GetObstacleValue();
            // Debug.Log("Total score: " + totalScore);
            actualScoreText.text = "Score: " + totalScore;
        }

        /*
        StartLevel startLevel = c.gameObject.GetComponent<StartLevel>();
        if(startLevel != null)
        {
            Time.timeScale = 1f;
        }*/

        FinishLevel finishLevel = c.gameObject.GetComponent<FinishLevel>();
        if(finishLevel != null)
        {   // finish level prefab hit

            // stop time and sound
            Time.timeScale = 0f;                                        
            AudioListener.pause = true;

            // interfaces display
            actualScoreCanvas.SetActive(false);
            finalScoreCanvas.SetActive(true);
            debugCanvas.SetActive(false);
            finalScoreText.text = "" + totalScore;
            finalCoinCountText.text = "" + coinCounter;

            // saving score
            ScoreStatistics scoreStatistics = new ScoreStatistics();
            Debug.Log("Salvo punteggio");
            scoreStatistics.SaveScore(levelNumber, totalScore);
        }
    }
}
