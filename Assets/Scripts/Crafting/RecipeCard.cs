using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RecipeCard : CraftCard
{


    public override void OnPointerClick(PointerEventData eventData)
    {
        if(!isDisabled){
            craft.MakeFromRecipe(this);
            craft.RemoveCard(craft.inventoryCards, this);
        }
    }

    public override void Render()
    {
        image = gameObject.GetComponent<Image>();
        qtyText = gameObject.GetComponentInChildren<TMP_Text>();
        image.sprite = card.cardImage;
        qtyText.text = qty.ToString();
    }

    public override void ToggleDisable()
    {
        if(isDisabled)
        {
            image.color = default;
        }
        else
        {
            image.color = craft.disabledColor;
        }
    }

}
