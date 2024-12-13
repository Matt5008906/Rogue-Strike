using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollide : MonoBehaviour
{
    private GameObject player;
    public PlayerMovement playerMovement;
    public GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacles") && other.gameObject != null)
        {
            playerMovement.playDeathSound();
            playerMovement.StopWalkingSound();
            Destroy(player);
            gameManager.ObstacleDeath();
        }
    }
}
