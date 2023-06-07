using UnityEngine;

[CreateAssetMenu(fileName = "TotemOfWeakness", menuName = "Artifact/TotemOfWeakness")]
public class TotemOfWeakness : Artifact
{
    public override void Effect()
    {
        Battle battleManager = FindObjectOfType<Battle>();
        ActionManager actionManager = FindObjectOfType<ActionManager>();
        Enemy enemy = battleManager.TargetRandomEnemy();
        actionManager.AddEffect(enemy, 3, ref enemy.weak, "weak");
        
    }
}
