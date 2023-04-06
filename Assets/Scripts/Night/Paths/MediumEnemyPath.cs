using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediumEnemyPath : PathChoice
{
    public override void AssignEvent() 
    {
        timeCost = 120;
        pathChoiceType = "Medium Enemy";
        base.AssignEvent();
    }

    public override void Choice()
    {
        base.Choice();
        singleton.navigation.Navigate("Battle");
    }
}
