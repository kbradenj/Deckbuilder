using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IngredientItem : MonoBehaviour
{
    public TMP_Text neededQty;
    public TMP_Text ownedQty;
    public TMP_Text ingredientTitle;

    public Image image;
    public Card card;

    public void Render(Card card, int needed, int owned)
    {
        neededQty.text = needed.ToString();
        ownedQty.text = owned.ToString();
        ingredientTitle.text = card.cardName;
        image.sprite = card.cardImage;
    }
}
