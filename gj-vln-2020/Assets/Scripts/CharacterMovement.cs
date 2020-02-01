using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{

    public LayerMask whatCanBeClickedOn;
    public int MaxStamina;
    public int CurrentStamina;
    public int StaminaRechargeDelay;
    public int StaminaRechargeRate;
    private NavMeshAgent CharacterAgent;

    bool NeedsToWalk;

    Animator CharacterAnimator;
    // assigning variable values
    void Start()
    {
        CharacterAgent = GetComponent<NavMeshAgent>();
        CharacterAnimator = GetComponent<Animator>();
        CurrentStamina = MaxStamina;
        //NeedsToWalk = false;
    }

    // Update is called once per frame
    void Update()
    {   // casting ray once from the mouse position
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
     
        
        //checking for collisions
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit MouseHit;
            
            // Checking if the player can go to the collision point, setting navmesh agent destination, playing walking animation.
            if (Physics.Raycast(MouseRay, out MouseHit, 100, whatCanBeClickedOn))
            {  
                CharacterAgent.SetDestination(MouseHit.point);
                CharacterAnimator.CrossFadeInFixedTime("Walking",0f);
                CharacterAgent.speed = 10;
                NeedsToWalk = true;
            }    
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Running(MouseRay);
        }
        // checking if the player reached the destination and switching to idle animation
        if (!CharacterAgent.pathPending)
        {   
            if (CharacterAgent.remainingDistance<=0.01f)
            {   
                if(NeedsToWalk)
                {
                    CharacterAnimator.CrossFadeInFixedTime("Idle",0f);
                    NeedsToWalk = false;
                }
               
            }
        } 
    }

    void Running(Ray ray)
    {    RaycastHit MouseHit;
        if (CurrentStamina > 1)
        {
            if (Physics.Raycast(ray, out MouseHit, 100, whatCanBeClickedOn))
            {  
                CharacterAgent.SetDestination(MouseHit.point);
                CharacterAnimator.CrossFadeInFixedTime("Running",0f);
                CharacterAgent.speed = 15;
                CharacterAgent.angularSpeed = 360;
                NeedsToWalk = true;
                CharacterAgent.velocity = CharacterAgent.velocity*10;
            }    
        }
    }


}
