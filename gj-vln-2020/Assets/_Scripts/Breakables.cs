using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    //0 - not broken; 1 - is being fixed
    //2 - is being broken 3 - broken
    public int BreakStatus = 0;
    //for easier status bar display, 0-100
    public int Health = 100;
    //possible locking to prevent multiple characters from accessing the same object at once?
    public GameObject Lock = null;

    private Animator animator;
    private int upgradeLevel;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        switch(this.tag)
        {
            case ("Electronic"):
                {
                    upgradeLevel = Game.current.RepairLevel;
                    break;
                }
            case ("Kickable"):
                {
                    upgradeLevel = Game.current.CleaningLevel;
                    break;
                }
            default:
                {
                    upgradeLevel = Game.current.StrengthLevel;
                    break;
                }
        }
    }

    public void Break()
    {
        BreakStatus = 2;
        //animator play break animation
        StartCoroutine(breakThis());
    }

    public void Fix()
    {
        BreakStatus = 1;
        //play fix anim
        StartCoroutine(fixThis());
    }

    private IEnumerator breakThis()
    {
        while (Health > 0)
        {
            Health -= 5;
            yield return new WaitForSeconds(0.1f);
        }

        Health = 0;
        BreakStatus = 3;
        //stop playing break animation, go to idle broken
    }

    private IEnumerator fixThis()
    {
        while(Health < 100)
        {
            Health += 2 * upgradeLevel;
            yield return new WaitForSeconds(0.1f);
        }

        Health = 100;
        BreakStatus = 0;
        //stop playing fix animation, go to idle
    }
}