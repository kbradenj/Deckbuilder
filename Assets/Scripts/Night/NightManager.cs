using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightManager : MonoBehaviour
{
    Singleton singleton;

    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        if(singleton.nightLeft <= 0)
        {
            singleton.navigation.Navigate("Home");
        }
        else
        {
            singleton.AdjustMoonlight();
        }
    }

    public void UpdateDayCount()
    {
        singleton.dayLeft = singleton.maxDaylight;
        singleton.dayCount++;
    }
}
