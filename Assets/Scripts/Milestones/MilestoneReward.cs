using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MilestoneReward : MonoBehaviour, IPointerClickHandler
{
    public Milestone milestone;
    public TMP_Text descriptionText;
    public TMP_Text titleText;
    public Image image;
    private Color defaultColor;
    public GameObject cardVisualPrefab;
    public GameObject imageArea;
    private GameObject cardVisual;

    MilestoneRewardSelector milestoneRewardSelector;
    void Start()
    {
        milestoneRewardSelector = FindObjectOfType<MilestoneRewardSelector>();
        descriptionText.text = milestone.description;

        if(milestone.type == "card")
        {
            Destroy(image.gameObject);
            cardVisual = GameObject.Instantiate(cardVisualPrefab, Vector2.zero, Quaternion.identity);
            cardVisual.GetComponent<CardBehavior>().RenderCard(milestone.card);
            RectTransform cardVisualRect = cardVisual.GetComponent<RectTransform>();
            cardVisualRect.localScale = new Vector3(.7f, .7f, .7f);
            cardVisualRect.position = new Vector2(cardVisualRect.position.x, 140);
            cardVisual.transform.SetParent(imageArea.transform);

        }
        else
        {
            image.sprite = milestone.image;
        }

        AssignTitle();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(milestoneRewardSelector.selectedReward != null)
        {
            milestoneRewardSelector.selectedReward.StopHighlight();
        }
        milestoneRewardSelector.selectedReward = this;
        GameObject.Find("ConfirmButton").GetComponent<Button>().interactable = true;
        Highlight();
    }

    public void AssignTitle()
    {
        string title;
        switch(milestone.type)
        {
            case "card":
            title = "Unlock Card";
            break;
            case "feature":
            title = "New Feature";
            break;
            case "recipe":
            title = "New Recipe";
            break;
            default:
            title = "No Title";
            break;
        }   
        titleText.text = title;
    }

    public void Highlight()
    {
        if(milestone.type == "card")
        {
            cardVisual.GetComponent<CardBehavior>().HighlightSelection();
        }
        else
        {
            defaultColor = image.color;
            image.color = new Color(0,255,0,50);
        }

    }

    public void StopHighlight()
    {
        if(milestone.type == "card")
        {
            cardVisual.GetComponent<CardBehavior>().StopHighlightSelection();
        }
        else
        {
            image.color = defaultColor;
        }

    }
}
