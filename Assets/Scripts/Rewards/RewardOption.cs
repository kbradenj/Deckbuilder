using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardOption : MonoBehaviour
{
    public string rewardType;
    public TMP_Text optionText;
    public Image optionImage;

    public void UpdateText(string optionName)
    {
        optionText = gameObject.GetComponentInChildren<TMP_Text>();
        optionText.text = optionName;
    }

    public void UpdateImage(Sprite sprite)
    {
        optionImage = Transform.FindObjectOfType<Image>();
        optionImage.sprite = sprite;
    }

     public void RenderOption(string rewardType)
    {
        string message;
        string imagePath;
        switch(rewardType)
        {
            case "card":
            message = "A Nice Shiny New Card";
            imagePath = "UI/Icons/card";
            break;

            case "multiCard":
            message = "Dirt and Leaves, Crafty cards";
            imagePath = "UI/Icons/cards";
            break;

            case "playerStat":
            message = "A Totally Legit Steroid";
            imagePath = "UI/Icons/increase";
            break;

            default:
            message = "mystery surprise!";
            imagePath = "UI/Icons/card";
            break;
        }
        UpdateText(message);
        UpdateImage(Resources.Load<Sprite>(imagePath));
    }
}
