

public class NormalEnemyPath : PathChoice
{
    public override void AssignEvent() 
    {
        timeCost = 120;
        pathChoiceType = "enemy";
        pathChoiceTitle = "Normal Enemy";
        difficulty = "normal";
        base.AssignEvent();
    }

    public override void Choice()
    {
        base.Choice();
        singleton.navigation.Navigate("Battle");
    }
}
