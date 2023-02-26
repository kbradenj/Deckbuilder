using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glossary : MonoBehaviour
{
    public string GetDefinition(string word)
    {
        switch(word)
        {
            case "attack":
            return "deal damage to target";
            case "xattack":
            return "Spend all your mana. Deal damage * the amount spent.";
            case "block":
            return "apply block to yourself";
            case "vulnerable":
            return "Vulnerable receives 1.25x damage from attacks.";
            case "weak":
            return "Weak reduces damage dealt by 50%";
            case "strength":
            return "+1 Damage for each stack of Strength";
            default:
            return "No available definition";
        }
    }
}
