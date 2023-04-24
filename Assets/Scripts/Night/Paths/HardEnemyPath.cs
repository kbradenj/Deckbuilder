
public class HardEnemyPath : PathChoice
{
    public override void AssignEvent() 
    {
        timeCost = 180;
        pathChoiceType = "enemy";
        pathChoiceTitle = "Hard Enemy";
        difficulty = "hard";
        base.AssignEvent();
    }

    public override void Choice()
    {
        base.Choice();
        singleton.navigation.Navigate("Battle");
    }
}
