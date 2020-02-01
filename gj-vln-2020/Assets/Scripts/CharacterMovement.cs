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

    public float CharacterAgentAcceleration = 2f;
    public float CharacterAgentDeceleration = 5f;
    public float CharacterAgentCloseToDestination = 0.00000000000001f;

    bool NeedsToWalk;

    bool IsRunning;

    Rect StaminaRectangle;
    Texture2D StaminaTexture;
    
    Animator CharacterAnimator;
    private bool isFixing = false;
    public Transform NextItemPosition;

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
    {   
        if(!isFixing)
        {
            // casting ray once from the mouse position
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
            // checking if the player reached the destination and switching to idle animation
            else if (!CharacterAgent.pathPending)
            {
                if (CharacterAgent.remainingDistance <= 0.1f)
                {
                    if (NeedsToWalk)
                    {
                        CharacterAgent.updateRotation = false;
                        NeedsToWalk = false;
                        IsRunning = false;
                        if ((transform.position - NextItemPosition.position).magnitude < 2f) //arrived at destination to fix item
                        {
                            PlayFixingAnimation();
                            StartCoroutine(WaitToFinishFixing());
                        }
                        else
                        {
                            CharacterAnimator.CrossFadeInFixedTime("Idle", 0.3f);
                        }
                    }
                }
            }
        }
        
        StaminaRecharge();
    }
    void Walking(Ray ray)
    {   RaycastHit MouseHit;
        if (Physics.Raycast(ray, out MouseHit, 100, whatCanBeClickedOn))
        {   
            CharacterAgent.updateRotation = true;
            CharacterAgent.SetDestination(MouseHit.point);
            CharacterAnimator.CrossFadeInFixedTime("Walking",0.3f);
            CharacterAgent.speed = 4;
            CharacterAgent.acceleration = (CharacterAgent.remainingDistance <CharacterAgentCloseToDestination) ? CharacterAgentDeceleration : CharacterAgentAcceleration;
            if (Vector3.Distance(MouseHit.point, transform.position) < CharacterAgent.stoppingDistance)
            {
                CharacterAgent.SetDestination(transform.position);
                CharacterAgent.updateRotation = false;
            }
            else
            {
                CharacterAgent.SetDestination(MouseHit.point);
            }
            NeedsToWalk = true;
            IsRunning =false;
        }    
    }
    void Running(Ray ray)
    {    RaycastHit MouseHit;
        if (CurrentStamina > 1)
        {   
            CharacterAgent.updateRotation = true;
            if (Physics.Raycast(ray, out MouseHit, 100, whatCanBeClickedOn))
            {  
                CharacterAgent.SetDestination(MouseHit.point);
                CharacterAnimator.CrossFadeInFixedTime("Running",0.3f);
                CharacterAgent.acceleration = (CharacterAgent.remainingDistance <CharacterAgentCloseToDestination) ? CharacterAgentDeceleration : CharacterAgentAcceleration;
                if (Vector3.Distance(MouseHit.point, transform.position) < CharacterAgent.stoppingDistance)
                {
                    CharacterAgent.SetDestination(transform.position);
                    CharacterAgent.updateRotation = false;
                }
                CharacterAgent.speed = 15;
                CharacterAgent.angularSpeed = 3600;
                NeedsToWalk = true;
                //CharacterAgent.velocity = CharacterAgent.velocity*10;
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

    private void PlayFixingAnimation()
    {
        switch(NextItemPosition.gameObject.tag)
        {
            case ("Electronic"):
                {
                    CharacterAnimator.CrossFadeInFixedTime("nanny_Fixing", 0.3f);
                    break;
                }
            case ("Kickable"):
                {
                    if(Game.current.CleaningLevel == 0)
                    {
                        CharacterAnimator.CrossFadeInFixedTime("nanny_Cleaning", 0.3f);
                    }
                    else
                    {
                        CharacterAnimator.CrossFadeInFixedTime("nanny_Sweeping", 0.3f);
                    }
                    
                    break;
                }
            default:
                {
                    CharacterAnimator.CrossFadeInFixedTime("nanny_Lifting", 0.3f);
                    break;
                }
        }
    }

    private IEnumerator WaitToFinishFixing()
    {
        while (NextItemPosition.gameObject.GetComponent<Breakables>().BreakStatus != 3)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isFixing = false;
    }
}
