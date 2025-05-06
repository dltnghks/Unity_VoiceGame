using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [Header("Map Settings")]
    public GameObject[] TileGameObject;
    private Vector3 startPosition;
    private Vector3 endPosition;
    
    [Header("Obstacle Settings")]
    public GameObject ObstaclePrefab;
    public Transform SpawnPoint;
    private float obstacleTimer = 0f;
    
    // Object Pool
    private Queue<GameObject> obstacles = new Queue<GameObject>();
    private List<GameObject> spawnedObstacles = new List<GameObject>();
    
    [Header("Game Settings")]
    public float ObstacleInterval = 5f;
    
    
    private void Awake()
    {
        if (TileGameObject.Length != 0)
        {
            endPosition = TileGameObject[0].transform.position;
            startPosition = TileGameObject[TileGameObject.Length - 1].transform.position;
        }
        else
        {
            Debug.LogError("No Ground GameObject");
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        obstacleTimer += Time.deltaTime;
        
        if (ObstacleInterval <= obstacleTimer)
        {
            obstacleTimer = 0f;
            CreateObstacle();
        }

        MoveMap();
        MoveObstacles();
    }

    private void CreateObstacle()
    {
        GameObject newObstacle;
        if (obstacles.TryDequeue(out GameObject obstacle))
        {
            obstacle.SetActive(true);
            obstacle.transform.position = SpawnPoint.position;

            newObstacle = obstacle;

        }
        else
        {
            newObstacle = Instantiate(ObstaclePrefab, SpawnPoint.position, Quaternion.identity);
        }

        newObstacle.GetComponent<Obstacle>().Init();
        
        spawnedObstacles.Add(newObstacle);
    }

    private void MoveMap()
    {
        foreach (var tile in TileGameObject)
        {
            tile.transform.position += Vector3.left * GameManager.Instance.GameSpeed;

            if (tile.transform.position.x < endPosition.x)
            {
                tile.transform.position = startPosition;
            }
        }
    }
    
    private void MoveObstacles()
    {
        foreach (var obstacle in spawnedObstacles)
        {
            if (obstacle.transform.position.x < endPosition.x)
            {
                obstacle.SetActive(false);
                obstacles.Enqueue(obstacle);
            }
        }
    }
    
}
