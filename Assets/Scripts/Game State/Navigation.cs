using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    Singleton singleton;

    void Awake() {
        singleton = FindObjectOfType<Singleton>();
    }

    public void Night()
    {
        singleton.isNight = true;
        SceneManager.LoadScene("Night");
    }

    public void Navigate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
