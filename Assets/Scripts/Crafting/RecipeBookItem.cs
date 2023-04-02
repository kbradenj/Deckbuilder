using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeBookItem : MonoBehaviour
{
    public CraftingRecipe recipe;
    private Craft craft;


     public void RecipeNameClicked()
    {
        if(!recipe.isLocked)
        {
            craft = GameObject.FindObjectOfType<Craft>();
            craft.currentRecipeListItem = this.gameObject;
            craft.ShowRecipe(recipe);
        }
      
    }
}
