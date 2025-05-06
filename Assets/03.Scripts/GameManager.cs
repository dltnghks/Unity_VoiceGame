using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;

    public float InitGameSpeed = 0.005f;
    public float GameSpeed = 0.005f;
    
    public bool IsGameOver = false;
    public float Score = 0;
    
    public Action<float> AddScoreAction;
    
    void Awake()
    {
        if (null == instance)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Update()
    {
        if (!IsGameOver)
        {
            Score += Time.deltaTime;
            AddScoreAction?.Invoke(Score);
        }
    }
    
    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void GameStart()
    {
        IsGameOver = false;
        Score = 0;
        GameSpeed = InitGameSpeed;
    }
    
    public void GameOver()
    {
        IsGameOver = true;
        GameSpeed = 0;

        StartCoroutine(ChangeScene());

        AddScoreAction = null;
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2.0f);
        
        SceneManager.LoadScene("EndingScene");
    }
}
