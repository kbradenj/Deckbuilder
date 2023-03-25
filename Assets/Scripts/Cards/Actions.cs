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

    //Vulnerable
    public void Vulnerable(Character target, int amount)
    {
        if(target.vulnerable == 0){
            target.AddStatusIcon("vulnerable", amount);
        }
        target.vulnerable += amount;
        target.UpdateStatus();
    }

    //Weak
    public void Weak(Character target, int amount)
    {
        if(target.weak == 0){
            target.AddStatusIcon("weak", amount);
        }
        target.weak += amount;
        target.weaknessMod = .5f;
        target.UpdateStatus();
    }

    //Strength
    public void Strength(Character target, int amount)
    {
        if(target.strength == 0)
        {
            target.AddStatusIcon("strength", amount);
        }
        target.strength += amount;
        target.UpdateStatus();
    }

    //Poison
    public void Poison(Character target, int amount)
    {
        if(amount == 0)
        {
            target.AddStatusIcon("poison", amount);
        }
    }

   
}
