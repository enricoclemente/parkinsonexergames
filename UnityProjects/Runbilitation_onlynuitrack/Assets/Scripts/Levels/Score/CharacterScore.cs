using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScore : MonoBehaviour
{
    int totalScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    void OnTriggerEnter(Collider c)
    {
        Coin coin = c.gameObject.GetComponent<Coin>();
        if (coin != null)
        { // a coin was hit
            totalScore += coin.GetCoinValue();
            Destroy(coin.gameObject);
        }


        Obstacle obstacle = c.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            totalScore -= obstacle.GetObstacleValue();
        }

        Debug.Log("Total score: " + totalScore);
    }
}
