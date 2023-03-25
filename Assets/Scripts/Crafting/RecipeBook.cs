using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeBook : MonoBehaviour
{

    public GameObject recipeTextPrefab;
    public GameObject recipeList;
    public Singleton singleton;

    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
        LoadRecipes();
    }

    public void LoadRecipes()
    {
        ClearRecipes();
        recipeList = GameObject.Find("Recipe List");
        foreach(CraftingRecipe recipe in singleton.recipeDatabase)
        {
            GameObject recipeObject = GameObject.Instantiate(recipeTextPrefab, Vector2.zero, Quaternion.identity);
            TMP_Text recipeText = recipeObject.GetComponent<TMP_Text>();
            if(recipe.isLocked)
            {
                 recipeText.text = "?????????????";
            }
            else
            {
                recipeText.text = recipe.resultItem.cardName;
            }
            recipeObject.transform.SetParent(recipeList.transform);
        }
    }

    public void ClearRecipes()
    {
               GameObject[] recipeNames = GameObject.FindGameObjectsWithTag("Recipe List Item");
               if(recipeNames.Length > 0)
               {
                    foreach(GameObject recipe in recipeNames)
                    {
                            Destroy(recipe);
                    }
               }
              
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void Unlock()
    {
        singleton.unlockedRecipes.Add(singleton.recipeDatabase[0]);
        singleton.recipeDatabase[0].isLocked = false;
        LoadRecipes();
    }
}
