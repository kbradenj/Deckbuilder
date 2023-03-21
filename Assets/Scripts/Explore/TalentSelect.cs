using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentSelect : MonoBehaviour
{
    public TalentOption talent;
    public Singleton singleton;

    void Awake()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
    }
    public void SelectTalent()
    {
        if(singleton.talents.ContainsKey(talent.talentName))
        {
            return;
        }
        else
        {
            singleton.talents.Add(talent.talentName, talent);
        }

    }
}
