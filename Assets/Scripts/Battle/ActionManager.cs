using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class ActionManager : MonoBehaviour
{
    Player player;
    CardManager cardManager;


    private void Start() {
        Singleton singleton = FindObjectOfType<Singleton>();
        player = singleton.player;
        cardManager = FindAnyObjectByType<CardManager>();
    }

    //Will Target Die?
    public bool WillTargetDie(Character target, int attack)
    {
        int damageTaken = attack - target.block;
        return damageTaken >= target.health;
    }

    //Attack
    public void Attack(Character target, int amount, int times = 1)
    {
        if(target.vulnerable > 0){
            amount = (int)Math.Ceiling(amount * target.vulnerableMod);
        }
        for(int i = 0; i < times; i++)
        {
            if(WillTargetDie(target, amount))
            {
                target.Death();
                break;
            }
            int unblockedDamage = target.block - amount;
            target.block = (unblockedDamage <=0) ? 0 : unblockedDamage;
            target.health += (unblockedDamage <= 0) ? unblockedDamage : 0;
        }
        target.UpdateStats();
    }

    //Block
    public void Block(Character target, int amount)
    {
        target.block += amount;
        target.UpdateStats();
    }

    public void AddEffect(Character target, int amount, ref int statusInt, string statusType)
    {
         if(statusInt == 0)
        {
            target.AddStatusIcon(statusType, amount);
        }
        statusInt += amount;
        target.UpdateStatus();
        target.UpdateStatus();
        cardManager.UpdateHandCards();
    }

   
}
