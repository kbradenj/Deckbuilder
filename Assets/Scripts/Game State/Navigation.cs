using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{

    public void Night()
    {
        SceneManager.LoadScene("Night");
    }

    public void Navigate(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
