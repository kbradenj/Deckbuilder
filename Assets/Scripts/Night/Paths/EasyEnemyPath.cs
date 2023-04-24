
public class EasyEnemyPath : PathChoice
{
    public override void AssignEvent()
    {
        timeCost = 60;
        pathChoiceType = "enemy";
        pathChoiceTitle = "Easy Enemy";
        difficulty = "easy";
        base.AssignEvent();
    }

    public override void Choice()
    {
        base.Choice();
        
        singleton.navigation.Navigate("Battle");
    }
}
