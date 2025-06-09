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
    
    private bool isAttacking = false;

    private Rigidbody2D rigidbody; 
    
    private Animator animator;

    // 발사 관련 변수
    public GameObject projectilePrefab; // 발사할 투사체의 프리팹 (Inspector에서 할당)
    public Transform firePoint;         // 투사체가 발사될 위치 (예: 총구, 플레이어 앞)
    public float projectileSpeed = 10f; // 투사체 속도



    // "앞으로 가자" 명령어
    Regex jump_regex = new Regex(@"(뛰어|점프|위로|넘어가|넘어 가|폴짝|JUMP|jump|Jumping)");
    Regex attack_regex = new Regex(@"(attack|shoot|shot)");

    private void Awake()
    {
        VoskSpeechToText.OnTranscriptionResult += OnTranscriptionResult;
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        
    }


    private void OnTranscriptionResult(string obj)
    {
        Debug.Log(obj);
        var result = new RecognitionResult(obj);
        bool action = false;
        foreach (RecognizedPhrase p in result.Phrases)
        {
            if (jump_regex.IsMatch(p.Text))
            {
                Jump();
                action = true;
            }
            if (attack_regex.IsMatch(p.Text))
            {
                FireProjectile();
                action = true;
            }
            if (action)
            {
                return;
            }
        }
    }
    
    // 점프 함수
    void Jump()
    {
        if (!isJumping && !GameManager.Instance.IsGameOver)
        {
            animator.SetTrigger("TriggerJump");
            isJumping = true; 
            Debug.Log("Jump");
            rigidbody.velocity = Vector2.up * JumpForce; // 위 방향으로 힘을 주어 점프
        }
    }

    // 공격 함수
    public void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Debug.Log("투사체 발사!");
            // 투사체 인스턴스 생성
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            // 투사체에 속도 부여
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = firePoint.right * projectileSpeed;
            }
            else
            {
                Debug.LogWarning("PlayerActionController: 발사된 투사체에 Rigidbody가 없습니다. 속도를 부여할 수 없습니다.");
            }
        }
    }

    // 음성 인식에서 호출될 함수 (외부에서 호출 가능)
    public void TriggerJump()
    {
        isJumping = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            Debug.Log(collision.gameObject.name);
            animator.SetTrigger("TriggerDeath");
            GameManager.Instance.GameOver();
            return;
        }
        if (collision.gameObject.tag == "Ground" && isJumping)
        {
            animator.SetTrigger("TriggerMove");
            isJumping = false; // 점프 후 초기화
        }
    }
}
