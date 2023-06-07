using UnityEngine;

[CreateAssetMenu(fileName = "TotemOfStrength", menuName = "Artifact/TotemOfStrength")]
public class TotemOfStrength : Artifact
{
    public override void Effect()
    {
        Player player = FindObjectOfType<Singleton>().player;
        FindAnyObjectByType<ActionManager>().AddEffect(player, 1, ref player.strength, "strength");
    }
}
