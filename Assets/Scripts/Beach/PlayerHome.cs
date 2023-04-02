using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHome : MonoBehaviour
{

    public Singleton singleton;
    public TMP_Text healthText;
    public TMP_Text strengthText;
    public TMP_Text levelText;
    public TMP_Text healAmountText;
    public TMP_Text healCostText;
    public TMP_Text homeDaylightCount;

    public GameObject playerProfilePrefab;

    public int healAmount = 50;
    public int healCost = 120;

  

    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    void Start(){
        singleton.AdjustDaylight();
        UpdateHealActionText();
        singleton.UpdateDayCount();
    }

    public void HomeHealPlayer()
    {
        
        if(singleton.CanSpendDaylight(healCost)){
            if(singleton.player.health == singleton.player.maxHealth)
            {
                Debug.Log("Already at Max Health");
            }
            else{
            singleton.HealPlayer(healAmount);
            singleton.AdjustDaylight(healCost);
            singleton.AdjustDaylight();
            }
        }
        else
        {
            Debug.Log("Not enought daylight");
        }
        
    }

    public void UpdateHealActionText()
    {
        healAmountText.text = "Heal: " + healAmount;
        healCostText.text = "Cost: " + healCost + " min";
    }


    public void Loiter(){
        singleton.AdjustDaylight(singleton.dayLeft);
    }

    public void PlayerProfile()
    {
        GameObject profile = GameObject.Instantiate(playerProfilePrefab, new Vector2 (Screen.width/2f, Screen.height/2f), Quaternion.identity);
        profile.transform.SetParent(GameObject.Find("Main Canvas").transform);
    }
}
