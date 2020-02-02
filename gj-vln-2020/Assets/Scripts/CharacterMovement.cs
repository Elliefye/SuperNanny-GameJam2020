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
    public float CharacterAgentDeceleration = 100f;
    public float CharacterAgentCloseToDestination = 0.00000000000001f;

    public float GameStartTimer;
    public GameObject GameOverCanvas;

    bool NeedsToWalk;

    bool IsRunning;

    Animator CharacterAnimator;
    private bool isFixing = false;
    public Transform NextItemPosition;
    public Transform staminaBar;

    void Start()
    {
        CharacterAgent = GetComponent<NavMeshAgent>();
        CharacterAnimator = GetComponent<Animator>();
        CurrentStamina = MaxStamina;
        StaminaIncreasePerFrame = StaminaIncreasePerFrame * (Game.current.StaminaLevel +1);
        GameStartTimer = 0.0f;
    }

    void Update()
    {   
        GameTimer();
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
                if (CharacterAgent.remainingDistance <= 1f)
                {
                    if (NeedsToWalk)
                    {
                        CharacterAgent.updateRotation = false;
                        CharacterAgent.SetDestination(transform.position);
                        NeedsToWalk = false;
                        IsRunning = false;
                        if (NextItemPosition != null && (transform.position - NextItemPosition.position).magnitude < 2f) //arrived at destination to fix item
                        {
                            isFixing = true;
                            PlayFixingAnimation();
                            NextItemPosition.gameObject.GetComponent<Breakables>().Fix();
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
                NeedsToWalk = true;
                //CharacterAgent.velocity = CharacterAgent.velocity*10;
               if(CurrentStamina <1)
               {
                   IsRunning = false;

               } 
                IsRunning = true;
            }    
        }
        else
        {
            Walking(ray);
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
            Debug.Log(CurrentStamina);
            if (StaminaRechargeDelayTimer >= StaminaTimeToRegen)
                {CurrentStamina = Mathf.Clamp(CurrentStamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
                Debug.Log(CurrentStamina);
                }
            else
                StaminaRechargeDelayTimer += Time.deltaTime;
        }
    }
    void OnGUI()
    {
        float ratio = CurrentStamina / MaxStamina;
        staminaBar.localScale = new Vector3(ratio, 1, 1);
    }

     void PlayFixingAnimation()
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

     IEnumerator WaitToFinishFixing()
    {
        while (NextItemPosition.gameObject.GetComponent<Breakables>().BreakStatus == 1)
        {
            yield return new WaitForSeconds(0.1f);
        }
        isFixing = false;
        CharacterAnimator.CrossFadeInFixedTime("Idle", 0.3f);
    }

    public void WalkToFix()
    {
        CharacterAgent.updateRotation = true;
        CharacterAgent.SetDestination(NextItemPosition.position);
        CharacterAnimator.CrossFadeInFixedTime("Walking", 0.3f);
        CharacterAgent.speed = 4;
        CharacterAgent.acceleration = (CharacterAgent.remainingDistance < CharacterAgentCloseToDestination) ? CharacterAgentDeceleration : CharacterAgentAcceleration;
        
        if (Vector3.Distance(NextItemPosition.position, transform.position) < CharacterAgent.stoppingDistance)
        {
            CharacterAgent.SetDestination(transform.position);
            CharacterAgent.updateRotation = false;
        }

        NeedsToWalk = true;
        IsRunning = false;
    }

    public void RunToFix()
    {
        if (CurrentStamina > 1)
        {
            CharacterAgent.updateRotation = true;
            CharacterAgent.SetDestination(NextItemPosition.position);
            CharacterAnimator.CrossFadeInFixedTime("Running", 0.3f);
            CharacterAgent.speed = 15;
            CharacterAgent.acceleration = (CharacterAgent.remainingDistance < CharacterAgentCloseToDestination) ? CharacterAgentDeceleration : CharacterAgentAcceleration;

            if (Vector3.Distance(NextItemPosition.position, transform.position) < CharacterAgent.stoppingDistance)
            {
                CharacterAgent.SetDestination(transform.position);
                CharacterAgent.updateRotation = false;
            }

            NeedsToWalk = true;
            if (CurrentStamina < 1)
            {
                IsRunning = false;
            }
            IsRunning = true;
        }
        else
        {
            WalkToFix();
        }
    }

     void GameTimer()
    {
        bool GameOverPlayed = false;
        
        
        if (GameStartTimer >= 60.0f)
        {
            
            if(GameStartTimer <= 5.5f)
            {
                GameOverCanvas.SetActive(true);;
            GameOverPlayed = true;

            }
            
            
            Debug.Log("gameover");
        }
        
        else
        {   
            GameStartTimer += Time.deltaTime;
            Debug.Log(GameStartTimer);
            GameOverPlayed = false;
            Debug.Log(GameOverPlayed);
        }
        

    }
}
