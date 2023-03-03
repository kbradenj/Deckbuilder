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
}
