using UnityEngine;

[CreateAssetMenu(fileName = "TotemOfSpeed", menuName = "Artifact/TotemOfSpeed")]
public class TotemOfSpeed : Artifact
{
    public override void Effect()
    {
        Player player = FindObjectOfType<Singleton>().player;
        player.evade++;
        player.AddStatusIcon("evade", player.evade);
    }
}
