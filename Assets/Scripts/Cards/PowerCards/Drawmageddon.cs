using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawmageddon : Card
{

    private Singleton singleton;

    public override void Effect()
    {
        singleton = FindObjectOfType<Singleton>();
        Player player = singleton.player;
        player.drawSize += 2;
        player.bonusDraw += 2;
        if(player.bonusDraw == 2)
        {
            player.AddStatusIcon("draw", 2);
        }
        else
        {
            player.UpdateStatus();
        }
    }
}

