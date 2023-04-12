using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilestoneReward : ScriptableObject
{
    private int dayRequirement;
    public string rewardName;
    public bool isLocked = true;

    public virtual void ApplyReward()
    {

    }

}
