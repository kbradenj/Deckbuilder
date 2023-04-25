using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProfile : MonoBehaviour
{
    Singleton singleton;

    [SerializeField] TMP_Text healthText;

    private void Awake() {
        singleton = FindAnyObjectByType<Singleton>();   
        healthText.text = singleton.player.health.ToString() + "/" + singleton.player.maxHealth.ToString();
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }
}
