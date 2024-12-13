using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    private SpriteRenderer bulletSprite;
    public int damage = 1;

    public bool isInitialBullet = false;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletSprite = GetComponent<SpriteRenderer>();

        rb.velocity = transform.right * speed;

        if (!isInitialBullet)  
        {
            Destroy(gameObject, 0.6f);  
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.takeDamage(damage);
        }

        if (!isInitialBullet)
        {
            Destroy(gameObject);
        }
    }
}
