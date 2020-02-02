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
        StartCoroutine("ShowText");
    }

    IEnumerator ShowText()
    {
        int moneyGained = HouseHealth.HouseHealthNum;
        int repGained = Mathf.RoundToInt(moneyGained/2);
        int currentMoney = Game.current.Money;
        int currentRep = Game.current.Reputation;
        while(moneyGained != 0)
        {
            moneyText.text = currentMoney + " +" + moneyGained;
            repText.text = currentRep + " +" + repGained;
            currentRep+=2;
            currentMoney+=2;
            moneyGained-=2;
            repGained-=2;
            yield return null;
        }
    }
    public void BackToShop()
    {
        SceneManager.LoadScene(1);
    }
    // Update is called once per frame
}
