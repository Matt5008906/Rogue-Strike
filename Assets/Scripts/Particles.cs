using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{

    public ParticleSystem partSystem;
    public PlayerMovement playerMovement;

    void Update()
    {
        PlayParticles();
        
    }
    void PlayParticles()
    {
    if (playerMovement.isGrounded && playerMovement.isWalking)
    {
        if (!partSystem.isPlaying) 
        {
            partSystem.Play();
        }
    }
    else
    {
        if (partSystem.isPlaying)
        {
            partSystem.Stop();
        }
    }
}
}
