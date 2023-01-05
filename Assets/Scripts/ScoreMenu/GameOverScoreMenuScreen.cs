using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScoreMenuScreen : MonoBehaviour
{
    public GameOverMenuScreen gameOverMenuScreen;
    public List<ScoreRowDisplay> scoreRowDisplays;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        Debug.Log(gameOverMenuScreen.scores);
        for (int i = 0; i < scoreRowDisplays.Count; ++i)
        {
            scoreRowDisplays[i].DisplayScore(gameOverMenuScreen.scores[i]);
        }
    }
}
