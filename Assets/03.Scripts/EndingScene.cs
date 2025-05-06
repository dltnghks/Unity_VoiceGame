using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingScene : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        if (ScoreText)
        {
            ScoreText.text = GameManager.Instance.Score.ToString("0");
        }
        else
        {
            Debug.LogWarning("No Score Text Provided");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("GameScene");
        }        
    }

}
