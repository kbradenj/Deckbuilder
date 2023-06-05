using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Totem", menuName = "Artifact/Totem")]
public class Totem : Artifact
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Effect()
    {
        Character player = FindObjectOfType<Singleton>().player;
        FindAnyObjectByType<ActionManager>().AddEffect(player, 1, ref player.damageReduction, "damageReduction");
        Debug.Log(player.damageReduction);
    }
}
