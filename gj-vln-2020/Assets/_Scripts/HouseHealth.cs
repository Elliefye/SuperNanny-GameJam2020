using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//designed to be attached to house health bar object
public class HouseHealth : MonoBehaviour
{
    [SerializeField] private KidsAI kidRef;
    private float lastRatio = 1;

    void Update()
    {
        float ratio = (float)(kidRef.AllBreakables.Length - Breakables.BrokenCount) / kidRef.AllBreakables.Length;

        if (ratio != lastRatio)
        {            
            StartCoroutine(SmoothTransition(ratio));
        }            
    }

    IEnumerator SmoothTransition(float targetRatio)
    {
        StopAllCoroutines();
        float currentRatio = (float)Math.Round(transform.localScale.x, 2);

        while(currentRatio != targetRatio)
        {
            if (currentRatio > targetRatio)
            {
                currentRatio -= 0.01f;
            }
            else
            {
                currentRatio += 0.01f;                
            }
            transform.localScale = new Vector3(currentRatio, 1, 1);
            yield return null;
        }

        lastRatio = targetRatio;
    }
}
