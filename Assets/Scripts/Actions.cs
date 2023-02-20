using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Actions : MonoBehaviour
{
    Player player;

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
            amount = (int)Math.Ceiling(amount * 1.25);
        }
        for(int i = 0; i < times; i++)
        {
            if(WillTargetDie(target, amount))
            {
                Destroy(target.gameObject);
                break;
            }
            else
            {   
               
                int unblockedDamage = target.block - amount;
                if(unblockedDamage <= 0)
                {
                    target.block = 0;
                    target.health += unblockedDamage;
                } else
                {
                    target.block -= amount;
                }
            }
        }
        target.UpdateStats();
    }

    //Block
    public void Block(Character target, int amount)
    {
        target.block += amount;
        target.UpdateStats();
    }

    //Vulnerable
    public void Vulnerable(Character target, int amount)
    {
        if(target.vulnerable == 0){
        target.AddStatusIcon("vulnerable");
        }
        target.vulnerable += amount;
    }
}
