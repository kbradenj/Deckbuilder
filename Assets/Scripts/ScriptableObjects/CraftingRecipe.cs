using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Craft Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public string item;
    public CraftingMaterial[] craftingMaterials;
    public Card resultItem;
}
