using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletStats")]
public class BulletStats : ScriptableObject
{
    [Range(1, 100)] public float speed = 10f;
    [Range(0, 100)] public float damage = 5f;
}
