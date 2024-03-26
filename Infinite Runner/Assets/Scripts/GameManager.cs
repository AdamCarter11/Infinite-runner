using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    int actualScore;
    int displayedScore;

    float time;
    int intTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MyAlwaysRunningScoreUpdater());
        StartCoroutine(MyAlwaysRunningScoreIncreaser());
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = displayedScore.ToString();
        
    }

    public int ReturnScore() 
    { 
        return actualScore;
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
