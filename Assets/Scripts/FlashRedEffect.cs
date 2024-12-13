using System.Collections;
using UnityEngine;

public class FlashRedEffect : MonoBehaviour
{
    private SpriteRenderer[] bodyParts;

    void Start()
    {
        bodyParts = GetComponentsInChildren<SpriteRenderer>();
    }

    public IEnumerator FlashRed()
    {

        foreach (var part in bodyParts)
        {
            if (part != null)
            {

                part.color = Color.red;  
            }
        }

        yield return new WaitForSeconds(0.3f); 

        foreach (var part in bodyParts)
        {
            if (part != null)
            {
                part.color = Color.white;  
            }
        }
    }
}
