using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryPath : PathChoice
{
    public override void AssignEvent()
    {
        timeCost = 60;
        pathChoiceType = "Story";
        base.AssignEvent(); 
    }

    public override void Choice()
    {
        base.Choice();
        singleton.navigation.Navigate("Story");
    }
}
