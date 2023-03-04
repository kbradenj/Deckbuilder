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
        optionImage = gameObject.GetComponentInChildren<Image>();
        optionImage.sprite = sprite;
    }

     public void GetOptionMessage(string rewardType)
    {
        string message;
        switch(rewardType)
        {
            case "card":
            message = "A Nice Shiny New Card";
            break;

            case "multiCard":
            message = "Dirt and Leaves, Crafty cards";
            break;

            case "playerStat":
            message = "A Totally Legit Steroid";
            break;

            default:
            message = "mystery surprise!";
            break;
        }
        UpdateText(message);
    }
}
