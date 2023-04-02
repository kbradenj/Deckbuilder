using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public void Close()
    {
        Destroy(this.gameObject);
    }
}
