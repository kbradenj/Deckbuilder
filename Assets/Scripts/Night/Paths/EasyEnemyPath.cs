using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyEnemyPath : PathChoice
{
    public override void AssignEvent()
    {
        timeCost = 60;
        pathChoiceType = "Easy Enemy";
        base.AssignEvent();
    }

    public override void Choice()
    {
        base.Choice();
        singleton.navigation.Navigate("Battle");
    }
}
