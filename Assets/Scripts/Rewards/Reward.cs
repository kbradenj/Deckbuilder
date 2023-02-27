using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public Singleton singleton;

    protected virtual void Start()
    {
        singleton = GameObject.FindObjectOfType<Singleton>();
    }

    public virtual void PickReward(){

    }

    public virtual void Render()
    {
        
    }
}
