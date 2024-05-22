using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupInfoHUD : MonoBehaviour
{
    [SerializeField] private Image enemyIcon;
    [SerializeField] private TextMeshProUGUI enemyCount;
    
    public void Init(WaveGroup wave)
    {
        enemyIcon.sprite = wave.EnemyType.Stats.sprite;
        enemyCount.text = $"X {wave.EnemyCount}";
    }
}
