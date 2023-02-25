using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InspectCard : MonoBehaviour
{
    //TMPro
    public TMP_Text cardNameField;
    public TMP_Text descriptionField;
    public TMP_Text costField;

    public GameObject definitionPrefab; 
    public GameObject definitionArea;

    //Scripts
    public Glossary glossary;

    public RectTransform darkenedBG;
    public Image image;
    public Sprite cardOverlay;
    public GameObject cardOverlayObject;
    public Card card;

    private int counter = 0;

    void Awake()
    {
        glossary = GameObject.FindObjectOfType<Glossary>();
    }

    //Populate Card Data to UI
    public void RenderCard(Card c)
    {
        darkenedBG.sizeDelta = new Vector2(Screen.width, Screen.height);
        card = c;
        cardNameField.text = c.cardName;
        cardOverlay = c.cardOverlay;
        descriptionField.text = c.FormatString();
        if(c.actionList[0] == "xattack" || c.actionList[0] == "xblock"){
            costField.text = "X"; 
        }
        else{
            costField.text = c.cardCost.ToString(); 
        }
        image.sprite = c.cardImage;
        LoadDefinitions();
        float currentScaleX = gameObject.transform.localScale.x;
        float currentScaleY = gameObject.transform.localScale.y;
        gameObject.transform.localScale = new Vector2(currentScaleX * 1.5f, currentScaleY * 1.5f);
    }
    
    public void DestroyCard(){
        Destroy(gameObject);
    }

    public void LoadDefinitions()
    {
        foreach(string action in card.actionList)
        {
            GameObject newDefinition = GameObject.Instantiate(definitionPrefab, new Vector2(Screen.width/2f,Screen.height/2f), Quaternion.identity) as GameObject;
            Definition definition = newDefinition.GetComponent<Definition>();
            definition.definitionTitle.text = card.actionList[counter];
            definition.definitionText.text = glossary.GetDefinition(card.actionList[counter]);
            definition.transform.SetParent(definitionArea.transform);
            counter++;
        }
    }
}
