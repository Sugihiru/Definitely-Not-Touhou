using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ScoreRowDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreIdTextField;
    public TextMeshProUGUI scoreUserTextField;
    public TextMeshProUGUI scoreTimeTextField;
    public TextMeshProUGUI scoreValueTextField;
    public TextMeshProUGUI scoreDateTimeTextField;

    private Color32 highlightColor = new Color32(255, 255, 0, 255);

    public void DisplayScore(ScoreModel score)
    {
        scoreUserTextField.text = score.author;
        scoreTimeTextField.text = score.secondsSurvived.ToString() + "s";
        scoreValueTextField.text = score.score.ToString();

        if (score.tmpScoreId != null)
        {
            scoreIdTextField.color = highlightColor;
            scoreUserTextField.color = highlightColor;
            scoreTimeTextField.color = highlightColor;
            scoreValueTextField.color = highlightColor;
            scoreDateTimeTextField.color = highlightColor;
        }
    }
}
