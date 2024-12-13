using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    public Tilemap groundTilemap;
    public GameObject prefab1;
    public GameObject prefab2;
    [SerializeField] private int maxEnemies = 15;
    private int currentEnemyCount = 0;
    public Vector2 colliderSize = new Vector2(2f, 3f);
    public float spawnRadius = 1f;

    void Start()
    {
    }

    void Update()
    {
        if (currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Bounds tilemapBounds = groundTilemap.localBounds;
        Vector3 tilemapWorldPosition = groundTilemap.transform.position;

        float randomX = Random.Range(tilemapBounds.min.x + tilemapWorldPosition.x + 1f, tilemapBounds.max.x + tilemapWorldPosition.x - 1f);
        float randomY = Random.Range(tilemapBounds.min.y + tilemapWorldPosition.y + 1f, tilemapBounds.max.y + tilemapWorldPosition.y - 1f);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0);

        while (IsPositionOccupied(spawnPosition))
        {
            randomX = Random.Range(tilemapBounds.min.x + tilemapWorldPosition.x + 1f, tilemapBounds.max.x + tilemapWorldPosition.x - 1f);
            randomY = Random.Range(tilemapBounds.min.y + tilemapWorldPosition.y + 1f, tilemapBounds.max.y + tilemapWorldPosition.y - 1f);

            spawnPosition = new Vector3(randomX, randomY, 0);
        }

        GameObject selectedPrefab = Random.Range(0, 2) == 0 ? prefab1 : prefab2;
        GameObject enemy = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        enemy.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = enemy.AddComponent<Rigidbody2D>();
            rb.gravityScale = 5;
        }
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.freezeRotation = true;
        rb.drag = 0f;

        BoxCollider2D boxCollider = enemy.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            boxCollider = enemy.AddComponent<BoxCollider2D>();
        }

        boxCollider.size = colliderSize;

        enemy.tag = "Enemy";

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
        if (enemyAI == null)
        {
            enemyAI = enemy.AddComponent<EnemyAI>();
        }
        FlashRedEffect flashRedEffect = enemy.GetComponent<FlashRedEffect>();
        if (flashRedEffect == null)
        {
            flashRedEffect = enemy.AddComponent<FlashRedEffect>();
        }

        currentEnemyCount++;
    }

    bool IsPositionOccupied(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, spawnRadius);
        return colliders.Length > 0;
    }

    public void ResetEnemyCount()
    {
        currentEnemyCount = 0;
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(Vector2.up * 0.1f, ForceMode2D.Impulse);  
        }

    }

}