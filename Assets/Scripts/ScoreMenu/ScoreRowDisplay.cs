using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class ScoreRowDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreIdTextField;
    public TextMeshProUGUI scoreUserTextField;
    public TextMeshProUGUI scoreTimeTextField;
    public TextMeshProUGUI scoreValueTextField;
    public TextMeshProUGUI scoreDateTimeTextField;
    public GameOverScoreMenuScreen gameOverScoreMenuScreen;

    private Color32 highlightColor = new Color32(255, 255, 0, 255);
    private List<KeyCode> allowedUserKeys = new List<KeyCode>() {
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z,
        KeyCode.Backspace,
        KeyCode.Return,
    };
    private ScoreModel score;
    private bool canEdit = true;

    public void DisplayScore(ScoreModel score)
    {
        this.score = score;
        scoreUserTextField.text = score.author;
        scoreTimeTextField.text = score.secondsSurvived.ToString().Replace(",", ".") + "s";
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

    void Update()
    {
        if (Input.anyKeyDown && this.score.tmpScoreId != null && canEdit)
        {
            KeyCode keyPressed = getCurrentKeyDown();
            if (keyPressed == KeyCode.Backspace)
            {
                if (scoreUserTextField.text.Length >= 1)
                    scoreUserTextField.text = scoreUserTextField.text.Remove(scoreUserTextField.text.Length - 1);
            }
            else if (keyPressed == KeyCode.Return)
            {
                if (scoreUserTextField.text.Length > 0)
                {
                    StartCoroutine(gameOverScoreMenuScreen.PublishScore(this.score.tmpScoreId, scoreUserTextField.text));
                    canEdit = false;
                }
            }
            else if (keyPressed != KeyCode.None && scoreUserTextField.text.Length != 8)
            {
                if (scoreUserTextField.text.Length == 0)
                {
                    scoreUserTextField.text += getCurrentKeyDown().ToString().ToUpper();
                }
                else
                {
                    scoreUserTextField.text += getCurrentKeyDown().ToString().ToLower();
                }
            }
        }
    }

    public KeyCode getCurrentKeyDown()
    {
        KeyCode finalKeyCode = KeyCode.None;

        foreach (KeyCode kcode in allowedUserKeys)
        {
            if (Input.GetKey(kcode))
            {
                finalKeyCode = kcode;
            }
        }
        return finalKeyCode;
    }
}
