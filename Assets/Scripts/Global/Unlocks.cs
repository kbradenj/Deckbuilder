using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocks : MonoBehaviour
{   
    public Singleton singleton;


    public int craftAreaUnlockDay = 3;
    public bool isCraftingLocked = true;
    public bool isFirstPlaythrough = true;
    public bool isHealLocked = true;
    public bool isScavengeLocked = true;

    public void CheckUnlocks(){

        singleton = FindAnyObjectByType<Singleton>();
        //unlock crafting
        if(isCraftingLocked)
        {
            isCraftingLocked = CanUnlockCrafting();
        }
    }

    private bool CanUnlockCrafting()
    {
        if(!isFirstPlaythrough)
        {
            return false;
        }
        else if (isFirstPlaythrough && singleton.dayCount >= craftAreaUnlockDay)
        {
            return false;
        }
        return true;
    }
    



}
