using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Sprite[] sprites;

    public void Init()
    {
        int index = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }

    public void Update()
    {
        transform.position += Vector3.left * GameManager.Instance.GameSpeed;
    }

    public void DestroyObstacle()
    {
        Debug.Log($"장애물 {gameObject.name} 파괴됨!");

        // 자신을 비활성화화
        gameObject.SetActive(false);
    }  
}
