using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private GameObject finishLine;
    public AudioSource audioSource;
    public AudioClip finishSound;
    public GameManager gameManager;

    void Start()
    {
        finishLine = GameObject.FindWithTag("Finish");
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Bullet"))
        {
        Destroy(finishLine);
        PlayFinishSound();
        gameManager.SuccessPanel();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);  
        }
        else
        {
            SceneManager.LoadScene(0); 
        }
    }
}

    void PlayFinishSound()
    {
        if (audioSource != null && finishSound != null)
        {
            audioSource.PlayOneShot(finishSound); 
        }
    }

}
