using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Enums
    private enum State
    {
        Idle,
        Walking,
        Chasing,
        Dead
    }
    #endregion

    #region Variables
    private State currentState = State.Idle;
    private GameObject player;
    private float detectionRange = 3f;
    private float walkSpeed = 4f;
    private float chaseSpeed = 6f;
    private float idleTime = 5f;
    private float idleTimer = 0f;
    private float walkDistance = 10f;
    private bool walkingRight = false;
    private Vector3 startingPosition;
    private Rigidbody2D rb;
    private bool isJumping = false;
    private float jumpForce = 13f;
    private bool isGrounded = false;
    public LayerMask groundLayer = 1 << 0;
    private float jumpChanceInterval = 2f; 
    private float jumpChanceTimer = 0f;
    private float jumpChance = 0.7f; 
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 1;

    public int hitPoints = 3;
    private FlashRedEffect flashRedEffect;

    public AudioSource audioSource;
    public AudioClip punchSFX;

    #endregion

    #region Start
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            Debug.LogWarning("Player object not found, make sure it has the 'Player' tag");
        }
        
        flashRedEffect = GetComponent<FlashRedEffect>();

        startingPosition = transform.position;
        currentState = State.Idle;


        GameObject audioObject = GameObject.Find("Audio");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }
        punchSFX = Resources.Load<AudioClip>("Audio/punchSFX");
    
    }
    #endregion

    #region Update
    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Walking:
                Walking();
                break;
            case State.Chasing:
                Chasing();
                break;
            case State.Dead:
                Dead();
                break;
        }
    }
    #endregion

    #region Idle
    void Idle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            currentState = State.Walking;
            idleTimer = 0f;
        }
    }
    #endregion

    #region Walking
    void Walking()
    {
        transform.Translate(Vector2.right * (walkingRight ? 1 : -1) * walkSpeed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - startingPosition.x) >= walkDistance)
        {
            Flip();
            walkingRight = !walkingRight;
            startingPosition = transform.position;
        }

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= detectionRange)
            {
                currentState = State.Chasing;
            }
        }

        jumpChanceTimer += Time.deltaTime; 

        if (jumpChanceTimer >= jumpChanceInterval) 
        {
            jumpChanceTimer = 0f; 

            if (Random.value <= jumpChance && isGrounded && !isJumping)
            {
                Jump();
            }
        }
    }
    #endregion

    #region Chasing
    void Chasing()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance > detectionRange)
            {
                currentState = State.Walking;
            }
            else
            {
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(directionToPlayer.x * chaseSpeed, rb.velocity.y);

                FlipIfNeeded();
                if (distance <= attackRange)
                {
                    dealDamage();
                    Debug.Log("Attack");
                }
        }
    
    }
    void dealDamage()
    {

        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(transform.position, attackRange, 1 << 0);
        foreach (Collider2D player in hitPlayer)
        {
            
            player.GetComponent<PlayerMovement>().takeDamage(attackDamage);
            PlayDamageSound();
        }
    }

    void PlayDamageSound()
    {
        if (audioSource != null && punchSFX != null)
        {
            audioSource.PlayOneShot(punchSFX); 
        }
    }
}
    #endregion

    #region Dead
    void Dead()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Damage and Death
    public void Die()
    {
        currentState = State.Dead;
        Dead();
    }

    public void takeDamage(int damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0)
        {
            Die();
        }
        if (flashRedEffect != null)
        {
            StartCoroutine(flashRedEffect.FlashRed());
        }
    }
    #endregion

    #region Flip
    void FlipIfNeeded(){
    if (player != null)
    {
        bool isPlayerToLeft = player.transform.position.x < transform.position.x;
        bool isPlayerToRight = player.transform.position.x > transform.position.x;

        if (isPlayerToLeft && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (isPlayerToRight && transform.localScale.x > 0)
        {
            Flip();
        }
    }
}

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    #endregion

    #region Jumping
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        Invoke(nameof(ResetJump), 0.5f);
    }

    void ResetJump()
    {
        isJumping = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }
    #endregion
}