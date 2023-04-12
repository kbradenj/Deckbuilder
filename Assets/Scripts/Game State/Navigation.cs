using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    Singleton singleton;

    public void Night()
    {
        singleton = FindObjectOfType<Singleton>();
        singleton.isNight = true;
        SceneManager.LoadScene("Night");
    }

    public void Navigate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
