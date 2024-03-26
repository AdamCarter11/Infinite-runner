using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    int actualScore;
    int displayedScore;


    string end = "End Screen";

    [SerializeField] Text highScore;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(MyAlwaysRunningScoreUpdater());
        if(end != SceneManager.GetActiveScene().name)
        {
            StartCoroutine(MyAlwaysRunningScoreIncreaser());
            PlayerPrefs.SetInt("Score", 0);
        }
       
        
        
    }

    // Update is called once per frame
    void Update()
    {
        highScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore").ToString();
        if(end != SceneManager.GetActiveScene().name) 
        {
            scoreText.text = displayedScore.ToString();
        }
        else
        {
            scoreText.text = PlayerPrefs.GetInt("Score").ToString();
        }
        
        //GAME OVER
        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("End Screen");
            PlayerPrefs.SetInt("Score", actualScore);
            if(PlayerPrefs.GetInt("Score") > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", actualScore);
            }
        }
        
    }

    public int ReturnScore() 
    { 
        return actualScore;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("SoyScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator MyAlwaysRunningScoreIncreaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            actualScore += 10;
        }
    }
    IEnumerator MyAlwaysRunningScoreUpdater()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            int difference = actualScore - displayedScore;

            if (difference != 0)
            {
                int constantTerm = 1;

                int proportionalTerm = difference / 5;

                int moveStep = Mathf.Abs(proportionalTerm) + constantTerm;

                displayedScore = (int)Mathf.MoveTowards(displayedScore, actualScore, moveStep);

                
            }
        }
    }
}
