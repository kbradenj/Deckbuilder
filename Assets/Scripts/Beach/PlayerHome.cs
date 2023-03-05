using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHome : MonoBehaviour
{

    public Singleton singleton;
    public TMP_Text healthText;
    public TMP_Text healAmountText;
    public TMP_Text healCostText;
    public TMP_Text homeDaylightCount;

    public int healAmount = 50;
    public int healCost = 120;

  

    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    void Start(){
        UpdatePlayerStats();
        UpdateHealActionText();
        UpdateHomeDaylightCount();
    }

    public void UpdatePlayerStats()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        healthText.text = "Health: " + singleton.player.health + "/" + singleton.player.maxHealth;
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
            UpdatePlayerStats();
            UpdateHomeDaylightCount();
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

    //Update UI to show current daylight
    public void UpdateHomeDaylightCount(){
        homeDaylightCount.text = singleton.dayLeft.ToString() + " minutes left in the day";
    }

    public void Loiter(){
        singleton.AdjustDaylight(singleton.dayLeft);
        UpdateHomeDaylightCount();
    }
}
