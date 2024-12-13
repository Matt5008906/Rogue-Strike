using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Transform groundCheck; 
    [SerializeField] private float hitPoints = 7;

    private Rigidbody2D rb;
    public bool isGrounded;
    private FlashRedEffect flashRedEffect;
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip walkingSound;
    public AudioClip deathSound;
    public bool isWalking = false;
    public GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flashRedEffect = GetComponent<FlashRedEffect>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        CheckGrounded();
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetKey(KeyCode.D) ? 1 : 0;
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (Input.GetKey(KeyCode.D))
        {
            isWalking = true;
            StartWalkingSound();
        }
        else
        {
            StopWalkingSound();
        }

    }

    void StartWalkingSound()
    { 

        if (!isWalking && audioSource != null && walkingSound != null && isGrounded)
        {
        audioSource.clip = walkingSound;
        audioSource.loop = true; 
        audioSource.Play();
        isWalking = true;
        }
    }
    public void StopWalkingSound()
    {
        if (isWalking && audioSource != null)
        {
        audioSource.Stop(); 
        isWalking = false;
        }
    }

    void HandleJump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            PlayJumpSound();
            isGrounded = false;  
        }
    }
    void PlayJumpSound()
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound); 
        }
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);

        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

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

    public void playDeathSound()
    {
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }

    public void Die()
    {
        playDeathSound();
        StopWalkingSound();
        Destroy(gameObject);
        gameManager.EnemyDeath();

    }    
}
