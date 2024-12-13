using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  
    public Vector3 offset;    

    public float followSpeed = 5f;  

    void Update()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}
