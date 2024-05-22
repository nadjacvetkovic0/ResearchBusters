using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum EnemyType
{
    //template
    regular,
    heavy,
    light,
    flying,
    boss
}

[CreateAssetMenu(menuName = "EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public Sprite sprite;

    [Range(1, 50)] public float Speed = 5f;
    [Range(1, 50)] public float Health = 10f;

    [Range(1, 50)] public int CoinsDropped = 1;
    [Range(1, 50)] public int Damage = 1;

    public EnemyType EnemyType = EnemyType.regular;
}
