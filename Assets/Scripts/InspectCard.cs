using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InspectCard : MonoBehaviour
{

    public TMP_Text cardNameField;
    public TMP_Text descriptionField;
    public TMP_Text costField;
    public RectTransform darkenedBG;
    public Image image;
    public Card card;

    //Populate Card Data to UI
    public void RenderCard(Card c)
    {
        darkenedBG.sizeDelta = new Vector2(Screen.width, Screen.height);
        card = c;
        cardNameField.text = c.cardName;
        descriptionField.text = c.FormatString();
        if(c.actionList[0] == "xattack" || c.actionList[0] == "xblock"){
            costField.text = "X"; 
        }
        else{
            costField.text = c.cardCost.ToString(); 
        }
        image.sprite = c.cardImage;
    }
    
    public void DestroyCard(){
        Destroy(gameObject);
    }
}
