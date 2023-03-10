using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Story", menuName = "Story")]
public class Story : ScriptableObject
{
    public Sprite sprite;
    public List<StoryOption> storyOptions;
    [TextArea(1,8)] 
    public List<string> paragraphs;
}

[System.Serializable]
public class StoryOption
{
    public string optionTitle;
    public string optionEffectText;
    public List<OptionEffect> effects = new List<OptionEffect>();
}

[System.Serializable]
public class OptionEffect
{
    public string effectKey;
    public int amount;
}

