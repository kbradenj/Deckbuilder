
public class StoryPath : PathChoice
{
    public override void AssignEvent()
    {
        timeCost = 60;
        pathChoiceType = "story";
        pathChoiceTitle = "Story";
        base.AssignEvent(); 
    }

    public override void Choice()
    {
        base.Choice();
        singleton.navigation.Navigate("Story");
    }
}
