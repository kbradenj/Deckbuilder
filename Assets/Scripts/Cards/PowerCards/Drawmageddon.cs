using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drawmageddon", menuName = "Card/PowerCard/Drawmageddon")]
public class Drawmageddon : Card
{
    public override void Effect()
    {
        Player player = GameObject.FindObjectOfType<Player>();
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

