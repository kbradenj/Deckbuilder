using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TotemOfProtection", menuName = "Artifact/TotemOfProtection")]
public class TotemOfProtection : Artifact
{

    public override void Effect()
    {
        Player player = FindObjectOfType<Singleton>().player;
        player.AddStatusIcon("halfBlock");
        player.halfBlock = true;
    }

    public override void RemoveEffect()
    {
       Player player = FindObjectOfType<Singleton>().player;
       player.halfBlock = false;
    }
}
