using UnityEngine;

[CreateAssetMenu(fileName = "TotemOfFury", menuName = "Artifact/TotemOfFury")]
public class TotemOfFury : Artifact
{
    public override void Effect()
    {
        Player player = FindObjectOfType<Singleton>().player;
        player.totemOfFury = true;
    }

    public override void Activate()
    {
        ActionManager actionManager = FindObjectOfType<ActionManager>();
        Battle battleManager = FindObjectOfType<Battle>();
        foreach(Enemy enemy in battleManager.enemies)
        {
            actionManager.Attack(enemy, 5);
        }
        

    }
}
