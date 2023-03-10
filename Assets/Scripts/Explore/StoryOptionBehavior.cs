using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryOptionBehavior : MonoBehaviour
{
    public TMP_Text optionText;
    public TMP_Text effectText;

    public StoryOption storyOption;

    void Start()
    {
        DisplayOption();
    }

    public void DisplayOption()
    {
        optionText.text = storyOption.optionTitle;
        effectText.text = storyOption.optionEffectText;
    }

    public void ChooseReward()
    {
        Debug.Log("Reward Chosen");
    }
}
