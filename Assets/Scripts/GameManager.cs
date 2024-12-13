using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ObstacleDeathPanel;  
    public GameObject EnemyDeathPanel;
    public GameObject successPanel;     

    public void ObstacleDeath()
    {
        ObstacleDeathPanel.SetActive(true); 
        StartCoroutine(WaitAndRestart(3f));  
    }

    public void EnemyDeath()
    {
        EnemyDeathPanel.SetActive(true); 
        StartCoroutine(WaitAndRestart(3f));  
    }

    public void SuccessPanel()
    {
        successPanel.SetActive(true);
        StartCoroutine(Wait(1f));
    }

    private IEnumerator WaitAndRestart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 
        RestartGame();  
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime); 
    }
}