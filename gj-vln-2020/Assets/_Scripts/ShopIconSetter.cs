using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopIconSetter : MonoBehaviour
{
    [SerializeField]
    private Text playerMoney;
    [SerializeField]
    private Text playerRep;

    [SerializeField]
    private Image repairImg;
    [SerializeField]
    private Text repairPrice;
    [SerializeField]
    private Text repairDesc;
    [SerializeField]
    private Button repairBtn;
    public Sprite[] RepairSprites = new Sprite[3];


    [SerializeField]
    private Image strengthImg;
    [SerializeField]
    private Text strengthPrice;
    [SerializeField]
    private Text strengthDesc;
    [SerializeField]
    private Button strengthBtn;
    public Sprite[] StrengthSprites = new Sprite[3];

    [SerializeField]
    private Image cleanImg;
    [SerializeField]
    private Text cleanPrice;
    [SerializeField]
    private Text cleanDesc;
    [SerializeField]
    private Button cleanBtn;
    public Sprite[] CleanSprites = new Sprite[2];


    [SerializeField]
    private Image staminaImg;
    [SerializeField]
    private Text staminaPrice;
    [SerializeField]
    private Text staminaDesc;
    [SerializeField]
    private Button staminaBtn;
    public Sprite[] StaminaSprites = new Sprite[3];


    void Start()
    {
        playerMoney.text = Game.current.Money.ToString();
        playerRep.text = Game.current.Reputation.ToString();

        SetRepairItem();
        SetSrengthItem();
        SetCleanItem();
        SetStaminaItem();
        
    }

    private void SetRepairItem()
    {
        switch (Game.current.RepairLevel)
        {
            case 0: //bronze book
                {
                    repairImg.sprite = RepairSprites[0];
                    repairPrice.text = "100";
                    repairDesc.text = "Repair electronics a little faster than a snail's pace";

                    if (Game.current.Money <= 100)
                    {
                        repairBtn.interactable = false;
                    }

                    break;
                }
            case 1: //silver book
                {
                    repairImg.sprite = RepairSprites[1];
                    repairPrice.text = "200";
                    repairDesc.text = "Repair electronics like a mediocre engineer";

                    if (Game.current.Money <= 200)
                    {
                        repairBtn.interactable = false;
                    }

                    break;
                }
            case 2: //gold book
                {
                    repairImg.sprite = RepairSprites[2];
                    repairPrice.text = "300";
                    repairDesc.text = "Repair electronics at the speed of light";

                    if (Game.current.Money <= 300)
                    {
                        repairBtn.interactable = false;
                    }

                    break;
                }
            default: //gold book, disabled btn
                {
                    repairImg.sprite = RepairSprites[2];
                    repairPrice.text = "Maxed";
                    repairDesc.text = "Repair electronics at the speed of light";
                    repairBtn.interactable = false;
                    break;
                }
        }
    }

    private void SetSrengthItem()
    {
        switch (Game.current.StrengthLevel)
        {
            case 0: //bronze
                {
                    strengthImg.sprite = StrengthSprites[0];
                    strengthPrice.text = "100";
                    strengthDesc.text = "Pick up heavy objects faster";

                    if (Game.current.Money <= 100)
                    {
                        strengthBtn.interactable = false;
                    }

                    break;
                }
            case 1: //silver
                {
                    strengthImg.sprite = StrengthSprites[1];
                    strengthPrice.text = "200";
                    strengthDesc.text = "Pick up heavy objects like a weightlifter";

                    if (Game.current.Money <= 200)
                    {
                        strengthBtn.interactable = false;
                    }

                    break;
                }
            case 2: //gold
                {
                    strengthImg.sprite = StrengthSprites[2];
                    strengthPrice.text = "300";
                    strengthDesc.text = "Get the strength of Hercules";

                    if (Game.current.Money <= 300)
                    {
                        strengthBtn.interactable = false;
                    }

                    break;
                }
            default: //gold, disabled btn
                {
                    strengthImg.sprite = StrengthSprites[2];
                    strengthPrice.text = "Maxed";
                    strengthDesc.text = "Get the strength of Hercules";
                    strengthBtn.interactable = false;
                    break;
                }
        }
    }

    private void SetCleanItem()
    {
        switch (Game.current.CleaningLevel)
        {
            case 0: //bronze
                {
                    cleanImg.sprite = CleanSprites[0];
                    cleanPrice.text = "100";
                    cleanDesc.text = "Purchase a broom for more efficient sweeping";

                    if (Game.current.Money <= 100)
                    {
                        cleanBtn.interactable = false;
                    }

                    break;
                }
            case 1: //silver
                {
                    cleanImg.sprite = CleanSprites[1];
                    cleanPrice.text = "200";
                    cleanDesc.text = "This vacuum will get everything pristine in no time";

                    if (Game.current.Money <= 200)
                    {
                        cleanBtn.interactable = false;
                    }

                    break;
                }
            default: //gold, disabled btn
                {
                    cleanImg.sprite = CleanSprites[1];
                    cleanPrice.text = "Maxed";
                    cleanDesc.text = "This vacuum will get everything pristine in no time";
                    cleanBtn.interactable = false;
                    break;
                }
        }
    }

    private void SetStaminaItem()
    {
        switch (Game.current.StaminaLevel)
        {
            case 0: //broom
                {
                    staminaImg.sprite = StaminaSprites[0];
                    staminaPrice.text = "100";
                    staminaDesc.text = "Get comfortable shoes for more efficient running";

                    if (Game.current.Money <= 100)
                    {
                        staminaBtn.interactable = false;
                    }

                    break;
                }
            case 1: //vacuum
                {
                    staminaImg.sprite = StaminaSprites[1];
                    staminaPrice.text = "200";
                    staminaDesc.text = "Train your lungs to run longer without getting out of breath";

                    if (Game.current.Money <= 200)
                    {
                        staminaBtn.interactable = false;
                    }

                    break;
                }
            case 2: //gold
                {
                    staminaImg.sprite = StaminaSprites[2];
                    staminaPrice.text = "300";
                    staminaDesc.text = "Grow a third lung through the power of modern technology";

                    if (Game.current.Money <= 300)
                    {
                        staminaBtn.interactable = false;
                    }

                    break;
                }
            default: //vacuum, disabled btn
                {
                    staminaImg.sprite = StaminaSprites[2];
                    staminaPrice.text = "Maxed";
                    staminaDesc.text = "Grow a third lung through the power of modern technology";
                    staminaBtn.interactable = false;
                    break;
                }
        }
    }

    public void RepairBtnClicked()
    {
        if (Game.current.Money >= int.Parse(repairPrice.text))
        {
            Game.current.Money -= int.Parse(repairPrice.text);
            playerMoney.text = Game.current.Money.ToString();
            Game.current.RepairLevel++;
            Game.SaveGame();
            SetRepairItem();
        }
    }

    public void StrengthBtnClicked()
    {
        if (Game.current.Money >= int.Parse(strengthPrice.text))
        {
            Game.current.Money -= int.Parse(strengthPrice.text);
            playerMoney.text = Game.current.Money.ToString();
            Game.current.StrengthLevel++;
            Game.SaveGame();
            SetSrengthItem();
        }
    }

    public void CleanBtnClicked()
    {
        if (Game.current.Money >= int.Parse(cleanPrice.text))
        {
            Game.current.Money -= int.Parse(cleanPrice.text);
            playerMoney.text = Game.current.Money.ToString();
            Game.current.CleaningLevel++;
            Game.SaveGame();
            SetCleanItem();
        }
    }

    public void StaminaBtnClicked()
    {
        if (Game.current.Money >= int.Parse(staminaPrice.text))
        {
            Game.current.Money -= int.Parse(staminaPrice.text);
            playerMoney.text = Game.current.Money.ToString();
            Game.current.StaminaLevel++;
            Game.SaveGame();
            SetStaminaItem();
        }
    }

    public void StartClicked()
    {
        SceneManager.LoadScene(2);
    }

    public void BackClicked()
    {
        SceneManager.LoadScene(0);
    }
}
