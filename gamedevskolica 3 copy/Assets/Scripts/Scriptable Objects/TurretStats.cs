using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TurretStats")]
public class TurretStats : ScriptableObject
{
    public string name = "";
    public Sprite sprite;
    [Range(.01f, 3f)] public float shootDelay = .1f;
    [Range(1f, 20f)] public float range = .1f;
    [Range(1, 100)] public int price = 5;
    public EnemyType[] TargetedTypes;
}
