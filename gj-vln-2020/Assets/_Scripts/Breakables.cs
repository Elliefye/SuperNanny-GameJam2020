using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Breakables : MonoBehaviour
{
    public static int BrokenCount = 0;
    //0 - not broken; 1 - is being fixed
    //2 - is being broken 3 - broken
    public int BreakStatus = 0;
    //for easier status bar display, 0-100
    public int Health = 100;

    [SerializeField] Sprite FixBtnSprite;
    [SerializeField] Sprite FixBtnSpriteHover;
    [SerializeField] CharacterMovement Player;
    
    private Image buttonImage;
    private Animator animator;
    private int upgradeLevel;
    //0 - fix, 1 - lift, 2 - clean
    public int itemType;

    private float time;

    private void Awake()
    {
        animator = GetComponent<Animator>();        

        switch(this.tag)
        {
            case ("Electronic"):
                {
                    upgradeLevel = Game.current.RepairLevel;
                    itemType = 0;
                    break;
                }
            case ("Kickable"):
                {
                    upgradeLevel = Game.current.CleaningLevel;
                    itemType = 2;
                    break;
                }
            default:
                {
                    upgradeLevel = Game.current.StrengthLevel;
                    itemType = 1;
                    break;
                }
        }

        CreateButton();
    }

    void Update()
    {
        if(BreakStatus == 3)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                buttonImage.gameObject.SetActive(true);
                float distance = Vector3.Distance(this.transform.position, Camera.main.transform.position);
                Vector3 namePose = Camera.main.WorldToScreenPoint(this.transform.position);
                buttonImage.transform.position = namePose;
            }
            else
            {
                buttonImage.gameObject.SetActive(false);
            }
        }     
        else buttonImage.gameObject.SetActive(false);
    }

    void OnMouseUp()
    {
        Player.NextItemPosition = transform;
        Player.WalkToFix();
    }

    void OnMouseOver()
    {
        if(Input.GetMouseButtonUp(1)){
            Player.NextItemPosition = transform;
            Player.RunToFix();
        }
    }

    void OnMouseEnter()
    {
        buttonImage.sprite = FixBtnSpriteHover;
    }
    void OnMouseExit()
    {
        buttonImage.sprite = FixBtnSprite;
    }

    public void Break(float damageDuration = 10)
    {
        BreakStatus = 2;
        if(damageDuration <= 3)
            animator.CrossFadeInFixedTime("damage", 0.3f);
        StartCoroutine(breakThis(damageDuration));
    }

    public void Fix()
    {
        BreakStatus = 1;    
        //possibly play fix animation here
        StartCoroutine(fixThis());
    }

    private IEnumerator breakThis(float damageDuration = 10)
    {
        while (Health > 0)
        {
            Health--;
            yield return new WaitForSeconds(damageDuration/100);
        }

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("damage"))
        {
            animator.CrossFadeInFixedTime("damage", 0.3f);
        }

        Health = 0;
        BreakStatus = 3;
        buttonImage.gameObject.SetActive(true);
        BrokenCount++;
    }

    private IEnumerator fixThis()
    {
        GetFixingIncrements();

        while(Health < 100)
        {
            Health++;
            yield return new WaitForSeconds(time);
        }

        Health = 100;
        BreakStatus = 0;
        //stop playing fix animation, go to idle
        animator.CrossFadeInFixedTime("repair", 0.3f);
        buttonImage.gameObject.SetActive(false);
        BrokenCount--;
    }

    private void CreateButton()
    {
        GameObject canvas = new GameObject("button Canvas");
        canvas.AddComponent<RectTransform>();
        canvas.AddComponent<Canvas>();
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        GameObject childObj = new GameObject();

        //Make block to be parent of this gameobject
        canvas.transform.parent = this.transform;
        childObj.transform.parent = canvas.transform;
        childObj.name = "buttonSprite";

        buttonImage = childObj.AddComponent<Image>();
        buttonImage.sprite = FixBtnSprite;
        buttonImage.rectTransform.sizeDelta = new Vector2(30, 30);
        buttonImage.gameObject.SetActive(false);
    }

    private void GetFixingIncrements()
    {
        if (itemType == 0 || itemType == 1) //fix or lift
        {
            if (upgradeLevel == 0) //5
            {
                time = 0.05f;
            }
            else if (upgradeLevel == 1) //4
            {
                time = 0.04f;
            }
            else if (upgradeLevel == 2) //3
            {
                time = 0.03f;
            }
            else //2
            {
                time = 0.02f;
            }
        }
        else //clean
        {
            if (upgradeLevel == 0) //6.3
            {
                time = 0.063f;
            }
            else if (upgradeLevel == 1) //4
            {
                time = 0.04f;
            }
            else if (upgradeLevel == 2) //2
            {
                time = 0.023f;
            }
        }
    }
}