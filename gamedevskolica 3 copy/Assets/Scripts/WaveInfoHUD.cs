using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfoHUD : MonoBehaviour
{
    void Start() => DeleteAllItems();

    public void DeleteAllItems()
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
    }
}
