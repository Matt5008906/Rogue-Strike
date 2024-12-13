using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialBullet : MonoBehaviour
{
    public GameObject initialBullet; 

    void Start()
    {
        Bullet bulletScript = initialBullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.isInitialBullet = true;  
        }
    }
}