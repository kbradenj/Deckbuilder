using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatReward : Reward
{


    protected override void Start()
    {
        base.Start();
    }

    public override void PickReward()
    {
        base.PickReward();
        singleton.player.maxHealth += 10;
        Destroy(this.gameObject);
    }
}
