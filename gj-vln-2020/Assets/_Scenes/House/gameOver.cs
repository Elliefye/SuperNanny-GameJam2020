using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOver : MonoBehaviour
{
    [SerializeField] Text moneyText;
    [SerializeField] Text repText;
    // Start is called before the first frame update
    void Start()
    {
        ShowText();
    }

    void ShowText()
    {
        int moneyGained = HouseHealth.HouseHealthNum;
      
        
            moneyText.text =  moneyGained.ToString();
            repText.text = Mathf.RoundToInt(moneyGained/2).ToString();

            Game.current.Money += moneyGained;
            Game.current.Reputation +=Mathf.RoundToInt(moneyGained/2) ;
            Game.SaveGame();
         
            
        
    }
    public void BackToShop()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
}
