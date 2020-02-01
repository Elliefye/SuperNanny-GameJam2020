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
    //possible locking to prevent multiple characters from accessing the same object at once?
    public GameObject Lock = null;
    [SerializeField] Sprite FixBtnSprite;
    [SerializeField] CharacterMovement Player;
    
    private Image buttonImage;
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

        CreateButton();
    }

    void Update()
    {
        if(BreakStatus == 3)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(this.transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                float distance = Vector3.Distance(this.transform.position, Camera.main.transform.position);
                Vector3 namePose = Camera.main.WorldToScreenPoint(this.transform.position);
                buttonImage.transform.position = namePose;
            }
            else
            {
                buttonImage.gameObject.SetActive(false);
            }
        }        
    }

    void OnMouseDown()
    {
        Player.NextItemPosition = transform;
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
        buttonImage.gameObject.SetActive(true);
        BrokenCount++;
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
    }
}