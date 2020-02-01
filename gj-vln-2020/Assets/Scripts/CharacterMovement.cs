using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovement : MonoBehaviour
{
    public LayerMask whatCanBeClickedOn;
    public int MaxStamina;
    public float CurrentStamina;
    public float StaminaRechargeDelayTimer;
    public float StaminaRechargeTimeToDelay;
    public float StaminaRechargeRate;
    public float StaminaDecreasePerFrame;
    public float StaminaIncreasePerFrame;
     private float StaminaTimeToRegen = 3.0f;
    private NavMeshAgent CharacterAgent;

    bool NeedsToWalk;

    bool IsRunning;

    Rect StaminaRectangle;
    Texture2D StaminaTexture;
    
    Animator CharacterAnimator;
    // assigning variable values
    void Start()
    {
        CharacterAgent = GetComponent<NavMeshAgent>();
        CharacterAnimator = GetComponent<Animator>();
        CurrentStamina = MaxStamina;
        StaminaRectangle = new Rect(Screen.width / 10, Screen.height *9/10, Screen.width /3, Screen.height / 50);
        StaminaTexture = new Texture2D(1,1);
        StaminaTexture.SetPixel(0,0, Color.black);
        StaminaTexture.Apply();
    }
    // Update is called once per frame
    void Update()
    {   // casting ray once from the mouse position
        Ray MouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //checking for collisions
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Walking(MouseRay);
            // Checking if the player can go to the collision point, setting navmesh agent destination, playing walking animation.
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Running(MouseRay);
        }
        else
        // checking if the player reached the destination and switching to idle animation
        if (!CharacterAgent.pathPending)
        {   
            if (CharacterAgent.remainingDistance<=0.01f)
            {   
                if(NeedsToWalk)
                {
                    CharacterAnimator.CrossFadeInFixedTime("Idle",0f);
                    NeedsToWalk = false;
                    IsRunning = false;
                }
            }
        } 
        StaminaRecharge();
    }
    void Walking(Ray ray)
    {   RaycastHit MouseHit;
        if (Physics.Raycast(ray, out MouseHit, 100, whatCanBeClickedOn))
        {  
            CharacterAgent.SetDestination(MouseHit.point);
            CharacterAnimator.CrossFadeInFixedTime("Walking",0f);
            NeedsToWalk = true;
            IsRunning =false;
            
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
                CharacterAgent.angularSpeed = 3600;
                NeedsToWalk = true;
                CharacterAgent.velocity = CharacterAgent.velocity*10;
               if(CurrentStamina <1)
               {
                   IsRunning = false;

               } 
                IsRunning = true;
            }    
        }
    }
    void StaminaRecharge()
    {
        if (IsRunning)
     {
         CurrentStamina = Mathf.Clamp(CurrentStamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
         StaminaRechargeDelayTimer = 0.0f;
     }
     else if (CurrentStamina < MaxStamina)
     {
         if (StaminaRechargeDelayTimer >= StaminaTimeToRegen)
             CurrentStamina = Mathf.Clamp(CurrentStamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
         else
             StaminaRechargeDelayTimer += Time.deltaTime;
     }
    }
    void OnGUI()
    {
        float ratio = CurrentStamina / MaxStamina;
        float rectWidth = ratio*Screen.width /3;
        StaminaRectangle.width =rectWidth;
        GUI.DrawTexture(StaminaRectangle, StaminaTexture);
    }

}
