using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;

    private void Start()
    {
        GameManager.Instance.AddScoreAction += SetScoreText;
        GameManager.Instance.GameStart();
    }

    private void SetScoreText(float score)
    {
        if (ScoreText)
        {
            ScoreText.text = score.ToString("0");
        }
        else
        {
            Debug.LogWarning("Score Text is null");
        }
    }
}
