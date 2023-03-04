using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatReward : Reward
{


    protected override void Awake()
    {
        base.Awake();
    }

    public override void PickReward()
    {
        base.PickReward();
        singleton.player.maxHealth += 10;
        singleton.player.health += 10;
    }
}
