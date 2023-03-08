using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{

    public void GoToCrafting()
    {
        SceneManager.LoadScene("Crafting");
    }
    
    public void GoToBattle()
    {
        SceneManager.LoadScene("Battle");
    }

    public void GoToCamp()
    {
        SceneManager.LoadScene("Home");  
    }

    public void Night()
    {
        SceneManager.LoadScene("Night");
    }

    public void GoToScavenge()
    {
        SceneManager.LoadScene("Scavenge");
    }
}
