using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weakpoint : MonoBehaviour
{
    [SerializeField] private float maxHealth = 5f;
    private float health;

    private SpriteRenderer sprite;

    void Awake() => sprite = GetComponent<SpriteRenderer>();
    public void Spawn()
    {
        health = maxHealth;
        gameObject.SetActive(true);
    }
    public void AttackWeakpoint(float damage)
    {
        health -= damage;
        if (health <= 0) gameObject.SetActive(false);
        else StartCoroutine(AnimateDamage());
    }
    private IEnumerator AnimateDamage()
    {
        var color = sprite.color;
        if (color == Color.red)
            yield break;
        sprite.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sprite.color = color;
    }
}
