using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterScore : MonoBehaviour
{
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

    //public TMP_Text finalObstacleCountText;

    // Start is called before the first frame update
    void Start()
    {
        actualScoreText.text = "Score: " + totalScore;
        playingSounds = GetComponents<AudioSource>();
        playingSounds[1].clip = runningSound;
        playingSounds[1].Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void OnTriggerEnter(Collider c)
    {
        Coin coin = c.gameObject.GetComponent<Coin>();
        if (coin != null)
        {   // a coin was hit
            coinCounter++;
            totalScore += coin.GetCoinValue();
            Destroy(coin.gameObject);
            // Debug.Log("Total score: " + totalScore);
            actualScoreText.text = "Score: " + totalScore;
            playingSounds[0].clip = coinSound;
            playingSounds[0].Play();
        }

        Obstacle obstacle = c.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {   // an obstacle was hit
            //obstacleCounter++;
            totalScore -= obstacle.GetObstacleValue();
            // Debug.Log("Total score: " + totalScore);
            actualScoreText.text = "Score: " + totalScore;
            playingSounds[0].clip = obstacleSound;
            playingSounds[0].Play();
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
            Time.timeScale = 0f;    //la speed si ferma
            AudioListener.pause = true;
            actualScoreCanvas.SetActive(false);
            finalScoreCanvas.SetActive(true);
            debugCanvas.SetActive(false);
            finalScoreText.text = "" + totalScore;
            finalCoinCountText.text = "" + coinCounter;
            //finalObstacleCountText.text = "" + obstacleCounter;

        }
    }
}
