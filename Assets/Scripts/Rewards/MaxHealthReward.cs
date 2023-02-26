using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthReward : Reward
{


    public override void PickReward()
    {
        base.PickReward();
        Debug.Log("I chose a max health reward");
    }
}
