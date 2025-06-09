// Projectile.cs
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // 투사체가 사라지는 시간

    void Start()
    {
        // 일정 시간 후 투사체 파괴 (화면 밖으로 나가는 것 방지)
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other) // 'other'는 충돌한 다른 콜라이더를 나타냅니다.
    {
        Debug.Log($"Projectile hit: {other.name}");

        // 부딪힌 오브젝트에 DestructibleObstacle 컴포넌트가 있는지 확인
        Obstacle obstacle = other.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            // 장애물 파괴
            obstacle.DestroyObstacle();
            
            // 투사체 자신도 파괴 (충돌 후 사라지도록)
            Destroy(gameObject);
        }
    }
}