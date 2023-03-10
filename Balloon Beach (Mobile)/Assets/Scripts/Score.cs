using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    public int highScore;
    public int coinCount;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI highScoreUI;
    public TextMeshProUGUI coinCountUI;
    public TextMeshProUGUI coinCountUIDeathScreen;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("highscore");
    }

    // Update is called once per frame
    void Update()
    {
        scoreUI.text = score.ToString();
        highScoreUI.text = highScore.ToString();
        coinCountUI.text = coinCount.ToString();
        coinCountUIDeathScreen.text = coinCount.ToString();
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highscore", highScore);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Score hit!");
        if (other.gameObject.tag == "score gate")
        {
            score++;
        }
        else if (other.gameObject.tag == "coin")
        {
            coinCount++;
        }

    }
}
