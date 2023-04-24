using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyObject))]
public class EnemyEditor : Editor
{
    public enum Rarity
    {
        Common, Uncommon, Rare, Legendary, Mythic
    }

    public enum Difficulty
    {
        Easy, Normal, Hard, Elite, Insane
    }

    public Rarity thisRarity;
    public Difficulty thisDifficulty;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SerializedProperty rarity = serializedObject.FindProperty("rarity");

        switch(serializedObject.FindProperty("rarity").stringValue)
        {
            case "commmon":
            thisRarity = Rarity.Common;
            break;
            case "uncommon":
            thisRarity = Rarity.Uncommon;
            break;
            case "rare":
            thisRarity = Rarity.Rare;
            break;
            case "legendary":
            thisRarity = Rarity.Legendary;
            break;
            case "mythic":
            thisRarity = Rarity.Mythic;
            break;
        }

        thisRarity = (Rarity) EditorGUILayout.EnumPopup("Rarity", thisRarity);
        EditorGUILayout.Space();

        switch(thisRarity)
        {
            case Rarity.Common:
            rarity.stringValue = "common";
            break;
            case Rarity.Uncommon:
            rarity.stringValue = "uncommon";
            break;
            case Rarity.Rare:
            rarity.stringValue = "rare";
            break;
            case Rarity.Legendary:
            rarity.stringValue = "legendary";
            break;
            case Rarity.Mythic:
            rarity.stringValue = "mythic";
            break;
        }

        //Difficulty
        SerializedProperty difficulty = serializedObject.FindProperty("difficulty");

        switch(serializedObject.FindProperty("difficulty").stringValue)
        {
            case "easy":
            thisDifficulty = Difficulty.Easy;
            break;
            case "normal":
            thisDifficulty = Difficulty.Normal;
            break;
            case "hard":
            thisDifficulty = Difficulty.Hard;
            break;
            case "elite":
            thisDifficulty = Difficulty.Elite;
            break;
            case "insane":
            thisDifficulty = Difficulty.Insane;
            break;
        }

        thisDifficulty = (Difficulty) EditorGUILayout.EnumPopup("Difficulty", thisDifficulty);
        EditorGUILayout.Space();

        switch(thisDifficulty)
        {
            case Difficulty.Easy:
            difficulty.stringValue = "easy";
            break;
            case Difficulty.Normal:
            difficulty.stringValue = "normal";
            break;
            case Difficulty.Hard:
            difficulty.stringValue = "hard";
            break;
            case Difficulty.Elite:
            difficulty.stringValue = "elite";
            break;
            case Difficulty.Insane:
            difficulty.stringValue = "insane";
            break;
        }
        
        serializedObject.ApplyModifiedProperties();

    }

}
