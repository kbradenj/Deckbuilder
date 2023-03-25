using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Craft Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public int timeCost;
    public CraftingMaterial[] craftingMaterials;
    public Card resultItem;
    public bool isLocked = true;
}
