using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using TMPro;

public class Player : MonoBehaviour
{
    public VoskSpeechToText VoskSpeechToText;

    public float JumpForce = 5f;
    
    private bool isJumping = false;
    private Rigidbody rigidbody; 

    
    // "앞으로 가자" 명령어
    Regex jump_regex = new Regex(@"(뛰어|점프|위로|넘어가|넘어 가|폴짝)");
    
    private void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
        rigidbody = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        
    }


    private void OnTranscriptionResult(string obj)
    {
        var result = new RecognitionResult(obj);
        foreach (RecognizedPhrase p in result.Phrases)
        {
            if (jump_regex.IsMatch(p.Text))
            {
                Jump();
                return;
            }
        }
    }
    
    // 점프 함수
    void Jump()
    {
        if (!isJumping)
        {
            isJumping = true; 
            Debug.Log("Jump");
            rigidbody.velocity = Vector2.up * JumpForce; // 위 방향으로 힘을 주어 점프
            isJumping = false; // 점프 후 초기화
        }
    }

    // 음성 인식에서 호출될 함수 (외부에서 호출 가능)
    public void TriggerJump()
    {
        isJumping = true;
    }
}
